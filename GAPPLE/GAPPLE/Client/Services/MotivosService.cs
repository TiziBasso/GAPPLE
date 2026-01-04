using GAPPLE.Client.Entities;
using GAPPLE.Shared.Model;
using Microsoft.AspNetCore.Components;
using System.Net;
using System.Net.Http.Json;

namespace GAPPLE.Client.Services
{
    public class MotivosService
    {
        [Inject]
        private HttpClient HttpClient { get; set; }
        private SesionDTO SesionDTO { get; }
        private const string URI_BASE = "api/motivos";
        public MotivosService(HttpClient httpClient) => HttpClient = httpClient;

        public async ValueTask<List<Motivos>> GetMotivos(string Descripcion = null, bool? Pasivo = null, int? IdDesposito = null, string DescripcionDesposito = null)
        {

            string uri = $"{URI_BASE}";
            Dictionary<string, object> query = new();
            if (Descripcion != null) query["Descripcion"] = Descripcion;
            if (Pasivo != null) query["Pasivo"] = Pasivo;
            if (IdDesposito != null) query["IdDeposito"] = IdDesposito;
            if (DescripcionDesposito != null) query["DescripcionDeposito"] = DescripcionDesposito;

            if (query.Any())
                uri += $"?{string.Join("&", query.Select(x => $"{x.Key}={x.Value}").ToArray())}";

            return await HttpClient.GetFromJsonAsync<List<Motivos>>(uri);
        }

        public async ValueTask<Response> PutMotivos(Motivos motivos)
        {

            var response = await HttpClient.PutAsJsonAsync($"{URI_BASE}", motivos);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                return new(true);
            }
            else if (response.StatusCode == HttpStatusCode.BadRequest)
                return new(false, await response.Content.ReadFromJsonAsync<Dictionary<string, List<string>>>());
            else
                return new(false);
        }

      


    }
}