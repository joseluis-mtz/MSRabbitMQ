using MicroRabbit.Domain.Core.Eventos;

namespace MicroRabbit.Domain.Core.Comandos
{
    public abstract class Comandos: Mensajes
    {
        protected Comandos()
        {
            Timestamp = DateTime.Now;
        }

        public DateTime Timestamp { get; protected set; }
    }
}
