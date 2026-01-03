using GAPPLE.Server.Data;
using GAPPLE.Shared.Model;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace GAPPLE.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepositosController : ControllerBase
    {

        private IConfiguration Configuration { get; }
        private SesionDTO SesionDTO { get; }

        public DepositosController(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        [HttpGet]
        public List<Deposito> GetDepositos(string codigoTango, string descripcion, bool? visible)
        {
            List<Deposito> depositos = [];
            DA_Depositos daD = new(Configuration.GetConnectionString("DefaultConnection"));

            foreach (DataRow row in daD.ObtenerDepositos(codigoTango, descripcion, visible).Rows)
            {
                Deposito d = new()
                {
                    IdDeposito = Convert.ToInt32(row["IdDeposito"]),
                    CodigoTango = Convert.ToString(row["CodigoTango"]),
                    Descripcion = Convert.ToString(row["Descripcion"]),
                    Visible = Convert.ToBoolean(row["Visible"])
                };

                depositos.Add(d);
            }

            return depositos;
        }

        [HttpPut("visibilidad")]
        public IActionResult PutDepositoVisibilidad(Deposito deposito)
        {
            try
            {
                DA_Depositos daD = new(Configuration.GetConnectionString("DefaultConnection"));
                daD.EditarDepositos(deposito.IdDeposito, !deposito.Visible);
                return Ok();
            }
            catch
            {
                return StatusCode(500);
            }
        }
    }
}
