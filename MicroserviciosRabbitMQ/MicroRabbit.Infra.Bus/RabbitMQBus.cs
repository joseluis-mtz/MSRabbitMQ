using MediatR;
using MicroRabbit.Domain.Core.Bus;
using MicroRabbit.Domain.Core.Comandos;
using MicroRabbit.Domain.Core.Eventos;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace MicroRabbit.Infra.Bus
{
    // sealed: No permite heredar e implementa interfaz
    public sealed class RabbitMQBus : IEventBus
    {
        private readonly RabbitConfig _rabbitConfig;
        private readonly IMediator _mediator; // Crear referencia del Mediator
        // Diccionario para manejar eventos del Bus
        private readonly Dictionary<string, List<Type>> _manejadores;
        // Lista que almacena los tipos de eventos
        private readonly List<Type> _tiposEvento;

        public RabbitMQBus(IMediator mediator, IOptions<RabbitConfig> ajustes)
        {
            // Inyeccion de los objetos
            _mediator = mediator;
            _manejadores = new Dictionary<string, List<Type>>();
            _tiposEvento = new List<Type>();
            _rabbitConfig = ajustes.Value;
        }

        public Task EnviarComando<T>(T Comando) where T : Comandos
        {
            return _mediator.Send(Comando);
        }

        public void Leer<T, TH>()
            where T : Eventos
            where TH : IManejadorEventos<T>
        {
            var NombreEvento = typeof(T).Name; // Obtener nombre del avento o Queue
            var ManejadorTipo = typeof(TH); // Obtener el objeto manejador
            if (!_tiposEvento.Contains(typeof(T)))
            {
                _tiposEvento.Add(typeof(T));
            }
            if (!_manejadores.ContainsKey(NombreEvento))
            {
                _manejadores.Add(NombreEvento, new List<Type>());
            }
            if (_manejadores[NombreEvento].Any(s => s.GetType() == ManejadorTipo))
            {
                throw new ArgumentException($"El handler exception {ManejadorTipo.Name} ya fue registrado antes por '{NombreEvento}'", nameof(ManejadorTipo));
            }
            _manejadores[NombreEvento].Add(ManejadorTipo);
            IniciarConsumoBasico<T>();
        }
        private void IniciarConsumoBasico<T>() where T : Eventos
        {
            var Fabrica = new ConnectionFactory
            {
                HostName = _rabbitConfig.Hostname,
                UserName = _rabbitConfig.Usuario,
                Password = _rabbitConfig.Password,
                DispatchConsumersAsync = true, // Se hace el dispach del consumer de manera asincrona
            };
            var Conexion = Fabrica.CreateConnection();
            var Canal = Conexion.CreateModel();

            var NombreEvento = typeof(T).Name;
            Canal.QueueDeclare(NombreEvento, false, false, false, null);
            var Consumer = new AsyncEventingBasicConsumer(Canal);
            Consumer.Received += Consumer_Received;
            Canal.BasicConsume(NombreEvento, true, Consumer);
        }

        private async Task Consumer_Received(object sender, BasicDeliverEventArgs e)
        {
            var NombreEvento = e.RoutingKey;
            var Mensaje = Encoding.UTF8.GetString(e.Body.ToArray());
            try
            {
                await ProcesarEvento(NombreEvento, Mensaje).ConfigureAwait(false);
            }
            catch (Exception varEx)
            {
                var Error = varEx.Message;
            }
        }

        private async Task ProcesarEvento(string nombreEvento, string mensaje)
        {
            if (_manejadores.ContainsKey(nombreEvento))
            {
                var Subscritores = _manejadores[nombreEvento];
                foreach (var item in Subscritores)
                {
                    var Manejador = Activator.CreateInstance(item);
                    if (Manejador == null) continue;
                    var TipoEvento = _tiposEvento.SingleOrDefault(t => t.Name == nombreEvento);
                    var EEvento = JsonConvert.DeserializeObject(mensaje, TipoEvento);
                    var TipoConcreto = typeof(IManejadorEventos<>).MakeGenericType(TipoEvento);
                    await (Task)TipoConcreto.GetMethod("Manejador").Invoke(Manejador, new object[] { EEvento});
                }
            }
        }

        public void Publicar<T>(T evento) where T : Eventos
        {
            var Fabrica = new ConnectionFactory
            {
                HostName = _rabbitConfig.Hostname,
                Password = _rabbitConfig.Password,
                UserName = _rabbitConfig.Usuario,
            };

            using (var Conexion = Fabrica.CreateConnection())
            using (var Canal = Conexion.CreateModel())
            {
                var NombreEvento = evento.GetType().Name; // Obtiene el nombre en tiempo de ejecucion
                Canal.QueueDeclare(NombreEvento, false, false, false, null);
                var Mensaje = JsonConvert.SerializeObject(evento);
                var Cuerpo = Encoding.UTF8.GetBytes(Mensaje);
                Canal.BasicPublish("", NombreEvento, null, Cuerpo);
            }
        }
    }
}
