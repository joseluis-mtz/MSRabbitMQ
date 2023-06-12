namespace MicroRabbit.Domain.Core.Eventos
{
    public abstract class Eventos
    {
        protected Eventos()
        {
            Timestamp = DateTime.Now;
        }

        public DateTime Timestamp { get; protected set; }
    }
}
