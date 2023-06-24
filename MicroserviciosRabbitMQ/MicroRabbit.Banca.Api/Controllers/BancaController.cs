using MicroRabbit.Banca.Aplicacion.Interfaces;
using MicroRabbit.Banca.Dominio.Modelos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MicroRabbit.Banca.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BancaController : ControllerBase
    {
        private readonly ICuentaServicio _cuentaServicio;

        public BancaController(ICuentaServicio cuentaServicio)
        {
            _cuentaServicio = cuentaServicio;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Cuentas>> Obtener()
        {
            return Ok(_cuentaServicio.ObtenerCuentas());
        }
    }
}
