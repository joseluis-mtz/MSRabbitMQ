using MediatR;

namespace MicroRabbit.Domain.Core.Eventos
{
    public abstract class Mensajes: IRequest<bool>
    {
        public string TipoMensaje { get; protected set; }

        protected Mensajes()
        {
            TipoMensaje = GetType().Name; // Extraer el nombre de clase que se ejecuta "Mensajes"
        }
    }
}
