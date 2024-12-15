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

        public async ValueTask<List<Oferta>> GetOfertas(int? idOferta, string? nombre)
        {
            string uri = $"{URI_BASE}";
            Dictionary<string, object> query = new();
            if (idOferta != null) query.Add("idOferta", idOferta);
            if (nombre != null) query["nombre"] = nombre;

            if (query.Any())
                uri += $"?{string.Join("&", query.Select(x => $"{x.Key}={x.Value}").ToArray())}";

            return await HttpClient.GetFromJsonAsync<List<Oferta>>(uri);
        }

        public async ValueTask<Response> PostCliente(Cliente cliente)
        {
            var response = await HttpClient.PostAsJsonAsync($"{URI_BASE}", cliente);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var clienteResponse = await response.Content.ReadFromJsonAsync<Cliente>();
                return new(true);
            }
            else if (response.StatusCode == HttpStatusCode.BadRequest)
                return new(false, await response.Content.ReadFromJsonAsync<Dictionary<string, List<string>>>());
            else
                return new(false, "Ha ocurrido un error inesperado! Por favor contacte a sistemas!");
        }
    }
}
