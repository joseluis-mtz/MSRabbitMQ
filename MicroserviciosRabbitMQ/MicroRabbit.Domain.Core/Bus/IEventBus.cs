using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroRabbit.Domain.Core.Bus
{
    public interface IEventBus
    {
        // Crear mensaje en Queue y Consumir
        void Publicar<T>(T evento) where T : Eventos;
        void Leer<T, TH>() where T : Eventos where TH : IManejadorEventos<T>;

        // Comunicacion interna entre clases
        Task EnviarComando<T>(T Comando) where T : Comandos;
    }
}
