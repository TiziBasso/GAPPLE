using GAPPLE.Client.Entities;
using GAPPLE.Shared.Model;
using Microsoft.AspNetCore.Components;
using System.Net.Http.Json;
using System.Net;

namespace GAPPLE.Client.Services
{
    public class OfertasService
    {
        [Inject]
        private HttpClient HttpClient { get; set; }
        private const string URI_BASE = "api/ofertas";

        public OfertasService(HttpClient httpClient) => HttpClient = httpClient;

        public async ValueTask<List<Oferta>> GetOfertas(string? nombre, string? linea, DateTime? desde, DateTime? hasta, bool? activas = null)
        {
            string uri = $"{URI_BASE}";
            Dictionary<string, object> query = new();
            if (nombre != null) query["nombre"] = nombre;
            if (linea != null) query["linea"] = linea;
            if (desde != null) query["desde"] = desde;
            if (hasta != null) query["hasta"] = hasta;
            if (activas != null) query["activas"] = activas;

            if (query.Any())
                uri += $"?{string.Join("&", query.Select(x => $"{x.Key}={x.Value}").ToArray())}";

            return await HttpClient.GetFromJsonAsync<List<Oferta>>(uri);
        }

        public async ValueTask<Response> PostOferta(Oferta oferta)
        {
            var response = await HttpClient.PostAsJsonAsync($"{URI_BASE}", oferta);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                return new(true);
            }
            else if (response.StatusCode == HttpStatusCode.BadRequest)
                return new(false, await response.Content.ReadFromJsonAsync<Dictionary<string, List<string>>>());
            else
                return new(false, "Ha ocurrido un error inesperado! Por favor contacte a sistemas!");
        }

        public async ValueTask<Response> PutOferta(Oferta oferta)
        {
            var response = await HttpClient.PutAsJsonAsync($"{URI_BASE}", oferta);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                return new(true);
            }
            else if (response.StatusCode == HttpStatusCode.BadRequest)
                return new(false, await response.Content.ReadFromJsonAsync<Dictionary<string, List<string>>>());
            else
                return new(false, "Ha ocurrido un error inesperado! Por favor contacte a sistemas!");
        }
    }
}
