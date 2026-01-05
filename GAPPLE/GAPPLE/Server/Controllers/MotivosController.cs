using GAPPLE.Server.Data;
using GAPPLE.Server.Tools;
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
        private string DefaultConnection { get; }

        public MotivosController(IConfiguration configuration)
        {
            Configuration = configuration;
            DefaultConnection = Configuration.GetConnectionString("DefaultConnection");
        }

        [HttpGet]

        public IActionResult GetMotivos(int? idMotivo, string descripcion, bool? pasivo, int? IdDeposito)
        {
            try
            {
                return Ok(ObtenerMotivos(idMotivo, descripcion, pasivo, IdDeposito));
            }
            catch (Exception ex)
            {
                //log
                return StatusCode(500);
            }
        }

        internal List<Motivo> ObtenerMotivos(int? idMotivo, string descripcion, bool? pasivo, int? IdDeposito)
        {
            List<Motivo> motivos = [];
            DA_Motivos daM = new(DefaultConnection);

            foreach (DataRow row in daM.ObtenerMotivos(idMotivo, descripcion, pasivo, IdDeposito).Rows)
                motivos.Add(DataRowHelper.MapDataRowTo<Motivo>(row));

            return motivos;
        }

        [HttpPut]
        public IActionResult PutMotivo(Motivo motivo)
        {
            try
            {
                DA_Motivos daM = new(DefaultConnection);

                daM.EditarMotivos(motivo);
                motivo.EdicionRegistro = DateTime.Now;

                return Ok(motivo);
            }
            catch
            {
                return StatusCode(500);
            }
        }

        [HttpPost]
        public IActionResult PostMotivo(Motivo motivo)
        {
            try
            {
                DA_Motivos daM = new(DefaultConnection);

                motivo.IdMotivo = daM.InsertarMotivo(motivo);
                motivo.AltaRegistro = DateTime.Now;

                return Ok(motivo);
            }
            catch (Exception ex)
            {
                //log
                return StatusCode(500);
            }
        }
    }
}
