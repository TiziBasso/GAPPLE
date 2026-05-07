using GAPPLE.Client.Entities;
using GAPPLE.Shared.Model;
using Microsoft.AspNetCore.Components;
using System.Net.Http.Json;
using System.Net;
using GAPPLE.Shared.Requests;
using GAPPLE.Shared.Enums;

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
                return new(response.StatusCode, await response.Content.ReadFromJsonAsync<List<AcuerdoCliente>>());

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

        public async Task<Response> DeleteAcuerdos(Acuerdo acuerdo) =>
            await Response.CreateAsync(await HttpClient.DeleteAsync($"{URI_BASE}/{acuerdo.IdAcuerdo}"));

        public async Task<Response> CambiarEstadoAcuerdo(Acuerdo acuerdo, AcuerdosEstadoEnum idEstado)
        {
            acuerdo.EdicionUsuario = SesionDTO.Nombre;
            var response = await HttpClient.PutAsJsonAsync($"{URI_BASE}/estado/{(int)idEstado}", acuerdo);

            if (response.StatusCode == HttpStatusCode.OK)
                return new(response.StatusCode, await response.Content.ReadFromJsonAsync<Acuerdo>());

            return await Response.CreateAsync(response);
        }

        #region AcuerdosMontos
        public async Task<Response> GetAcuerdosMontos(AcuerdoMontosRequest request)
        {
            var response = await HttpClient.PostAsJsonAsync($"{URI_BASE}/montos/obtener", request);

            if (response.StatusCode == HttpStatusCode.OK)
                return new(response.StatusCode, await response.Content.ReadFromJsonAsync<List<AcuerdoMonto>>());

            return await Response.CreateAsync(response);
        }

        public async Task<Response> PostAcuerdosMonto(AcuerdoMonto acuerdoMonto)
        {
            acuerdoMonto.AltaUsuario = SesionDTO.Nombre;
            var response = await HttpClient.PostAsJsonAsync($"{URI_BASE}/montos", acuerdoMonto);

            if (response.StatusCode == HttpStatusCode.OK)
                return new(response.StatusCode, await response.Content.ReadFromJsonAsync<AcuerdoMonto>());

            return await Response.CreateAsync(response);
        }

        public async Task<Response> PutAcuerdosMonto(AcuerdoMonto acuerdoMonto)
        {
            acuerdoMonto.EdicionUsuario = SesionDTO.Nombre;
            var response = await HttpClient.PutAsJsonAsync($"{URI_BASE}/montos", acuerdoMonto);

            if (response.StatusCode == HttpStatusCode.OK)
                return new(response.StatusCode, await response.Content.ReadFromJsonAsync<AcuerdoMonto>());

            return await Response.CreateAsync(response);
        }

        public async Task<Response> DeleteAcuerdosMonto(AcuerdoMonto acuerdoMonto) =>
            await Response.CreateAsync(await HttpClient.DeleteAsync($"{URI_BASE}/montos/{acuerdoMonto.Id}"));
        #endregion
    }
}