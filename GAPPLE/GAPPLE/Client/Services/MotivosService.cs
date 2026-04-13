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
        [Inject]
        private SesionDTO SesionDTO { get; }
        private const string URI_BASE = "api/motivos";
        public MotivosService(HttpClient httpClient, SesionDTO sesionDTO) => (HttpClient, SesionDTO) = (httpClient, sesionDTO);

        public async ValueTask<List<Motivo>> GetMotivos(int? idMotivo = null, string descripcion = null, bool? pasivo = null, int? idDesposito = null)
        {

            string uri = $"{URI_BASE}";
            Dictionary<string, object> query = [];
            if (idMotivo != null) query["idMotivo"] = idMotivo;
            if (descripcion != null) query["descripcion"] = descripcion;
            if (pasivo != null) query["pasivo"] = pasivo;
            if (idDesposito != null) query["idDeposito"] = idDesposito;

            if (query.Count != 0)
                uri += $"?{string.Join("&", query.Select(x => $"{x.Key}={x.Value}").ToArray())}";

            var response = await HttpClient.GetAsync(uri);

            if (response.StatusCode == HttpStatusCode.OK)
                return await response.Content.ReadFromJsonAsync<List<Motivo>>();

            return null;
        }

        public async ValueTask<Response> PutMotivo(Motivo motivo)
        {
            motivo.EdicionUsuario = SesionDTO.Nombre;
            var response = await HttpClient.PutAsJsonAsync($"{URI_BASE}", motivo);

            if (response.StatusCode == HttpStatusCode.OK)
                return new(response.StatusCode, await response.Content.ReadFromJsonAsync<Motivo>());

            return await Response.CreateAsync(response);
        }

        public async ValueTask<Response> PostMotivo(Motivo motivo)
        {
            motivo.AltaUsuario = SesionDTO.Nombre;
            var response = await HttpClient.PostAsJsonAsync($"{URI_BASE}", motivo);

            if (response.StatusCode == HttpStatusCode.OK)
                return new(response.StatusCode, await response.Content.ReadFromJsonAsync<Motivo>());

            return await Response.CreateAsync(response);
        }
    }
}