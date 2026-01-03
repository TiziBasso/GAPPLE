using GAPPLE.Server.Data;
using GAPPLE.Shared.Model;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace GAPPLE.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MotivosController : ControllerBase
    {
        private IConfiguration Configuration { get; }
        private SesionDTO SesionDTO { get; }

        public MotivosController(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        [HttpGet]

        public List<Motivos> GetMotivos(string descripcion, bool pasivo, int? IdDeposito, string descripcionDeposito)
        {
            List<Motivos> motivos = [];
            DA_Motivos daM = new(Configuration.GetConnectionString("DefaultConnection"));

            foreach (DataRow row in daM.ObtenerMotivos(descripcion, pasivo, IdDeposito, descripcionDeposito).Rows)
            {
                Motivos m = new()
                {
                    Descripcion = Convert.ToString(row["Descripcion"]),
                    Pasivo = Convert.ToBoolean(row["Pasivo"]),
                    IdDeposito = Convert.ToInt32(row["IdDeposito"]),
                    DescripcionDeposito = Convert.ToString(row["DescripcionDeposito"])
                };

                motivos.Add(m);
            }

            return motivos;
        }

        [HttpPut]
        public IActionResult PutMotivos(Motivos motivos)
        {
            try
            {
                DA_Motivos daM = new(Configuration.GetConnectionString("DefaultConnection"));
                daM.EditarMotivos(motivos.Descripcion, !motivos.Pasivo);
                return Ok();
            }
            catch
            {
                return StatusCode(500);
            }
        }
    }
}
