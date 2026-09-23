using GAPPLE.Shared.Model;
using Microsoft.AspNetCore.Components;
using System.Net;
using System.Net.Http.Json;

namespace GAPPLE.Client.Services
{
    public class VendedoresPorClienteService
    {
        [Inject]
        private HttpClient HttpClient { get; set; }
        private const string URI_BASE = "api/vendedoresporcliente";

        public VendedoresPorClienteService(HttpClient httpClient) => HttpClient = httpClient;

        public async ValueTask<List<VendedorPorCliente>> GetVendedoresPorCliente(int idCliente)
        {
            return await HttpClient.GetFromJsonAsync<List<VendedorPorCliente>>($"{URI_BASE}?idCliente={idCliente}") ?? new();
        }

        public async ValueTask<string> GetVendedorDefault(int idCliente)
        {
            var response = await HttpClient.GetAsync($"{URI_BASE}/default?idCliente={idCliente}");
            if (response.StatusCode != HttpStatusCode.OK)
                return null;

            var content = await response.Content.ReadAsStringAsync();
            return string.IsNullOrWhiteSpace(content) || content == "null" ? null : content.Trim('"');
        }

        public async ValueTask<bool> PostVendedoresPorCliente(int idCliente, List<int> idUsuarios)
        {
            var response = await HttpClient.PostAsJsonAsync($"{URI_BASE}?idCliente={idCliente}", idUsuarios);
            return response.IsSuccessStatusCode;
        }

        public async ValueTask<List<Cliente>> GetClientesPorVendedor(int idUsuario)
        {
            return await HttpClient.GetFromJsonAsync<List<Cliente>>($"{URI_BASE}/porvendedor?idUsuario={idUsuario}") ?? new();
        }

        public async ValueTask<bool> PostClientesPorVendedor(int idUsuario, List<int> idClientes)
        {
            var response = await HttpClient.PostAsJsonAsync($"{URI_BASE}/porvendedor?idUsuario={idUsuario}", idClientes);
            return response.IsSuccessStatusCode;
        }
    }
}
