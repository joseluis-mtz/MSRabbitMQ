using MicroRabbit.Banca.Aplicacion.Interfaces;
using MicroRabbit.Banca.Aplicacion.Servicios;
using MicroRabbit.Banca.Data.Contexto;
using MicroRabbit.Banca.Data.Repositorio;
using MicroRabbit.Banca.Dominio.Interfaces;
using MicroRabbit.Banca.Dominio.Modelos;
using MicroRabbit.Domain.Core.Bus;
using MicroRabbit.Infra.Bus;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

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

            // Desde el proyecto aplicaciones servicios
            Servicios.AddTransient<ICuentaServicio, CuentaServicio>();

            // Desde Data
            Servicios.AddTransient<ICuentasRepositorio, CuentasRepositorio>();

            // Contexto
            Servicios.AddTransient<BancaDbConext>();

            
        }
    }
}
