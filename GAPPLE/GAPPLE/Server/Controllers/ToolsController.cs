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
                return Ok(ObtenerEstado<int?>(request));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.ToString());
            }
        }

        /// <summary>
        /// T puede ser un Enum, si no queres crearlo mandale int?
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="request"></param>
        /// <returns></returns>
        public List<Estado<T>> ObtenerEstado<T>(EstadoRequest request)
        {
            List<Estado<T>> estados = [];

            if (request.ShowNull && Nullable.GetUnderlyingType(typeof(T)) != null)
                estados.Add(new Estado<T> { Id = default, Descripcion = "(Todos)" });

            foreach (DataRow row in new DA_Tools(ConnectionString).ObtenerEstados(request.Seccion).Rows)
                estados.Add(DataRowHelper.MapDataRowTo<Estado<T>>(row));

            return estados;
        }
    }
}
