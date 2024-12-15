using GAPPLE.Server.Data;
using GAPPLE.Shared.Model;
using Integra.Web.Shared.Model;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace GAPPLE.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductosController : ControllerBase
    {
        private IConfiguration Configuration { get; }
        private Usuario Usuario { get; }

        public ProductosController(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        [HttpGet]
        public List<Producto> GetProductosDTO(string? codigoProducto, string? descripcion, bool? clasificado)
        {
            List<Producto> productos = new();
            DA_Producto daP = new(Configuration.GetConnectionString("DefaultConnection"));
            using (DataTable dt = daP.ObtenerProductos(codigoProducto, descripcion, clasificado))
            {
                foreach (DataRow row in dt.Rows)
                {
                    Producto producto = new()
                    {
                        CodigoProducto = row["CodigoProducto"].ToString()!,
                        Descripcion = (string)row["Descripcion"],
                        Pasivo = (bool)row["Pasivo"],
                    };
                    if (row["Clasificacion"] != DBNull.Value) producto.Clasificacion = row["Clasificacion"].ToString()!;
                    if (row["Observaciones"] != DBNull.Value) producto.Observaciones = row["Observaciones"].ToString()!;
                    if (row["Linea"] != DBNull.Value) producto.Linea = row["Linea"].ToString()!;

                    productos.Add(producto);
                }
            }
            return productos;
        }
    }
}
