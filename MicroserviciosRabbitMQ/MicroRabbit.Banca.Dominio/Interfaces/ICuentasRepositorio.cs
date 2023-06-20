using MicroRabbit.Banca.Dominio.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroRabbit.Banca.Dominio.Interfaces
{
    public interface ICuentasRepositorio
    {
        IEnumerable<Cuentas> ObtenerCuentas();
    }
}
