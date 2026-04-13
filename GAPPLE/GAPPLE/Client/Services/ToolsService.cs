using GAPPLE.Shared.Entities;
using GAPPLE.Shared.Model;
using GAPPLE.Shared.Requests;
using Microsoft.AspNetCore.Components;
using System.Net;
using System.Net.Http.Json;

namespace GAPPLE.Client.Services
{
    internal class ToolsService
    {
        [Inject]
        private HttpClient HttpClient { get; }
        [Inject]
        private SesionDTO SesionDTO { get; }
        private const string URI_BASE = "api/tools";

        public ToolsService(HttpClient httpClient, SesionDTO sesionDTO)
        {
            HttpClient = httpClient;
            SesionDTO = sesionDTO;
        }

        public async ValueTask<List<Estado<T>>> GetEstados<T>(EstadoRequest request)
        {
            var response = await HttpClient.PostAsJsonAsync($"{URI_BASE}/estados/obtener", request);
            
            if (response.StatusCode == HttpStatusCode.OK)
                return await response.Content.ReadFromJsonAsync<List<Estado<T>>>();

            return null;
        }

    }
}
