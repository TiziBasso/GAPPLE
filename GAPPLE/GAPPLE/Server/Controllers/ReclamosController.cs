using GAPPLE.Server.Data;
using GAPPLE.Server.Tools;
using GAPPLE.Shared.Enums;
using GAPPLE.Shared.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
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
            var error = ValidarReclamo(reclamo);
            if (error != null)
                return BadRequest(error);

            SqlTransaction transaction = null;
            try
            {
                using SqlConnection cnn = new(DefaultConnection);
                DA_Reclamos da = new(cnn.ConnectionString);
                cnn.Open();
                transaction = cnn.BeginTransaction();

                reclamo.IdReclamo    = da.InsertarReclamo(reclamo, transaction);
                reclamo.AltaRegistro = DateTime.Now;

                foreach (var detalle in reclamo.Detalle)
                    da.InsertarReclamoDetalle(reclamo.IdReclamo, detalle, transaction);

                transaction.Commit();
                return Ok(reclamo);
            }
            catch (Exception ex)
            {
                transaction?.Rollback();
                return StatusCode(500, ex.Message);
            }
        }

        // PUT api/reclamos
        [HttpPut]
        public IActionResult PutReclamo(Reclamo reclamo)
        {
            var error = ValidarReclamo(reclamo);
            if (error != null)
                return BadRequest(error);

            SqlTransaction transaction = null;
            try
            {
                using SqlConnection cnn = new(DefaultConnection);
                DA_Reclamos da = new(cnn.ConnectionString);
                cnn.Open();
                transaction = cnn.BeginTransaction();

                da.ActualizarReclamo(reclamo, transaction);
                reclamo.EdicionRegistro = DateTime.Now;

                // Borrar y reinsertar el detalle completo
                da.EliminarReclamoDetalle(reclamo.IdReclamo, transaction);
                foreach (var detalle in reclamo.Detalle)
                    da.InsertarReclamoDetalle(reclamo.IdReclamo, detalle, transaction);

                transaction.Commit();
                return Ok(reclamo);
            }
            catch (Exception ex)
            {
                transaction?.Rollback();
                return StatusCode(500, ex.Message);
            }
        }

        // DELETE api/reclamos/{idReclamo}
        [HttpDelete("{idReclamo}")]
        public IActionResult DeleteReclamo(int idReclamo)
        {
            SqlTransaction transaction = null;
            try
            {
                using SqlConnection cnn = new(DefaultConnection);
                DA_Reclamos da = new(cnn.ConnectionString);
                cnn.Open();
                transaction = cnn.BeginTransaction();

                da.EliminarReclamoDetalle(idReclamo, transaction);
                da.EliminarReclamo(idReclamo, transaction);

                transaction.Commit();
                return Ok();
            }
            catch (Exception ex)
            {
                transaction?.Rollback();
                return StatusCode(500, ex.Message);
            }
        }

        // ─── Validación server-side (no depende del cliente) ──────────────────────
        private static string ValidarReclamo(Reclamo reclamo)
        {
            if (reclamo.Detalle == null || reclamo.Detalle.Count == 0)
                return "Debe incluir al menos un producto en el detalle del reclamo";

            if (reclamo.Detalle.Any(d => string.IsNullOrWhiteSpace(d.SKU)))
                return "Todas las líneas de detalle deben tener un SKU";

            if (reclamo.Detalle.Any(d => d.Cantidad <= 0))
                return "Las cantidades de todas las líneas deben ser mayores a cero";

            var duplicados = reclamo.Detalle
                .GroupBy(d => d.SKU.Trim(), StringComparer.OrdinalIgnoreCase)
                .Where(g => g.Count() > 1)
                .Select(g => g.Key)
                .ToList();

            if (duplicados.Count > 0)
                return $"Hay productos duplicados en el detalle: {string.Join(", ", duplicados)}";

            return null;
        }
    }
}
