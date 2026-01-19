using GAPPLE.Server.Data;
using GAPPLE.Shared.Model;
using GAPPLE.Shared.Requests;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Net;

namespace GAPPLE.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ComprobantesController : ControllerBase
    {
        private IConfiguration Configuration { get; }
        private Usuario Usuario { get; }

        public ComprobantesController(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        [HttpPost("notacredito/obtener")]
        public IActionResult GetNotasCredito(ComprobanteCabeceraRequest request)
        {
            try
            {
                return Ok(ObtenerNotasCredito(request));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.ToString());
            }
        }

        internal List<ComprobanteCabecera> ObtenerNotasCredito(ComprobanteCabeceraRequest request)
        {
            DA_Comprobantes daC = new(Configuration.GetConnectionString("DefaultConnection"));
            List<ComprobanteCabecera> lstComprobantes = new();
            using (DataTable dt = daC.ObtenerComprobantesCabecera(request.FechaDesde, request.FechaHasta, request.CodigoOrden, request.CodigoTango, request.MercaderiaIngresada, (int)request.IdEstado.Value, request.RazonSocialCliente))
            {
                foreach (DataRow row in dt.Rows)
                {
                    ComprobanteCabecera cc = new();
                    if (row["IdComprobante"] != DBNull.Value) cc.IdComprobante = int.Parse(row["IdComprobante"].ToString());
                    if (row["CodigoOrden"] != DBNull.Value) cc.CodigoOrden = row["CodigoOrden"].ToString();
                    if (row["CodigoTango"] != DBNull.Value) cc.CodigoTango = row["CodigoTango"].ToString();
                    if (row["Fecha"] != DBNull.Value) cc.FechaComprobante = DateTime.Parse(row["Fecha"].ToString());
                    if (row["DescripcionMotivo"] != DBNull.Value) cc.MotivoDescripcion = row["DescripcionMotivo"].ToString();
                    if (row["DescripcionEstado"] != DBNull.Value) cc.EstadoDescripcion = row["DescripcionEstado"].ToString();
                    if (row["ImporteTotal"] != DBNull.Value) cc.ImporteTotal = decimal.Parse(row["ImporteTotal"].ToString());
                    if (row["MercaderiaIngresada"] != DBNull.Value) cc.MercaderiaIngresada = bool.Parse(row["ImporteTotal"].ToString());
                    if (row["Observaciones"] != DBNull.Value) cc.Observaciones = row["Observaciones"].ToString();
                    if (row["RazonSocial"] != DBNull.Value) cc.ClienteRazonSocial = row["RazonSocial"].ToString();
                    if (row["CUIT"] != DBNull.Value) cc.ClienteCuit = row["CUIT"].ToString();
                    if (row["CategoriaIVA"] != DBNull.Value) cc.ClienteCategoriaIVA = row["CategoriaIVA"].ToString();

                    lstComprobantes.Add(cc);
                }
            }
            return lstComprobantes;
        }

        [HttpPost("notacredito")]
        public IActionResult PostNotaCredito(ComprobanteCabecera comprobante)
        {
            try
            {

                return Ok(comprobante);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.ToString());
            }
        }

        [HttpPut("notacredito")]
        public IActionResult PutNotaCredito(ComprobanteCabecera comprobante)
        {
            try
            {
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.ToString());
            }
        }

        [HttpPut("notacredito/{idComprobante:int}/cancelar")]
        public IActionResult CancelarNotaCredito(int idComprobante, [FromBody] string usuario)
        {
            DA_Comprobantes daC = new(Configuration.GetConnectionString("DefaultConnection"));
            daC.CancelarNotaCredito(idComprobante, usuario);
            return Ok();
        }
    }
}
