using MicroRabbit.Banca.Dominio.Modelos;
using Microsoft.EntityFrameworkCore;

namespace MicroRabbit.Banca.Data.Contexto
{
    public class BancaDbConext: DbContext
    {
        public BancaDbConext(DbContextOptions opciones): base(opciones) 
        { 
        }

        public DbSet<Cuentas> Cuentas { get; set; }
    }
}
