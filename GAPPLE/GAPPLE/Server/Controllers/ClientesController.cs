using System.Data;
using GAPPLE.Server.Data;
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
        public List<Cliente> GetClientes(string? codCliente = null, string? razonSocial = null, string? cuit = null, bool? clienteEspecial = null, int? idUsuario = null)
        {
            DA_Clientes daC = new(Configuration.GetConnectionString("DefaultConnection"));
            List<Cliente> lst = new();
            foreach (DataRow row in daC.ObtenerClientes(codCliente, razonSocial?.Trim(), cuit, clienteEspecial, idUsuario, null).Rows)
            {
                Cliente c = new Cliente();
                c.IdCliente = int.Parse(row["IdCliente"].ToString()!);
                c.CodigoCliente = row["CodigoCliente"].ToString()!;
                c.RazonSocial = row["RazonSocial"].ToString()!;
                c.Observaciones = row["Observaciones"].ToString()!;
                c.ClienteEspecial = bool.Parse(row["ClienteEspecial"].ToString()!);
                c.CUIT = row["CUIT"].ToString()!;
                c.CodListaPrecioDefault = row["IdListaDePrecio"].ToString()!;
                c.CondVentaDefault = row["CondVenta"].ToString()!;
                c.ZonaDefault = row["Zona"].ToString()!;
                if (row["ID_GVA"] != DBNull.Value) c.Id_GVA = int.Parse(row["ID_GVA"].ToString());
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
                ArticulosPorCliente c = new ArticulosPorCliente();
                c.CodProducto = row["CodProducto"].ToString()!;
                c.Descuento = decimal.Parse(row["Bonificacion"].ToString()!);
                lst.Add(c);
            }
            return lst;
        }

        [HttpGet("sucursales")]
        public List<SucursalesPorCliente> GetSucursalesPorCliente(string codCliente)
        {
            DA_Clientes daC = new(Configuration.GetConnectionString("DefaultConnection"));
            List<SucursalesPorCliente> lst = new();
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
            return lst.OrderByDescending(x => x.Habitual).ToList();
        }
    }
}
