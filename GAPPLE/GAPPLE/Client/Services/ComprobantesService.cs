using GAPPLE.Client.Entities;
using GAPPLE.Shared.Model;
using GAPPLE.Shared.Requests;
using Microsoft.AspNetCore.Components;
using System.Net.Http.Json;
using System.Net;

namespace GAPPLE.Client.Services
{
    public class ComprobantesService
    {
        [Inject]
        private HttpClient HttpClient { get; set; }
        private ILogger<ComprobantesService> Logger { get; }
        [Inject]
        private SesionDTO SesionDTO { get; }

        private const string URI_BASE = "api/comprobantes";

        public ComprobantesService(HttpClient httpClient, ILogger<ComprobantesService> logger, SesionDTO sesionDTO) => (HttpClient, Logger, SesionDTO) = (httpClient, logger, sesionDTO);

        public async ValueTask<ComprobanteCabecera> GetNotaCredito(int idComprobante)
        {
            var aux = await GetNotasCredito(new() { IdComprobante = idComprobante, ConDetalle = true });

            if (aux != null && aux.Count > 0)
                return aux.First();

            return null;
        }

        public async ValueTask<List<ComprobanteCabecera>> GetNotasCredito(ComprobanteCabeceraRequest request)
        {
            var response = await HttpClient.PostAsJsonAsync($"{URI_BASE}/notacredito/obtener", request);

            if (response.StatusCode == HttpStatusCode.OK)
                return await response.Content.ReadFromJsonAsync<List<ComprobanteCabecera>>();

            return null;
        }

        public async ValueTask<Response> RevertirNotaCredito(int idComprobante)
        {
            var response = await HttpClient.PutAsJsonAsync($"{URI_BASE}/notacredito/{idComprobante}/revertir", SesionDTO.Nombre);

            if (response.StatusCode == HttpStatusCode.OK)
                return new(response.StatusCode);

            return await Response.CreateAsync(response);
        }

        public async ValueTask<Response> CancelarNotaCredito(int idComprobante)
        {
            var response = await HttpClient.PutAsJsonAsync($"{URI_BASE}/notacredito/{idComprobante}/cancelar", SesionDTO.Nombre);

            if (response.StatusCode == HttpStatusCode.OK)
                return new(response.StatusCode);

            return await Response.CreateAsync(response);
        }

        public async ValueTask<Response> AprobarNotaCredito(int idComprobante)
        {
            var response = await HttpClient.PutAsJsonAsync($"{URI_BASE}/notacredito/{idComprobante}/aprobar", SesionDTO.Nombre);

            if (response.StatusCode == HttpStatusCode.OK)
                return new(response.StatusCode);

            return await Response.CreateAsync(response);
        }

        public async ValueTask<Response> PostNotaCredito(ComprobanteCabecera comprobante)
        {
            comprobante.AltaUsuario = SesionDTO.Nombre;
            var response = await HttpClient.PostAsJsonAsync($"{URI_BASE}/notacredito", comprobante);
            if (response.StatusCode == HttpStatusCode.OK)
                return new(response.StatusCode, await response.Content.ReadFromJsonAsync<ComprobanteCabecera>());

            return await Response.CreateAsync(response);
        }

        public async ValueTask<Response> PutNotaCredito(ComprobanteCabecera comprobante)
        {
            comprobante.EdicionUsuario = SesionDTO.Nombre;
            var response = await HttpClient.PutAsJsonAsync($"{URI_BASE}/notacredito", comprobante);
            if (response.StatusCode == HttpStatusCode.OK)
                return new(response.StatusCode, await response.Content.ReadFromJsonAsync<ComprobanteCabecera>());

            return await Response.CreateAsync(response);
        }
    }
}
