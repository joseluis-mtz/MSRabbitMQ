using MicroRabbit.Banca.Dominio.Modelos;

namespace MicroRabbit.Banca.Aplicacion.Interfaces
{
    public interface ICuentaServicio
    {
        IEnumerable<Cuentas> ObtenerCuentas();
    }
}
