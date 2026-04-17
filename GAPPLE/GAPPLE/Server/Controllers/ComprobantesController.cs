using GAPPLE.Server.Data;
using GAPPLE.Shared.Enums;
using GAPPLE.Shared.Model;
using GAPPLE.Shared.Requests;
using Microsoft.AspNetCore.Mvc;
using NPOI.SS.Formula.Functions;
using Org.BouncyCastle.Asn1.Ocsp;
using System.Data;
using System.Data.SqlClient;
using System.IO.Compression;
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
            DA_Comprobantes daC = new(DefaultConnectionString);
            List<ComprobanteCabecera> lstComprobantes = new();
            using (DataTable dt = daC.ObtenerComprobantesCabecera(request.FechaDesde, request.FechaHasta, request.CodigoOrden, request.CodigoTango, request.MercaderiaIngresada, request.IdEstado, request.RazonSocialCliente, request.IdComprobante))
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
                    if (row["MercaderiaIngresada"] != DBNull.Value) cc.MercaderiaIngresada = bool.Parse(row["MercaderiaIngresada"].ToString());
                    if (row["Observaciones"] != DBNull.Value) cc.Observaciones = row["Observaciones"].ToString();
                    if (row["RazonSocial"] != DBNull.Value) cc.ClienteRazonSocial = row["RazonSocial"].ToString();
                    if (row["CUIT"] != DBNull.Value) cc.ClienteCuit = row["CUIT"].ToString();
                    if (row["CategoriaIVA"] != DBNull.Value) cc.ClienteCategoriaIVA = row["CategoriaIVA"].ToString();
                    if (row["ComprobanteReferencia"] != DBNull.Value) cc.ComprobanteReferencia = row["ComprobanteReferencia"].ToString();
                    if (row["Alternativo"] != DBNull.Value) cc.Presupuesto = bool.Parse(row["Alternativo"].ToString());
                    cc.Factura = !cc.Presupuesto;
                    if (row["IdListaPrecio"] != DBNull.Value) cc.IdListaPrecio = int.Parse(row["IdListaPrecio"].ToString());
                    if (row["IdMotivo"] != DBNull.Value) cc.IdMotivo = int.Parse(row["IdMotivo"].ToString());
                    if (row["IdEstado"] != DBNull.Value) cc.IdEstado = (Shared.Enums.ComprobanteCabeceraEstadoEnum)int.Parse(row["IdEstado"].ToString());
                    if (row["IdCliente"] != DBNull.Value) cc.IdCliente = int.Parse(row["IdCliente"].ToString());
                    if (row["CodigoCliente"] != DBNull.Value) cc.CodCliente = row["CodigoCliente"].ToString();
                    if (row["NumeroNC"] != DBNull.Value) cc.NumeroNC = row["NumeroNC"].ToString();
                    cc.IdDeposito = Convert.ToInt32(row["IdDeposito"]);
                    cc.DepositoDescripcion = Convert.ToString(row["DescripcionDeposito"]);

                    if (request.ConDetalle)
                    {
                        using (DataTable dtd = daC.ObtenerComprobantesDetalle(cc.IdComprobante))
                        {
                            foreach (DataRow rowd in dtd.Rows)
                            {
                                ComprobanteDetalle cd = new();
                                cd.IdProducto = Convert.ToInt32(rowd["IdProducto"]);
                                cd.IdComprobante = (int)request.IdComprobante;  
                                cd.NumeroLinea = int.Parse(rowd["Linea"].ToString());
                                cd.CodProducto = rowd["CodProducto"].ToString();
                                cd.DescripcionProducto = rowd["Descripcion"].ToString();
                                cd.Cantidad = int.Parse(rowd["Cantidad"].ToString());
                                cd.Precio = decimal.Parse(rowd["Precio"].ToString());
                                cd.Descuento = decimal.Parse(rowd["Descuento"].ToString());
                                cd.Detalle = rowd["Detalle"].ToString();
                                cc.Detalle.Add(cd);
                            }

                        }
                    }
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
                return Ok(comprobante);
            }
            catch (Exception ex)
            {
                if (transaction != null && transaction.Connection != null)
                    transaction.Rollback();
                return StatusCode(500, ex.ToString());
            }
        }

        [HttpPut("notacredito/{idComprobante:int}/cancelar")]
        public IActionResult CancelarNotaCredito(int idComprobante, [FromBody] string usuario)
        {
            try
            {
                DA_Comprobantes daC = new(DefaultConnectionString);
                daC.ActualizarNotaCreditoEstado(idComprobante, ComprobanteCabeceraEstadoEnum.Cancelado, usuario);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.ToString());
            }
        }

        [HttpPut("notacredito/{idComprobante:int}/aprobar/{numeroNC}")]
        public IActionResult AprobarNotaCredito(int idComprobante, string numeroNC, [FromBody] string usuario)
        {
            try
            {
                DA_Comprobantes daC = new(DefaultConnectionString);
                daC.ActualizarNotaCreditoEstado(idComprobante, ComprobanteCabeceraEstadoEnum.Aprobado, usuario, numeroNC);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.ToString());
            }
        }

        [HttpPut("notacredito/{idComprobante:int}/revertir")]
        public IActionResult RevertirNotaCredito(int idComprobante, [FromBody] string usuario)
        {
            try
            {
                DA_Comprobantes daC = new(DefaultConnectionString);
                daC.ActualizarNotaCreditoEstado(idComprobante, ComprobanteCabeceraEstadoEnum.Pendiente, usuario);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.ToString());
            }
        }

        [HttpGet("notacredito/{idComprobante:int}/archivos")]
        public async Task<IActionResult> GetArchivos(int idComprobante)
        {
            try
            {
                return Ok(await ObtenerArchivos(idComprobante, null, default));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.ToString());
            }
        }

        [HttpPost("notacredito/archivos/download")]
        public async Task<IActionResult> GetGiftCardPrint(List<NotaCreditoArchivo> archivos, CancellationToken cancellationToken = default)
        {
            try
            {
                FileController fC = new();
                using var ms = new MemoryStream();
                using var zip = new ZipArchive(ms, ZipArchiveMode.Create, true);

                foreach (var archivo in archivos)
                {
                    if (cancellationToken.IsCancellationRequested)
                        break;

                    var bytes = fC.GetFile(archivo.Path);

                    var entry = zip.CreateEntry(archivo.NombreArchivo);
                    using var entryStream = entry.Open();
                    entryStream.Write(bytes);

                    await Task.Yield();
                }

                zip.Dispose();
                ms.Position = 0;

                if (cancellationToken.IsCancellationRequested)
                    return StatusCode(500);

                return File(ms.ToArray(), "application/zip", $"archivos.zip");
            }
            catch (Exception ex)
            {
                //log.LogError(User.Identity.Name, ex.ToString());
                return StatusCode(500, ex.Message);
            }
        }

        internal async Task<List<NotaCreditoArchivo>> ObtenerArchivos(int idComprobante, int? idArchivo, CancellationToken cancellationToken)
        {
            DA_Comprobantes daC = new(DefaultConnectionString);
            List<NotaCreditoArchivo> archivos = new();
            using (DataTable dt = daC.ObtenerArchivos(idComprobante, idArchivo))
            {
                foreach (DataRow row in dt.Rows)
                {

                    NotaCreditoArchivo nc = new()
                    {
                        IdArchivo = int.Parse(row["IdArchivo"].ToString()),
                        IdComprobante = int.Parse(row["IdComprobante"].ToString()),
                        NombreArchivo = row["NombreArchivo"].ToString(),
                        Path = row["Ruta"].ToString(),
                        TipoArchivo = row["TipoMime"].ToString(),
                        FechaSubida = DateTime.Parse(row["FechaSubida"].ToString())
                    };
                    archivos.Add(nc);
                }
            }
            return archivos;
        }

        [HttpPost("notacredito/{idComprobante:int}/archivos")]
        public IActionResult PostArchivos(int idComprobante, List<NotaCreditoArchivo> archivos)
        {
            SqlTransaction transaction = null;
            try
            {
                using (SqlConnection cnn = new(DefaultConnectionString))
                {
                    DA_Comprobantes daC = new(cnn.ConnectionString);
                    cnn.Open();
                    transaction = cnn.BeginTransaction();
                    foreach (var item in archivos)
                    {
                        item.IdComprobante = idComprobante;
                        var dt = daC.InsertarArchivo(item, transaction);
                        item.IdArchivo = Convert.ToInt32(dt.Rows[0]["IdArchivo"]);
                    }
                    transaction.Commit();
                    cnn.Close();
                }
                return Ok(archivos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.ToString());
            }
        }

        [HttpDelete("notacredito/{idcomprobante:int}/archivo/{idarchivo:int}")]
        public async Task<IActionResult> DeleteArchivo(int idcomprobante, int idarchivo)
        {
            SqlTransaction transaction = null;
            try
            {
                FileController fC = new();
                using (SqlConnection cnn = new(DefaultConnectionString))
                {
                    var archivo = (await ObtenerArchivos(idcomprobante, idarchivo, default)).FirstOrDefault();
                    DA_Comprobantes daC = new(cnn.ConnectionString);
                    cnn.Open();
                    transaction = cnn.BeginTransaction();
                    daC.DeleteArchivo(archivo.IdArchivo, archivo.IdComprobante, transaction);
                    fC.DeleteFile(archivo.Path);
                    transaction.Commit();
                    cnn.Close();
                }
                return Ok();
            }
            catch (Exception ex)
            {
                if (transaction != null && transaction.Connection != null)
                    transaction.Rollback();

                return StatusCode(500, ex.ToString());
            }
        }
    }
}
