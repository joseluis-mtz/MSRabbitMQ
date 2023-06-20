using MicroRabbit.Domain.Core.Bus;
using MicroRabbit.Infra.Bus;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MicroRabbit.Infra.IoC
{
    public class ContenedorDependencias
    {
        // Clase que permite vincular Interfaces con las respectivas clases de implementación
        // Setear configuracion del rabbit mq desde un proyecto web api
        public static void RegistrarServicios(IServiceCollection Servicios, IConfiguration Configuracion) 
        {
            // Registrar interfaz e implementacion
            // Dominio-Bus 
            // IEventBus viene desde el Domain Core Bus
            // RabbitMQBus viene desde el Infra Bus
            Servicios.AddTransient<IEventBus, RabbitMQBus>();
            // Se mapea la clase con una estructura json llamada igual que la clase
            Servicios.Configure<RabbitConfig>(c => Configuracion.GetSection("RabbitConfig"));

        }
    }
}
