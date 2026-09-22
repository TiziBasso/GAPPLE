using GAPPLE.Server.Data;
using GAPPLE.Shared.Model;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace GAPPLE.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VendedoresPorClienteController : ControllerBase
    {
        private IConfiguration Configuration { get; }
        private string DefaultConnectionString { get; }

        public VendedoresPorClienteController(IConfiguration configuration)
        {
            Configuration = configuration;
            DefaultConnectionString = Configuration.GetConnectionString("DefaultConnection");
        }

        [HttpGet]
        public List<VendedorPorCliente> GetVendedoresPorCliente(int idCliente)
        {
            DA_VendedoresPorCliente da = new(DefaultConnectionString);
            List<VendedorPorCliente> lista = new();
            using DataTable dt = da.ObtenerVendedoresPorCliente(idCliente);
            foreach (DataRow row in dt.Rows)
            {
                lista.Add(new VendedorPorCliente
                {
                    IdCliente = Convert.ToInt32(row["IdCliente"]),
                    IdUsuario = Convert.ToInt32(row["IdUsuario"]),
                    NombreUsuario = row["NombreUsuario"].ToString(),
                    ApellidoYNombre = row["ApellidoYNombre"].ToString()
                });
            }
            return lista;
        }

        [HttpGet("default")]
        public string GetVendedorDefault(int idCliente)
        {
            DA_VendedoresPorCliente da = new(DefaultConnectionString);
            return da.ObtenerVendedorDefaultPorCliente(idCliente);
        }

        [HttpPost]
        public IActionResult PostVendedoresPorCliente(int idCliente, [FromBody] List<int> idUsuarios)
        {
            try
            {
                DA_VendedoresPorCliente da = new(DefaultConnectionString);
                string ids = string.Join(",", idUsuarios);
                da.PersistirVendedoresPorCliente(idCliente, ids);
                return Ok();
            }
            catch (Exception ex) { return StatusCode(500, ex.Message); }
        }

        [HttpDelete]
        public IActionResult DeleteVendedoresPorCliente(int idCliente)
        {
            try
            {
                DA_VendedoresPorCliente da = new(DefaultConnectionString);
                da.EliminarVendedoresPorCliente(idCliente);
                return Ok();
            }
            catch (Exception ex) { return StatusCode(500, ex.Message); }
        }
    }
}
