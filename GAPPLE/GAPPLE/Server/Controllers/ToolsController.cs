using GAPPLE.Server.Data;
using GAPPLE.Server.Tools;
using GAPPLE.Shared.Entities;
using GAPPLE.Shared.Model;
using GAPPLE.Shared.Requests;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace GAPPLE.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ToolsController : ControllerBase
    {
        private string ConnectionString { get; set; }
        private IConfiguration Configuration { get; }
        private SesionDTO SesionDTO { get; }

        public ToolsController(IConfiguration configuration)
        {
            Configuration = configuration;
            ConnectionString = Configuration.GetConnectionString("DefaultConnection")!;
        }

        [HttpPost("estados/obtener")]
        public IActionResult GetEstados(EstadoRequest request)
        {
            try
            {
                List<Estado<int?>> estados = [];

                if (request.ShowNull)
                    estados.Add(new Estado<int?> { Id = null, Descripcion = "Todos" });

                foreach (DataRow row in new DA_Tools(ConnectionString).ObtenerEstados(request.Seccion).Rows)
                    estados.Add(DataRowHelper.MapDataRowTo<Estado<int?>>(row));

                return Ok(estados);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.ToString());
            }
        }
    }
}
