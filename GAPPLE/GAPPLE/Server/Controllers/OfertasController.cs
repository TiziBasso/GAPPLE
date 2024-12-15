using GAPPLE.Server.Data;
using GAPPLE.Shared.Model;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace GAPPLE.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OfertasController : ControllerBase
    {
        private IConfiguration Configuration { get; }
        private Usuario Usuario { get; }

        public OfertasController(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        [HttpGet]
        public List<Oferta> GetOfertas(string? nombre, string? linea, DateTime? desde, DateTime? hasta)
        {
            DA_Ofertas daO = new(Configuration.GetConnectionString("DefaultConnection"));
            List<Oferta> lstOfertas = new List<Oferta>();
            using (DataTable dt = daO.ObtenerOfertas(nombre,linea,desde,hasta))
            {
                foreach(DataRow row in dt.Rows)
                {
                    Oferta o = new()
                    {
                        IdOferta = int.Parse(row["IdOferta"].ToString()!),
                        Nombre = row["Nombre"].ToString()!,
                        Linea = row["Linea"].ToString()!,
                        Descuento = decimal.Parse(row["Descuento"].ToString()!),
                        Desde = DateTime.Parse(row["Desde"].ToString()!),
                        Hasta = DateTime.Parse(row["Hasta"].ToString()!),
                        Activa = bool.Parse(row["Activo"].ToString()!)
                    };
                    lstOfertas.Add(o);
                }
            }
            return lstOfertas;
        }
    }
}
