using GAPPLE.Client.Entities;
using GAPPLE.Shared.Model;
using Microsoft.AspNetCore.Components;
using System.Net.Http.Json;
using System.Net;

namespace GAPPLE.Client.Services
{
    public class DepositosService
    {
        [Inject]
        private HttpClient HttpClient { get; set; }
        private SesionDTO SesionDTO { get; }
        private const string URI_BASE = "api/depositos";
        public DepositosService(HttpClient httpClient) => HttpClient = httpClient;


        public async ValueTask<List<Deposito>> GetDepositos(string codigoTango = null, string descripcion = null, bool? visible = null)
        {

            string uri = $"{URI_BASE}";
            Dictionary<string, object> query = new();
            if (!string.IsNullOrWhiteSpace(codigoTango)) query["codigoTango"] = codigoTango;
            if (!string.IsNullOrWhiteSpace(descripcion)) query["descripcion"] = descripcion;
            if (visible != null) query["visible"] = visible;

            if (query.Count != 0)
                uri += $"?{string.Join("&", query.Select(x => $"{x.Key}={x.Value}").ToArray())}";

            return await HttpClient.GetFromJsonAsync<List<Deposito>>(uri);
        }

        public async ValueTask<Response> PutDepositoVisibilidad(Deposito deposito)
        {

            var response = await HttpClient.PutAsJsonAsync($"{URI_BASE}/visibilidad", deposito);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                return new(response.StatusCode);
            }
            else if (response.StatusCode == HttpStatusCode.BadRequest)
                return new(response.StatusCode, await response.Content.ReadFromJsonAsync<Dictionary<string, List<string>>>());
            else
                return new(HttpStatusCode.InternalServerError);
        }

    }
}
