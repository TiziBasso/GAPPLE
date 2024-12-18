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

        public ClientesController(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        [HttpGet]
        public List<Cliente> GetClientes(string? codCliente = null, string? razonSocial = null, string? cuit = null, bool? clienteEspecial = null)
        {
            DA_Clientes daC = new(Configuration.GetConnectionString("DefaultConnection"));
            List<Cliente> lst = new();
            foreach (DataRow row in daC.ObtenerClientes(codCliente, razonSocial?.Trim(), cuit, clienteEspecial, null).Rows)
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
                lst.Add(c);
            }
            return lst;
        }

        [HttpPost]
        public IActionResult PostClienteEspecial(Cliente cliente)
        {
            DA_Clientes daC = new(Configuration.GetConnectionString("DefaultConnection"));
            daC.PersistirEdicionCliente((int)cliente.IdCliente!,true,cliente.Observaciones!);
            return Ok();
        }

    }


}
