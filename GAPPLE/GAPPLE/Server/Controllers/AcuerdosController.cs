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

        [HttpPut("obtener")]
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

        private List<Acuerdo> ObtenerAcuerdos(AcuerdosRequest request)
        {
            List<Acuerdo> acuerdos = [];

            foreach (DataRow row in new DA_Acuerdos(connectionString).ObtenerAcuerdos(request).Rows)
                acuerdos.AddMapped(row);

            return acuerdos;
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
    }
}
