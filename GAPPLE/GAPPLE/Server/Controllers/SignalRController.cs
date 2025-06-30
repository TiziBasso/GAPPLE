using Microsoft.AspNetCore.SignalR;

namespace Integra.Web.Server.Controllers
{
    public class SignalRController : Hub
    {
        public async Task CambiarPorcentajeTarea(IHubClients cliente, string connectionId, int porcentaje)
        {
            await cliente.Client(connectionId).SendAsync("CambiarPorcentaje", porcentaje);
        }

        public async Task CambiarTextoTarea(IHubClients cliente, string connectionId, string text)
        {
            await cliente.Client(connectionId).SendAsync("CambiarTexto", text);
        }
    }
}
