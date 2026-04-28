using GAPPLE.Server.Data;
using GAPPLE.Server.Helpers;
using GAPPLE.Shared.Model;
using GAPPLE.Shared.Requests;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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

        private List<AcuerdoCliente> ObtenerAcuerdos(AcuerdosRequest request)
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

                aux.Acuerdos.Add(new()
                {
                    IdAcuerdo = (int)row["IdAcuerdo"],
                    IdCliente = aux.IdCliente,
                    //TODO: decidir cual
                    //IdEstado = (enum)row["IdEstado"]
                    //Aprobado = (bool)row["IdEstado"]
                    Linea = (string)row["Linea"],
                    Condicion = (string)row["Condicion"],
                    FechaDesde = (DateTime)row["FechaDesde"],
                    FechaHasta = (DateTime)row["FechaHasta"],
                    Activo = (bool)row["Activo"]
                });
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
