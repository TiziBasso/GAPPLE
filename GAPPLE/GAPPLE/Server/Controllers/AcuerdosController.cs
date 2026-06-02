using GAPPLE.Server.Data;
using GAPPLE.Server.Helpers;
using GAPPLE.Shared.Enums;
using GAPPLE.Shared.Model;
using GAPPLE.Shared.Requests;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Asn1.Ocsp;
using System.Data;

namespace GAPPLE.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AcuerdosController : ControllerBase
    {
        private IConfiguration Configuration { get; }
        private string connectionString { get; }

        public AcuerdosController(IConfiguration configuration)
        {
            Configuration = configuration;
            connectionString = Configuration.GetConnectionString("DefaultConnection");
        }

        [HttpPost("obtener")]
        public IActionResult GetAcuerdos(AcuerdosRequest request)
        {
            try
            {
                return Ok(ObtenerAcuerdos(request));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.ToString());
            }
        }

        internal List<AcuerdoCliente> ObtenerAcuerdos(AcuerdosRequest request)
        {
            List<AcuerdoCliente> acuerdosClientes = [];

            foreach (DataRow row in new DA_Acuerdos(connectionString).ObtenerAcuerdos(request).Rows)
            {
                var aux = acuerdosClientes.FirstOrDefault(x => x.IdCliente == (int)row["IdCliente"]);
                if (aux == null)
                {
                    aux = new()
                    {
                        IdCliente = (int)row["IdCliente"],
                        CodigoCliente = (string)row["CodigoCliente"],
                        RazonSocial = (string)row["RazonSocial"],
                    };
                    acuerdosClientes.Add(aux);
                }

                Acuerdo a = new()
                {
                    IdAcuerdo = (int)row["IdAcuerdo"],
                    IdCliente = aux.IdCliente,
                    Condicion = (string)row["Condicion"],
                    FechaDesde = (DateTime)row["FechaDesde"],
                    FechaHasta = (DateTime)row["FechaHasta"],
                    IdEstado = (AcuerdosEstadoEnum)int.Parse(row["IdEstado"].ToString()),
                    DescripcionEstado = row["DescripcionEstado"].ToString(),
                    MontosCargados = int.Parse(row["MontosCargados"].ToString()),
                    TotalCargado = decimal.Parse(row["TotalCargado"].ToString())
                };

                if (row["Linea"] != DBNull.Value) a.Linea = row["Linea"].ToString();
                aux.Acuerdos.Add(a);
            }

            return acuerdosClientes;
        }

        [HttpPost]
        public IActionResult PostAcuerdo(Acuerdo acuerdo)
        {
            try
            {
                acuerdo.IdAcuerdo = new DA_Acuerdos(connectionString).InsertarAcuerdo(acuerdo);
                return Ok(acuerdo);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.ToString());
            }
        }

        [HttpPut]
        public IActionResult PutAcuerdo(Acuerdo acuerdo)
        {
            try
            {
                new DA_Acuerdos(connectionString).EditarAcuerdo(acuerdo);
                return Ok(acuerdo);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.ToString());
            }
        }

        [HttpPut("estado/{idEstado:int}")]
        public IActionResult PutEstadoAcuerdo(AcuerdosEstadoEnum idEstado, [FromBody] Acuerdo acuerdo)
        {
            try
            {
                DA_Acuerdos daA = new(connectionString);
                ToolsController tC = new(Configuration);
                var acuerdos = ObtenerAcuerdos(new() { IdAcuerdo = acuerdo.IdAcuerdo });

                if (acuerdos == null || acuerdos.Count == 0 || acuerdos[0].Acuerdos.Count == 0)
                    ModelState.AddModelError("errores", "No se econtró el acuerdo, realice la búsqueda nuevamente.");
                else if (acuerdos[0].Acuerdos[0].IdEstado != acuerdo.IdEstado)
                    ModelState.AddModelError("errores", "El acuerdo cambio de estado, realice la búsqueda nuevamente.");
                else
                {
                    var estados = tC.ObtenerEstado<AcuerdosEstadoEnum>(new() { Seccion = "Acuerdos" });
                    acuerdo.DescripcionEstado = estados.First(x => x.Id == idEstado).Descripcion;
                    acuerdo.IdEstado = idEstado;
                    acuerdo.EdicionRegistro = DateTime.Now;
                    daA.EditarAcuerdo(new() { IdAcuerdo = acuerdo.IdAcuerdo, IdEstado = acuerdo.IdEstado, EdicionUsuario = acuerdo.EdicionUsuario });
                }

                if (ModelState.ErrorCount > 0)
                    return BadRequest(ModelState);

                return Ok(acuerdo);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.ToString());
            }
        }

        [HttpDelete("{idAcuerdo:int}")]
        public IActionResult DeleteAcuerdo(int idAcuerdo)
        {
            try
            {
                new DA_Acuerdos(connectionString).BorrarAcuerdo(idAcuerdo);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.ToString());
            }
        }

        #region AcuerdosMontos
        [HttpPost("montos/obtener")]
        public IActionResult GetAcuerdosMontos(AcuerdoMontosRequest request)
        {
            try
            {
                return Ok(ObtenerAcuerdoMontos(request));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.ToString());
            }
        }

        internal List<AcuerdoMonto> ObtenerAcuerdoMontos(AcuerdoMontosRequest request)
        {
            List<AcuerdoMonto> acuerdosMontos = [];

            foreach (DataRow row in new DA_Acuerdos(connectionString).ObtenerAcuerdoMontos(request, null).Rows)
                acuerdosMontos.AddMapped(row);

            return acuerdosMontos;
        }

        [HttpPost("montos")]
        public IActionResult PostAcuerdosMonto(AcuerdoMonto acuerdoMonto)
        {
            try
            {
                acuerdoMonto.Id = new DA_Acuerdos(connectionString).InsertarAcuerdoMonto(acuerdoMonto);
                return Ok(acuerdoMonto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.ToString());
            }
        }

        [HttpPut("montos")]
        public IActionResult PutAcuerdosMonto(AcuerdoMonto acuerdoMonto)
        {
            try
            {
                new DA_Acuerdos(connectionString).EditarAcuerdosMonto(acuerdoMonto);
                return Ok(acuerdoMonto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.ToString());
            }
        }

        [HttpDelete("montos/{id:int}")]
        public IActionResult DeleteAcuerdosMonto(int id)
        {
            try
            {
                new DA_Acuerdos(connectionString).EliminarAcuerdoMonto(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.ToString());
            }
        }
        #endregion
    }
}
