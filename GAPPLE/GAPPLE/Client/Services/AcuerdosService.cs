using GAPPLE.Client.Entities;
using GAPPLE.Shared.Model;
using Microsoft.AspNetCore.Components;
using System.Net.Http.Json;
using System.Net;
using GAPPLE.Shared.Requests;

namespace GAPPLE.Client.Services
{
    public class AcuerdosService
    {
        [Inject]
        private HttpClient HttpClient { get; set; }
        private SesionDTO SesionDTO { get; }
        private const string URI_BASE = "api/acuerdos";

        public AcuerdosService(HttpClient httpClient, SesionDTO sesionDTO)
        {
            HttpClient = httpClient;
            SesionDTO = sesionDTO;
        }

        public async Task<Response> GetAcuerdos(AcuerdosRequest request)
        {
            var response = await HttpClient.PostAsJsonAsync($"{URI_BASE}/obtener", request);

            if (response.StatusCode == HttpStatusCode.OK)
                return new(response.StatusCode, await response.Content.ReadFromJsonAsync<List<Acuerdo>>());

            return await Response.CreateAsync(response);
        }

        public async Task<Response> PostAcuerdo(Acuerdo acuerdo)
        {
            acuerdo.AltaUsuario = SesionDTO.Nombre;
            var response = await HttpClient.PostAsJsonAsync(URI_BASE, acuerdo);

            if (response.StatusCode == HttpStatusCode.OK)
                return new(response.StatusCode, await response.Content.ReadFromJsonAsync<Acuerdo>());

            return await Response.CreateAsync(response);
        }

        public async Task<Response> PutAcuerdo(Acuerdo acuerdo)
        {
            acuerdo.EdicionUsuario = SesionDTO.Nombre;
            var response = await HttpClient.PutAsJsonAsync(URI_BASE, acuerdo);
            if (response.StatusCode == HttpStatusCode.OK)
                return new(response.StatusCode, await response.Content.ReadFromJsonAsync<Acuerdo>());

            return await Response.CreateAsync(response);
        }
    }
}