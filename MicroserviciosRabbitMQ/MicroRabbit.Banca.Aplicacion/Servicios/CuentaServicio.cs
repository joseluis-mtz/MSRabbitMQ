using MicroRabbit.Banca.Aplicacion.Interfaces;
using MicroRabbit.Banca.Dominio.Interfaces;
using MicroRabbit.Banca.Dominio.Modelos;

namespace MicroRabbit.Banca.Aplicacion.Servicios
{
    public class CuentaServicio: ICuentaServicio
    {
        private readonly ICuentasRepositorio _cuentaRepositorio;

        public CuentaServicio(ICuentasRepositorio cuentaRepositorio)
        {
            _cuentaRepositorio = cuentaRepositorio;
        }

        public IEnumerable<Cuentas> ObtenerServicios()
        {
            return _cuentaRepositorio.ObtenerCuentas();
        }
    }
}
