using System.Data;
using System.Reflection.Metadata;
using GAPPLE.Server.Data;
using GAPPLE.Shared;
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

            //HttpContextAccessor = contextAccessor;

            //connectionString = Configuration.GetConnectionString("DefaultConnection");

            //string nombreUsuario = HttpContextAccessor.HttpContext.User.Identity.Name[(HttpContextAccessor.HttpContext.User.Identity.Name.LastIndexOf(@"\") + 1)..].ToUpper();
            //Usuario = new SeguridadController(configuration, parametros).ObtenerUsuario(nombreUsuario);
        }

        [HttpGet]
        public List<Cliente> GetClientes(string? codCliente = null, string? razonSocial = null, string? cuit = null, bool? clienteEspecial = null)
        {
            DA_Clientes daC = new(Configuration.GetConnectionString("DefaultConnection"));
            List<Cliente> lst = new();
            foreach (DataRow row in daC.ObtenerClientes(codCliente, razonSocial?.Trim(), cuit, clienteEspecial, null).Rows)
            {
                lst.Add(new(row["CodigoCliente"].ToString()!,
                            (string)row["RazonSocial"],
                            row["CUIT"].ToString()!));
            }
            return lst;
        }

    }
}
