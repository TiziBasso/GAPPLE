using GAPPLE.Server.Data;
using GAPPLE.Shared.Model;
using GAPPLE.Shared.Requests;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;
using System.Net;

namespace GAPPLE.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ComprobantesController : ControllerBase
    {
        private IConfiguration Configuration { get; }
        private Usuario Usuario { get; }
        private string DefaultConnectionString { get; }

        public ComprobantesController(IConfiguration configuration)
        {
            Configuration = configuration;
            DefaultConnectionString = Configuration.GetConnectionString("DefaultConnection");
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
            using (DataTable dt = daC.ObtenerComprobantesCabecera(request.FechaDesde, request.FechaHasta, request.CodigoOrden, request.CodigoTango, request.MercaderiaIngresada, request.IdEstado, request.RazonSocialCliente))
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
            SqlTransaction transaction = null;
            try
            {
                using (SqlConnection cnn = new(DefaultConnectionString))
                {
                    DA_Comprobantes daC = new(cnn.ConnectionString);
                    cnn.Open();
                    transaction = cnn.BeginTransaction();
                    comprobante.IdComprobante = Convert.ToInt32(daC.InsertarNotaCreditoCabecera(comprobante, transaction).Rows[0]["IdComprobante"]);
                    cnn.Open();
					transaction = cnn.BeginTransaction();
					foreach (ComprobanteDetalle detalle in comprobante.Detalle)
                    {
						detalle.IdComprobante = comprobante.IdComprobante;
                        daC.InsertarNotaCreditoDetalle(detalle, transaction);
                    }
                    transaction.Commit();
                    cnn.Close();
                }

                return Ok(comprobante);
            }
            catch (Exception ex)
            {
                if (transaction != null && transaction.Connection != null)
                    transaction.Rollback();
                return StatusCode(500, ex.ToString());
            }
        }

        [HttpPut("notacredito")]
        public IActionResult PutNotaCredito(ComprobanteCabecera comprobante)
        {
            SqlTransaction transaction = null;
            try
            {
                using (SqlConnection cnn = new(DefaultConnectionString))
                {
                    DA_Comprobantes daC = new(cnn.ConnectionString);
                    cnn.Open();
                    transaction = cnn.BeginTransaction();
                    daC.ActualizarNotaCreditoCabecera(comprobante, transaction);
                    daC.EliminarNotaCreditoDetalle(comprobante.IdComprobante, transaction);
                    foreach (ComprobanteDetalle detalle in comprobante.Detalle)
                    {
                        detalle.IdComprobante = comprobante.IdComprobante;
                        daC.InsertarNotaCreditoDetalle(detalle, transaction);
                    }
                    transaction.Commit();
                    cnn.Close();
                }
                return Ok();
            }
            catch (Exception ex)
            {
                if(transaction != null && transaction.Connection != null)
                    transaction.Rollback();
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
