using GAPPLE.Shared.Entities;
using GAPPLE.Shared.Model;
using GAPPLE.Shared.Requests;
using Microsoft.AspNetCore.Components;
using System.Net.Http.Json;

namespace GAPPLE.Client.Services
{
    internal class ToolsService
    {
        [Inject]
        private HttpClient HttpClient { get; }
        [Inject]
        private SesionDTO SesionDTO { get; }
        private NavigationManager NavigationManager { get; }
        private const string URI_BASE = "api/tools";

        public ToolsService(HttpClient httpClient, NavigationManager navigationManager, SesionDTO sesionDTO)
        {
            HttpClient = httpClient;
            NavigationManager = navigationManager;
            SesionDTO = sesionDTO;
        }

        public async ValueTask<List<Estado<T>>> GetEstados<T>(EstadoRequest request)
        {
            var response = await HttpClient.PostAsJsonAsync($"{URI_BASE}/estados/obtener", request);
            
            if (response.IsSuccessStatusCode)
                return await response.Content.ReadFromJsonAsync<List<Estado<T>>>();

            return null;
        }

    }
}
