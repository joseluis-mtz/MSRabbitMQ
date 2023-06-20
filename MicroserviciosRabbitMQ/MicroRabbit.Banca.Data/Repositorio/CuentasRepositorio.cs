using MicroRabbit.Banca.Data.Contexto;
using MicroRabbit.Banca.Dominio.Interfaces;
using MicroRabbit.Banca.Dominio.Modelos;

namespace MicroRabbit.Banca.Data.Repositorio
{
    public class CuentasRepositorio : ICuentasRepositorio
    {
        private readonly BancaDbConext _contexto;

        public CuentasRepositorio(BancaDbConext contexto)
        {
            _contexto = contexto;
        }

        public IEnumerable<Cuentas> ObtenerCuentas()
        {
            return _contexto.Cuentas;
        }
    }
}
