using Microsoft.AspNetCore.Components;

namespace GAPPLE.Client.Services
{
    public class SeguridadService
    {
        [Inject]
        private HttpClient HttpClient { get; set; }
        private const string URI_BASE = "api/clientes";

        public SeguridadService(HttpClient httpClient) => HttpClient = httpClient;


    }
}
