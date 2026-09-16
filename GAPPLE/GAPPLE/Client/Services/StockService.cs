using GAPPLE.Shared.Model;
using System.Net;
using System.Net.Http.Json;

namespace GAPPLE.Client.Services
{
    public class StockService
    {
        private HttpClient HttpClient { get; }
        private const string URI_BASE = "api/stock";

        public StockService(HttpClient httpClient) => HttpClient = httpClient;

        /// <summary>
        /// Stock consolidado (depositos 01 y 6). Sin parametros trae el dataset completo:
        /// la pantalla filtra en memoria para que la busqueda sea instantanea.
        /// </summary>
        public async ValueTask<List<StockConsolidado>> GetStock(string codigo = null, string descripcion = null, string marca = null)
        {
            string uri = URI_BASE;
            Dictionary<string, object> query = new();
            if (!string.IsNullOrWhiteSpace(codigo)) query["codigo"] = codigo;
            if (!string.IsNullOrWhiteSpace(descripcion)) query["descripcion"] = descripcion;
            if (!string.IsNullOrWhiteSpace(marca)) query["marca"] = marca;

            if (query.Count != 0)
                uri += $"?{string.Join("&", query.Select(x => $"{x.Key}={Uri.EscapeDataString(x.Value.ToString())}"))}";

            var response = await HttpClient.GetAsync(uri);

            if (response.StatusCode == HttpStatusCode.OK)
                return await response.Content.ReadFromJsonAsync<List<StockConsolidado>>();

            return null;
        }
    }
}
