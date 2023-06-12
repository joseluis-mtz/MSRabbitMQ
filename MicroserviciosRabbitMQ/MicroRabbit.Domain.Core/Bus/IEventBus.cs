using MicroRabbit.Domain.Core.Comandos;
using MicroRabbit.Domain.Core.Eventos;

namespace MicroRabbit.Domain.Core.Bus
{
    public interface IEventBus
    {
        // Crear mensaje en Queue y Consumir
        void Publicar<T>(T evento) where T : Eventos.Eventos;
        void Leer<T, TH>() where T : Eventos.Eventos where TH : IManejadorEventos<T>;

        // Comunicacion interna entre clases
        Task EnviarComando<T>(T Comando) where T : Comandos.Comandos;
    }
}
