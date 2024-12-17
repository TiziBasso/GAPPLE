using GAPPLE.Client.Pages;
using GAPPLE.Server.Data;
using GAPPLE.Shared.Model;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Net;

namespace GAPPLE.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdenesController : ControllerBase
    {
        private IConfiguration Configuration { get; }
        private Usuario Usuario { get; }

        public OrdenesController(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        [HttpGet]
        public List<Orden> GetOrdenes(int? idOrden, string? cliente, string? desdeStr, string? hastaStr, int? idEstado)
        {
            DateTime? desde = null, hasta = null;
            if (desdeStr != null) desde = DateTime.Parse(WebUtility.UrlDecode(desdeStr));
            if (hastaStr != null) hasta = DateTime.Parse(WebUtility.UrlDecode(hastaStr));
            DA_Ordenes daO = new(Configuration.GetConnectionString("DefaultConnection"));
            List<Orden> lstOrdenes = new();
            using (DataTable dt = daO.ObtenerOrdenes(idOrden, cliente, desde, hasta, idEstado))
            {
                foreach (DataRow row in dt.Rows)
                {
                    Orden o = new()
                    {
                        Id = (int)row["IdPedido"],
                        Presupuesto = (bool)row["Presupuesto"],
                        Cliente = row["RazonSocial"].ToString(),
                        Linea = row["Linea"].ToString(),
                        Creacion = (DateTime)row["AltaRegistro"],
                        Zona = row["Zona"].ToString(),
                        DescripcionEstado = row["DescripcionEstado"].ToString(),
                        NumeroFactura = row["NumFactura"].ToString(),
                        Unidades = (int)row["CantidadLineas"]
                    };

                    lstOrdenes.Add(o);
                }
            }
            return lstOrdenes;
        }

        [HttpPost]
        public IActionResult PostOfertas(Oferta oferta)
        {
            DA_Ofertas daO = new(Configuration.GetConnectionString("DefaultConnection"));
            daO.PersistirOferta(oferta.Nombre, oferta.Linea, oferta.Descripcion, oferta.Descuento, oferta.Desde, oferta.Hasta, oferta.Inclusiones!);
            return Ok();
        }

        [HttpPut]
        public IActionResult PutOfertas(Oferta oferta)
        {
            DA_Ofertas daO = new(Configuration.GetConnectionString("DefaultConnection"));
            daO.EditarOferta(oferta.IdOferta, oferta.Nombre, oferta.Linea, oferta.Descripcion, oferta.Descuento, oferta.Desde, oferta.Hasta, oferta.Inclusiones!);
            return Ok();
        }
    }
}
