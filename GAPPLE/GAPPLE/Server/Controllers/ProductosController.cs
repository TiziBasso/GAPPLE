using GAPPLE.Server.Data;
using GAPPLE.Shared.Model;
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
        public List<Producto> GetProductosDTO(string? codigoProducto, string? descripcion, bool? clasificado, bool? pasivo)
        {
            List<Producto> productos = new();
            DA_Producto daP = new(Configuration.GetConnectionString("DefaultConnection"));
            using (DataTable dt = daP.ObtenerProductos(codigoProducto, descripcion, clasificado, pasivo))
            {
                foreach (DataRow row in dt.Rows)
                {
                    Producto producto = new()
                    {
                        IdProducto = int.Parse(row["IdProducto"].ToString()),
                        CodigoProducto = row["CodigoProducto"].ToString()!,
                        Descripcion = (string)row["Descripcion"],
                        Pasivo = bool.Parse(row["Pasivo"].ToString()),
                        Orden = int.Parse(row["Orden"].ToString()),
                        Id_STA = int.Parse(row["ID_STA"].ToString())
                    };
                    if (row["Observaciones"] != DBNull.Value) producto.Observaciones = row["Observaciones"].ToString()!;
                    if (row["Linea"] != DBNull.Value) producto.Linea = row["Linea"].ToString()!;

                    productos.Add(producto);
                }
            }
            return productos;
        }

        [HttpPut]
        public IActionResult PutProducto([FromBody] Producto producto)
        {
            DA_Producto daP = new(Configuration.GetConnectionString("DefaultConnection"));
            daP.EditarProducto(producto.CodigoProducto, producto.Pasivo, producto.Orden);
            return Ok();
        }

        [HttpGet("lineas")]
        public List<string> GetLineas()
        {
            DA_Producto daP = new(Configuration.GetConnectionString("DefaultConnection"));
            List<string> lineas = new();
            using (DataTable dt = daP.GetLineas())
            {
                foreach (DataRow row in dt.Rows)
                {
                    lineas.Add(row["Linea"].ToString()!);
                }
            }
            return lineas;
        }

        [HttpGet("productosparaofertas")]
        public List<ProductoParaOfertas> GetProductosParaOfertas(string linea)
        {
            DA_Producto daP = new(Configuration.GetConnectionString("DefaultConnection"));
            List<ProductoParaOfertas> productos = new();
            using (DataTable dt = daP.GetProductosParaOfertas(linea))
            {
                foreach (DataRow row in dt.Rows)
                {
                    ProductoParaOfertas p = new()
                    {
                        CodigoProducto = row["CodigoProducto"].ToString()!,
                        Descripcion = row["Descripcion"].ToString()!,
                        Familia = row["Familia"].ToString()!
                    };
                    productos.Add(p);
                }
            }
            return productos;
        }
    }
}
