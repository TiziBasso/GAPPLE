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
    public class ClientesController: ControllerBase
    {
        private IConfiguration Configuration { get; }
        private IHttpContextAccessor HttpContextAccessor { get; }
        private Usuario Usuario { get; }
        private readonly string connectionString;

        public ClientesController(IConfiguration configuration, IHttpContextAccessor contextAccessor, )
        {
            Configuration = configuration;
            
            HttpContextAccessor = contextAccessor;
            
            connectionString = Configuration.GetConnectionString("DefaultConnection");

            string nombreUsuario = HttpContextAccessor.HttpContext.User.Identity.Name[(HttpContextAccessor.HttpContext.User.Identity.Name.LastIndexOf(@"\") + 1)..].ToUpper();
            //Usuario = new SeguridadController(configuration, parametros).ObtenerUsuario(nombreUsuario);
        }

        [HttpGet("clientes")]
        public List<Cliente> GetClientes(int? codCliente = null, string razonSocial = null,int? cuit = null ,bool? clienteEspecial = null)
        {
            DA_Clientes daC = new(Configuration.GetConnectionString("DefaultConnection"));
            List<Cliente> lst = new();
            foreach (DataRow row in daC.ObtenerClientes(codCliente, razonSocial, cuit, clienteEspecial, null).Rows)
            {
                lst.Add(new((int)row["CodigoCliente"],
                            (string)row["RazonSocial"],
                            (int)row["CUIT"]!));
            }
        }

    }
}
