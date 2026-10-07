using System.Data;
using GAPPLE.Server.Data;
using GAPPLE.Server.Helpers;
using GAPPLE.Shared.Helpers;
using GAPPLE.Shared.Model;
using Microsoft.AspNetCore.Mvc;

namespace GAPPLE.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientesController : ControllerBase
    {
        private IConfiguration Configuration { get; }
        private SesionDTO SesionDTO { get; }

        public ClientesController(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        [HttpGet]
        public List<Cliente> GetClientes(int? idCliente = null, string codCliente = null, string razonSocial = null, string cuit = null, bool? clienteEspecial = null, int? idUsuario = null)
        {
            DA_Clientes daC = new(Configuration.GetConnectionString("DefaultConnection"));
            List<Cliente> lst = new();
            foreach (DataRow row in daC.ObtenerClientes(idCliente, codCliente, razonSocial?.Trim(), cuit, clienteEspecial, idUsuario, null).Rows)
            {
                Cliente c = new()
                {
                    IdCliente = int.Parse(row["IdCliente"].ToString()!),
                    CodigoCliente = row["CodigoCliente"].ToString()!,
                    RazonSocial = row["RazonSocial"].ToString()!,
                    Observaciones = row["Observaciones"].ToString()!,
                    ClienteEspecial = bool.Parse(row["ClienteEspecial"].ToString()!),
                    CUIT = row["CUIT"].ToString()!,
                    CodListaPrecioDefault = row["IdListaDePrecio"].ToString()!,
                    CondVentaDefault = row["CondVenta"].ToString()!,
                    Domicilio = row["Domicilio"].ToString()!,
                    CodigoPostal = row["CPostal"].ToString(),
                    Localidad = row["Localidad"].ToString(),
                    ZonaDefault = row["Zona"].ToString()!
                };
                if (row["ID_GVA"] != DBNull.Value) c.Id_GVA = int.Parse(row["ID_GVA"].ToString());
                if (row["CategoriaIVA"] != DBNull.Value) c.CategoriaIva = row["CategoriaIVA"].ToString();
                lst.Add(c);
            }
            return lst;
        }

        [HttpPost]
        public IActionResult PostClienteEspecial(Cliente cliente)
        {
            DA_Clientes daC = new(Configuration.GetConnectionString("DefaultConnection"));
            daC.PersistirEdicionCliente((int)cliente.IdCliente!, true, cliente.Observaciones!);
            return Ok();
        }

        [HttpGet("articulos")]
        public List<ArticulosPorCliente> GetArticulosPorCliente(string codCliente)
        {
            DA_Clientes daC = new(Configuration.GetConnectionString("DefaultConnection"));
            List<ArticulosPorCliente> lst = new();
            foreach (DataRow row in daC.GetArticulosPorCliente(codCliente).Rows)
            {
                ArticulosPorCliente c = new()
                {
                    CodProducto = row["CodProducto"].ToString()!,
                    Descuento = decimal.Parse(row["Bonificacion"].ToString()!)
                };
                lst.Add(c);
            }
            return lst;
        }

        /// <summary>
        /// Descarga en Excel la lista de precios vigente del cliente, con los mismos datos que se ven en la carga de pedidos
        /// (linea, familia, codigo, sinonimo, descripcion, precio y bonificacion del cliente por articulo).
        /// Se valida en el server que el usuario tenga el permiso y que el cliente este en su cartera
        /// (prc_get_Clientes ya filtra por los vendedores del usuario).
        /// </summary>
        [HttpGet("{idCliente:int}/listaprecios")]
        public IActionResult GetListaPrecios(int idCliente, int idUsuario)
        {
            if (!TienePermiso(idUsuario, Permisos.Menu.Clientes, Permisos.DescargarListaPrecios))
                return StatusCode(StatusCodes.Status403Forbidden);

            DA_Clientes daC = new(Configuration.GetConnectionString("DefaultConnection"));
            DataTable dtCliente = daC.ObtenerClientes(idCliente, null, null, null, null, idUsuario);
            if (dtCliente.Rows.Count == 0)
                return StatusCode(StatusCodes.Status403Forbidden);

            var cliente = dtCliente.Rows[0];
            string codCliente = cliente["CodigoCliente"].ToString()!;
            string codLista = cliente["IdListaDePrecio"].ToString()!;
            if (string.IsNullOrWhiteSpace(codLista))
                return BadRequest("El cliente no tiene una lista de precios asignada");

            var bonificaciones = new Dictionary<string, decimal>();
            foreach (DataRow row in daC.GetArticulosPorCliente(codCliente).Rows)
                bonificaciones[row["CodProducto"].ToString()!] = decimal.Parse(row["Bonificacion"].ToString()!);

            DataTable dt = new();
            dt.Columns.Add("Línea", typeof(string));
            dt.Columns.Add("Familia", typeof(string));
            dt.Columns.Add("Código", typeof(string));
            dt.Columns.Add("Sinónimo", typeof(string));
            dt.Columns.Add("Descripción", typeof(string));
            dt.Columns.Add("Precio lista", typeof(decimal));
            dt.Columns.Add("% Bonif. cliente", typeof(decimal));
            dt.Columns.Add("Precio c/bonif.", typeof(decimal));

            DA_Producto daP = new(Configuration.GetConnectionString("DefaultConnection"));
            foreach (DataRow rowLinea in daP.GetLineas().Rows)
            {
                string linea = rowLinea["Linea"].ToString()!;
                var procesados = new HashSet<string>();
                foreach (DataRow row in daP.GetProductosParaOfertas(linea, codLista).Rows)
                {
                    string codProducto = row["CodigoProducto"].ToString()!;
                    // El SP repite el producto por cada complemento y trae productos sin precio en la lista
                    if (row["Precio"] == DBNull.Value || !procesados.Add(codProducto))
                        continue;

                    decimal precio = decimal.Parse(row["Precio"].ToString()!);
                    decimal bonif = bonificaciones.GetValueOrDefault(codProducto);
                    string descripcion = row["Descripcion"].ToString()!;
                    if (descripcion.EndsWith($"({codProducto})"))
                        descripcion = descripcion[..^(codProducto.Length + 2)];

                    dt.Rows.Add(linea, row["Familia"].ToString(), codProducto, row["Sinonimo"].ToString(), descripcion,
                                precio, bonif, Math.Round(precio * (1 - bonif / 100), 2));
                }
            }

            var file = new Export().ToExcel(dt);
            file.FileDownloadName = $"ListaPrecios_{codCliente}_{codLista}.xlsx";
            return file;
        }

        private bool TienePermiso(int idUsuario, string nombrePagina, string permiso)
        {
            DA_Parametro daP = new(Configuration.GetConnectionString("DefaultConnection"));
            DataTable dtPagina = daP.ObtenerPermisos(idUsuario, null, null, null, nombrePagina);
            if (dtPagina.Rows.Count == 0)
                return false;

            int idPagina = (int)dtPagina.Rows[0]["IdPermiso"];
            return daP.ObtenerPermisos(idUsuario, 'P', null, idPagina, null).Rows.Cast<DataRow>()
                      .Any(r => (r["HRef"] != DBNull.Value ? r["HRef"].ToString() : r["Nombre"].ToString()) == permiso);
        }

        [HttpGet("sucursales")]
        public List<SucursalesPorCliente> GetSucursalesPorCliente(string codCliente)
        {
            DA_Clientes daC = new(Configuration.GetConnectionString("DefaultConnection"));
            List<SucursalesPorCliente> lst = [];
            foreach (DataRow row in daC.GetSucursalesPorCliente(codCliente).Rows)
            {
                SucursalesPorCliente c = new SucursalesPorCliente();
                c.CodCliente = row["CodCliente"].ToString();
                c.CodigoPostal = row["CodigoPostal"].ToString();
                c.Direccion = row["Direccion"].ToString();
                c.Localidad = row["Localidad"].ToString();
                c.Habitual = Convert.ToBoolean(row["Habitual"].ToString());
                lst.Add(c);
            }
            return [.. lst.OrderByDescending(x => x.Habitual)];
        }
    }
}
