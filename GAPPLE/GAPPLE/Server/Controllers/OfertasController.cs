using GAPPLE.Shared.Model;
using Microsoft.AspNetCore.Mvc;

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
        public List<Oferta> GetOfertas()
        {
            return new();
        }
    }
}
