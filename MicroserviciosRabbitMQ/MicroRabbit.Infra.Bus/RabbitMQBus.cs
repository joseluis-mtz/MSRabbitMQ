using MediatR;
using MicroRabbit.Domain.Core.Bus;
using MicroRabbit.Domain.Core.Comandos;
using MicroRabbit.Domain.Core.Eventos;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json.Serialization;

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
            throw new NotImplementedException();
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
