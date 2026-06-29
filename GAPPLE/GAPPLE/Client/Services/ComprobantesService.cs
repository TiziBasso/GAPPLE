using GAPPLE.Client.Entities;
using GAPPLE.Shared.Model;
using GAPPLE.Shared.Requests;
using Microsoft.AspNetCore.Components;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;

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
            request.IdUsuario = SesionDTO.IdUsuario;
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

        public async ValueTask<Response> AprobarNotaCredito(int idComprobante, string numeroNC)
        {
            var response = await HttpClient.PutAsJsonAsync($"{URI_BASE}/notacredito/{idComprobante}/aprobar/{numeroNC}", SesionDTO.Nombre);

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

        public async ValueTask<List<NotaCreditoArchivo>> GetArchivos(int idComprobante)
        {
            var response = await HttpClient.GetAsync($"{URI_BASE}/notacredito/{idComprobante}/archivos");

            if (response.StatusCode == HttpStatusCode.OK)
                return await response.Content.ReadFromJsonAsync<List<NotaCreditoArchivo>>();

            return [];
        }

        public async ValueTask<Response> PostNotaCreditoArchivo(int idComprobante, List<NotaCreditoArchivo> archivos)
        {
            var response = await HttpClient.PostAsJsonAsync($"{URI_BASE}/notacredito/{idComprobante}/archivos", archivos);
            if (response.StatusCode == HttpStatusCode.OK)
                return new(response.StatusCode, await response.Content.ReadFromJsonAsync<List<NotaCreditoArchivo>>());

            return await Response.CreateAsync(response);
        }

        public async ValueTask<Response> DeleteNotaCreditoArchivo(NotaCreditoArchivo archivo)
        {
            var response = await HttpClient.DeleteAsync($"{URI_BASE}/notacredito/{archivo.IdComprobante}/archivo/{archivo.IdArchivo}");
            if (response.StatusCode == HttpStatusCode.OK)
                return new(response.StatusCode);

            return await Response.CreateAsync(response);
        }

        public async ValueTask<Response> GetNotaCreditoArchivos(List<NotaCreditoArchivo> archivos)
        {
            var response = await HttpClient.PostAsJsonAsync($"{URI_BASE}/notacredito/archivos/download", archivos);

            if (response.StatusCode == HttpStatusCode.OK)
                return new(response.StatusCode, await response.Content.ReadAsByteArrayAsync());

            return await Response.CreateAsync(response);
        }

        public async ValueTask<List<ComprobanteDetalle>> GetNotaCreditoDetalle(int idComprobante)
        {
            var aux = await GetNotasCredito(new() { IdComprobante = idComprobante, ConDetalle = true });

            if (aux != null && aux.Count > 0)
                return aux.First().Detalle;

            return null;
        }
    }
}
