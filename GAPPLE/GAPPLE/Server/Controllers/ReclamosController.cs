using GAPPLE.Server.Data;
using GAPPLE.Server.Tools;
using GAPPLE.Shared.Enums;
using GAPPLE.Shared.Model;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace GAPPLE.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReclamosController : ControllerBase
    {
        private string DefaultConnection { get; }

        public ReclamosController(IConfiguration configuration)
        {
            DefaultConnection = configuration.GetConnectionString("DefaultConnection");
        }

        // GET api/reclamos?fechaDesde=...&fechaHasta=...&razonSocialCliente=...&tipo=1&motivo=2
        [HttpGet]
        public IActionResult GetReclamos(
            DateTime fechaDesde,
            DateTime fechaHasta,
            string? razonSocialCliente = null,
            int? tipo = null,
            int? motivo = null)
        {
            try
            {
                DA_Reclamos da = new(DefaultConnection);
                List<Reclamo> lista = [];

                foreach (DataRow row in da.ObtenerReclamos(
                    fechaDesde, fechaHasta, razonSocialCliente, tipo, motivo).Rows)
                    lista.Add(DataRowHelper.MapDataRowTo<Reclamo>(row));

                return Ok(lista);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // GET api/reclamos/{idReclamo}/detalle
        [HttpGet("{idReclamo}/detalle")]
        public IActionResult GetReclamoDetalle(int idReclamo)
        {
            try
            {
                DA_Reclamos da = new(DefaultConnection);
                List<ReclamoDetalle> lista = [];

                foreach (DataRow row in da.ObtenerReclamoDetalle(idReclamo).Rows)
                    lista.Add(DataRowHelper.MapDataRowTo<ReclamoDetalle>(row));

                return Ok(lista);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // POST api/reclamos
        [HttpPost]
        public IActionResult PostReclamo(Reclamo reclamo)
        {
            try
            {
                DA_Reclamos da = new(DefaultConnection);

                reclamo.IdReclamo   = da.InsertarReclamo(reclamo);
                reclamo.AltaRegistro = DateTime.Now;

                foreach (var detalle in reclamo.Detalle)
                    da.InsertarReclamoDetalle(reclamo.IdReclamo, detalle);

                return Ok(reclamo);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // PUT api/reclamos
        [HttpPut]
        public IActionResult PutReclamo(Reclamo reclamo)
        {
            try
            {
                DA_Reclamos da = new(DefaultConnection);

                da.ActualizarReclamo(reclamo);
                reclamo.EdicionRegistro = DateTime.Now;

                // Borrar y reinsertar el detalle completo
                da.EliminarReclamoDetalle(reclamo.IdReclamo);
                foreach (var detalle in reclamo.Detalle)
                    da.InsertarReclamoDetalle(reclamo.IdReclamo, detalle);

                return Ok(reclamo);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // DELETE api/reclamos/{idReclamo}
        [HttpDelete("{idReclamo}")]
        public IActionResult DeleteReclamo(int idReclamo)
        {
            try
            {
                DA_Reclamos da = new(DefaultConnection);
                da.EliminarReclamoDetalle(idReclamo);
                da.EliminarReclamo(idReclamo);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
