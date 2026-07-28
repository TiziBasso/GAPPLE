using GAPPLE.Server.Data;
using GAPPLE.Server.Tools;
using GAPPLE.Shared.Model;
using Integra.Web.Server.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Threading.Tasks;

namespace GAPPLE.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductosController : ControllerBase
    {
        private IConfiguration Configuration { get; }
        private Usuario Usuario { get; }
        private readonly IHubContext<SignalRController> HubContext;


        public ProductosController(IConfiguration configuration, IHubContext<SignalRController> hub = null)
        {
            Configuration = configuration;
            HubContext = hub;
        }

        [HttpGet]
        public List<Producto> GetProductosDTO(string? codigoProducto, string? descripcion, bool? clasificado, bool? pasivo, string? linea)
        {
            List<Producto> productos = new();
            DA_Producto daP = new(Configuration.GetConnectionString("DefaultConnection"));
            using (DataTable dt = daP.ObtenerProductos(codigoProducto, descripcion, clasificado, pasivo, linea))
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
                    };
                    if (row["ID_STA"] != DBNull.Value) producto.Id_STA = int.Parse(row["ID_STA"].ToString()!);
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
        public List<ProductoParaOfertas> GetProductosParaOfertas(string linea, string codListaPrecio)
        {
            DA_Producto daP = new(Configuration.GetConnectionString("DefaultConnection"));
            List<ProductoParaOfertas> productos = new();
            using (DataTable dt = daP.GetProductosParaOfertas(linea, codListaPrecio))
            {
                foreach (DataRow row in dt.Rows)
                {
                    ProductoParaOfertas p = new()
                    {
                        CodigoProducto = row["CodigoProducto"].ToString()!,
                        Descripcion = row["Descripcion"].ToString()!,
                        Familia = row["Familia"].ToString()!,
                        Sinonimo = row["Sinonimo"].ToString()!
                    };
                    if (row["CodigoComplemento"] != DBNull.Value) p.CodigoComplemento = row["CodigoComplemento"].ToString();
                    if (row["Precio"] != DBNull.Value) p.Precio = decimal.Parse(row["Precio"].ToString());
                    productos.Add(p);
                }
            }
            return productos;
        }

        [HttpGet("complementos")]
        public List<ProductosComplementos> GetProductosComplementos()
        {
            List<ProductosComplementos> pc = new();
            DA_Producto daP = new(Configuration.GetConnectionString("DefaultConnection"));
            using (DataTable dt = daP.GetProductosComplementos())
            {
                foreach (DataRow row in dt.Rows)
                {
                    ProductosComplementos p = new()
                    {
                        CodigoPrincipal = row["CodigoPrincipal"].ToString(),
                        DescripcionPrincipal = row["DescripcionPrincipal"].ToString(),
                        LineaPrincipal = row["LineaPrincipal"].ToString(),
                        CodigoRelacionado = row["CodigoRelacionado"].ToString(),
                        DescripcionRelacionado = row["DescripcionRelacionado"].ToString(),
                        LineaRelacionado = row["LineaRelacionado"].ToString()
                    };
                    pc.Add(p);
                }
            }
            return pc;
        }

        [HttpGet("precios")]
        public List<ProductosComplementos> GetPrecios(string codlista, string linea)
        {
            List<ProductosComplementos> pc = new();
            DA_Producto daP = new(Configuration.GetConnectionString("DefaultConnection"));
            using (DataTable dt = daP.GetProductosComplementos())
            {
                foreach (DataRow row in dt.Rows)
                {
                    ProductosComplementos p = new()
                    {
                        CodigoPrincipal = row["CodigoPrincipal"].ToString(),
                        DescripcionPrincipal = row["DescripcionPrincipal"].ToString(),
                        LineaPrincipal = row["LineaPrincipal"].ToString(),
                        CodigoRelacionado = row["CodigoRelacionado"].ToString(),
                        DescripcionRelacionado = row["DescripcionRelacionado"].ToString(),
                        LineaRelacionado = row["LineaRelacionado"].ToString()
                    };
                    pc.Add(p);
                }
            }
            return pc;
        }

        [HttpPost("complementos/insert")]
        public IActionResult InsertProductosComplementos(List<ProductosComplementos> productosComplementos)
        {
            SqlTransaction? trans = null;
            try
            {
                using (SqlConnection cnn = new(Configuration.GetConnectionString("DefaultConnection")))
                {
                    DA_Producto daP = new(cnn.ConnectionString);
                    cnn.Open();
                    trans = cnn.BeginTransaction();
                    foreach (var p in productosComplementos)
                        daP.InsertProductosComplementos(p.CodigoPrincipal, p.CodigoRelacionado, trans);

                    trans.Commit();
                    cnn.Close();
                }
                return Ok();
            }
            catch (Exception ex)
            {
                if (trans != null && trans.Connection != null)
                    trans.Rollback();

                return StatusCode(500, ex.ToString());
            }
        }

        [HttpPost("complementos/delete")]
        public IActionResult DeleteProductosComplementos(List<ProductosComplementos> productosComplementos)
        {
            SqlTransaction? trans = null;
            try
            {
                using (SqlConnection cnn = new(Configuration.GetConnectionString("DefaultConnection")))
                {
                    DA_Producto daP = new(cnn.ConnectionString);
                    cnn.Open();
                    trans = cnn.BeginTransaction();
                    foreach (var p in productosComplementos)
                        daP.DeleteProductosComplementos(p.CodigoPrincipal, p.CodigoRelacionado, trans);

                    trans.Commit();
                    cnn.Close();
                }
                return Ok();
            }
            catch (Exception ex)
            {
                if (trans != null && trans.Connection != null)
                    trans.Rollback();

                return StatusCode(500, ex.ToString());
            }
        }

        [HttpPost("procesar")]
        public async Task<IActionResult> ProcesarArchivo(ByteArrayRequest req)
        {
            try
            {
                List<ProductoArchivo> productos = new();
                SignalRController srC = new();
                ClientesController cC = new(Configuration);
                DataTable dt = ManejoDeArchivos.ExcelToDataTable(req.File, false, true);
                int i = 0;
                List<ArticulosPorCliente> prodCliente = cC.GetArticulosPorCliente(req.CodCliente);

                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        ProductoArchivo p = null;
                        bool carga = false;
                        if (!string.IsNullOrEmpty(row["0"].ToString().Trim()))
                        {
                            var cod = row["0"].ToString().Trim();
                            p = ObtenerProductoArchivo($"%{cod}", req.CodListaPrecio);
                            if (p == null)
                                ModelState.AddModelError($"En la fila {row["originalRow"]}", $"El código {cod} no sé encontró");
                            else if (p.Pasivo)
                                ModelState.AddModelError($"En la fila {row["originalRow"]}", $"El producto {cod} se encuentra pasivo");
                            else
                            {
                                if (int.TryParse(row["1"].ToString(), out int cant))
                                    p.CantidadSeleccionada = cant;

                                //mayor a 3 porque existe la columna "originalRow"
                                if (dt.Columns.Count > 3 && int.TryParse(row["2"].ToString(), out int prob))
                                    p.CantidadProbador = prob;

                                //mayor a 4 porque existe la columna "originalRow"
                                if (dt.Columns.Count > 4 && int.TryParse(row["3"].ToString(), out int obs))
                                    p.CantidadObsequio = obs;

                                if (p.CantidadProbador < 0 || p.CantidadSeleccionada < 0 || p.CantidadObsequio < 0)
                                    ModelState.AddModelError($"En la fila {row["originalRow"]}", "Hay cantidades negativas cargadas");
                                else if (p.CantidadProbador == 0 && p.CantidadSeleccionada == 0 && p.CantidadObsequio == 0)
                                    ModelState.AddModelError($"En la fila {row["originalRow"]}", "No hay cantidades cargadas");
                                else
                                    carga = true;
                            }
                        }
                        else
                            ModelState.AddModelError($"En la fila {row["originalRow"]}", "La columna codigo producto está vacía");

                        if (carga)
                        {
                            if (prodCliente.Any(x => x.CodProducto == p.CodigoProducto))
                                p.DescuentoCliente = prodCliente.First(x => x.CodProducto == p.CodigoProducto).Descuento;

                            p.DescuentoTotal = p.DescuentoCliente;
                            p.PrecioConDescuento = p.Precio * (1 - p.DescuentoTotal / 100);
                            p.PrecioTotal = p.PrecioConDescuento * p.CantidadSeleccionada;

                            productos.Add(p);

                            if (p.CodComplemento != null)
                            {
                                var aux = ObtenerProductoArchivo(p.CodComplemento, req.CodListaPrecio);
                                if (aux.Pasivo)
                                    ModelState.AddModelError($"En la fila {row["originalRow"]}", $"El producto complemento {p.CodComplemento} se encuentra pasivo");
                                else
                                {
                                    if (productos.Exists(x => x.CodigoProducto == aux.CodigoProducto))
                                    {
                                        var existing = productos.First(x => x.CodigoProducto == aux.CodigoProducto);
                                        existing.CantidadSeleccionada += p.CantidadSeleccionada;
                                        existing.CantidadProbador += p.CantidadProbador;
                                        existing.PrecioTotal = existing.PrecioConDescuento * existing.CantidadSeleccionada;
                                    }
                                    else
                                    {
                                        aux.CantidadSeleccionada = p.CantidadSeleccionada;
                                        aux.CantidadProbador = p.CantidadProbador;
                                        aux.CantidadObsequio = p.CantidadObsequio;
                                        aux.PrecioTotal = aux.PrecioConDescuento * aux.CantidadSeleccionada;
                                        productos.Add(aux);
                                    }
                                }
                            }
                        }

                        i++;
                        await srC.CambiarPorcentajeTarea(HubContext.Clients, req.ConnectionId, (i * 100 / dt.Rows.Count));
                    }
                }
                else
                {
                    ModelState.AddModelError("El archivo", "Se encuentra vacío o no se pudo leer");
                }

                if (ModelState.ErrorCount > 0)
                    return BadRequest(ModelState);
                else
                    return Ok(productos);
            }
            catch (Exception ex)
            {
                //log
                return StatusCode(500, ex.Message);
            }
        }

        internal ProductoArchivo ObtenerProductoArchivo(string codProducto, string codListaPrecio)
        {
            ProductoArchivo p = null;
            DA_Producto daP = new DA_Producto(Configuration.GetConnectionString("DefaultConnection"));
            using (DataTable dt = daP.ObtenerProductos(codProducto, null, null, null, null, null))
            {
                if (dt.Rows.Count > 0)
                {
                    DataRow row = dt.Rows[0];
                    p = new()
                    {
                        IdProducto = int.Parse(row["IdProducto"].ToString()),
                        CodigoProducto = row["CodigoProducto"].ToString()!,
                        Descripcion = row["Descripcion"].ToString()!,
                        Linea = row["Linea"].ToString(),
                        Pasivo = bool.Parse(row["Pasivo"].ToString())
                    };
                    if (row["CodigoRelacionado"] != DBNull.Value)
                        p.CodComplemento = row["CodigoRelacionado"].ToString();

                    using (DataTable dtP = daP.ObtenerPrecio(codListaPrecio, null, p.CodigoProducto))
                    {
                        if (dtP.Rows.Count > 0)
                        {
                            p.Precio = decimal.Parse(dtP.Rows[0]["Precio"].ToString());
                            p.PrecioConDescuento = p.Precio;
                        }
                    }
                }
            }
            return p;
        }

        internal ProductoOrden ObtenerProductoOrden(string codProducto, string codListaPrecio)
        {
            ProductoOrden p = null;
            DA_Producto daP = new DA_Producto(Configuration.GetConnectionString("DefaultConnection"));
            using (DataTable dt = daP.ObtenerProductos(codProducto, null, null, null, null, null))
            {
                if (dt.Rows.Count > 0)
                {
                    DataRow row = dt.Rows[0];
                    p = new()
                    {
                        IdProducto = int.Parse(row["IdProducto"].ToString()),
                        CodigoProducto = row["CodigoProducto"].ToString()!,
                        Descripcion = row["Descripcion"].ToString()!,
                        Linea = row["Linea"].ToString(),
                        Pasivo = bool.Parse(row["Pasivo"].ToString())
                    };
                    if (row["CodigoRelacionado"] != DBNull.Value) p.CodComplemento = row["CodigoRelacionado"].ToString();

                    using (DataTable dtP = daP.ObtenerPrecio(codListaPrecio, null, p.CodigoProducto))
                    {
                        if (dtP.Rows.Count > 0)
                            p.Precio = decimal.Parse(dtP.Rows[0]["Precio"].ToString());
                    }
                }
            }

            return p;
        }

        internal List<ProductoOrden> ObtenerProductosOrden(string codProducto, string descripcion, string linea, string codListaPrecio)
        {
            List<ProductoOrden> productos = new();
            DA_Producto daP = new DA_Producto(Configuration.GetConnectionString("DefaultConnection"));
            using (DataTable dt = daP.ObtenerProductos(codProducto, descripcion, null, null, linea, null))
            {

                foreach (DataRow row in dt.Rows)
                {
                    ProductoOrden producto = new()
                    {
                        IdProducto = int.Parse(row["IdProducto"].ToString()),
                        CodigoProducto = row["CodigoProducto"].ToString()!,
                        Descripcion = (string)row["Descripcion"],
                        Pasivo = bool.Parse(row["Pasivo"].ToString()),
                    };
                    if (row["Linea"] != DBNull.Value) producto.Linea = row["Linea"].ToString()!;

                    if (!string.IsNullOrEmpty(codListaPrecio))
                    {
                        using (DataTable dtP = daP.ObtenerPrecio(codListaPrecio, null, producto.CodigoProducto))
                        {
                            if (dtP.Rows.Count > 0)
                                producto.Precio = decimal.Parse(dtP.Rows[0]["Precio"].ToString());
                        }
                    }

                    productos.Add(producto);
                }
            }

            return productos;
        }


        [HttpGet("orden")]
        public IActionResult GetProductoOrden(string codProducto, string codListaPrecio)
        {
            try
            {
                return Ok(ObtenerProductoOrden(codProducto, codListaPrecio));
            }
            catch (Exception ex)
            {
                //log
                return StatusCode(500, ex.ToString());
            }
        }

        [HttpGet("orden/varios")]
        public IActionResult GetProductosOrden(string codProducto, string descripcion, string linea, string codListaPrecio)
        {
            try
            {
                return Ok(ObtenerProductosOrden(codProducto, descripcion, linea, codListaPrecio));
            }
            catch (Exception ex)
            {
                //log
                return StatusCode(500, ex.ToString());
            }
        }


    }
}
