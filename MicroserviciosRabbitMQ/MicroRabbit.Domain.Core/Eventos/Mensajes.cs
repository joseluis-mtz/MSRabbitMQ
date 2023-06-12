using MediatR;

namespace MicroRabbit.Domain.Core.Eventos
{
    public abstract class Mensajes: IRequest<bool> // Se hereda para usar MediatR
    {
        // Crear comunicacion entre 1 o mas componentes [patron mediator]
        // IRequest permite comunicarse con otras clases
        public string TipoMensaje { get; protected set; } // Mensaje que se enviara [Tipo]

        protected Mensajes()
        {
            TipoMensaje = GetType().Name; // Extraer el nombre de clase que se ejecuta por ejemplo "Mensajes"
        }
    }
}
