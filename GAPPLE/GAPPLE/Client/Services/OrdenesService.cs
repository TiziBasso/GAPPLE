using GAPPLE.Client.Entities;
using GAPPLE.Shared.Model;
using Microsoft.AspNetCore.Components;
using System.Net.Http.Json;
using System.Net;
using GAPPLE.Client.Tools;

namespace GAPPLE.Client.Services
{
    public class OrdenesService
    {
        [Inject]
        private HttpClient HttpClient { get; set; }
        private const string URI_BASE = "api/ordenes";

        public OrdenesService(HttpClient httpClient) => HttpClient = httpClient;

        public async ValueTask<List<Orden>> GetOrdenes(int? idOrden, string? cliente, DateTime? desde, DateTime? hasta, int? idEstado)
        {
            string uri = $"{URI_BASE}";
            Dictionary<string, object> query = new();
            if (idOrden != null) query["idOrden"] = idOrden;
            if (cliente != null) query["cliente"] = cliente;
            if (desde != null) query["desde"] = WebUtility.UrlEncode(desde.ToString()!);
            if (hasta != null) query["hasta"] = WebUtility.UrlEncode(hasta.ToString()!);
            if (idEstado != null) query["idEstado"] = idEstado;


            if (query.Any())
                uri += $"?{string.Join("&", query.Select(x => $"{x.Key}={x.Value}").ToArray())}";

            return await HttpClient.GetFromJsonAsync<List<Orden>>(uri);
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
