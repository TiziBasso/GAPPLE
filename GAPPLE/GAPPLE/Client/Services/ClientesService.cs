using GAPPLE.Client.Entities;
using GAPPLE.Shared.Model;
using Microsoft.AspNetCore.Components;
using System.Net.Http.Json;
using System.Net;

namespace GAPPLE.Client.Services
{

    public class ClientesService
    {
        [Inject]
        private HttpClient HttpClient { get; set; }
        private const string URI_BASE = "api/clientes";

        public ClientesService(HttpClient httpClient) => HttpClient = httpClient;

        public async ValueTask<List<Cliente>> GetClientes(string? codCliente = null, string? razonSocial = null, string? cuit = null)
        {
            string uri = $"{URI_BASE}";
            Dictionary<string, object> query = new();
            if (codCliente != null) query["codCliente"] = codCliente;
            if (razonSocial != null) query["razonSocial"] = razonSocial.Trim();
            if (cuit != null) query["cuit"] = cuit;

            if (query.Any())
                uri += $"?{string.Join("&", query.Select(x => $"{x.Key}={x.Value}").ToArray())}";

            return await HttpClient.GetFromJsonAsync<List<Cliente>>(uri);
        }



    }

}
