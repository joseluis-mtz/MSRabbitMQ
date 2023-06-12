namespace MicroRabbit.Domain.Core.Bus
{
    public interface IManejadorEventos<in TEvento> : IManejadorEventos where TEvento : Eventos.Eventos
    {
        Task Manejador(TEvento evento);
    }

    public interface IManejadorEventos { }
}
