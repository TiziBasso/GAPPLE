using GAPPLE.Client.Entities;
using GAPPLE.Shared.Model;
using Microsoft.AspNetCore.Components;
using System.Net.Http.Json;
using System.Net;

namespace GAPPLE.Client.Services
{
    public class DepositoService
    {
        [Inject]
        private HttpClient HttpClient { get; set; }
        private SesionDTO SesionDTO { get; }
        private const string URI_BASE = "api/depositos";
        public DepositoService(HttpClient httpClient) => HttpClient = httpClient;


        public async ValueTask<List<Deposito>> GetDepositos(string? CodigoTango = null, string? Descripcion = null, bool? Visible = null)
        {
            
            string uri = $"{URI_BASE}";
            Dictionary<string, object> query = new();
            if(CodigoTango != null) query ["CodigoTango"] = CodigoTango;
            if(Descripcion != null) query ["Descripcion"] = Descripcion;
            if(Visible != null) query ["Visible"] = Visible;

            if (query.Any())
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
