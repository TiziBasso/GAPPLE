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
        [Inject]
        private SesionDTO SesionDTO { get; set; }
        private const string URI_BASE = "api/ofertas";

        public OfertasService(HttpClient httpClient, SesionDTO sesionDTO) => (HttpClient, SesionDTO) = (httpClient, sesionDTO);

        public async ValueTask<List<Oferta>> GetOfertas(string? nombre, string? linea, DateTime? mes, bool? activas = null)
        {
            string uri = $"{URI_BASE}";
            Dictionary<string, object> query = new();
            if (nombre != null) query["nombre"] = nombre;
            if (linea != null) query["linea"] = linea;
            if (mes != null) query["mes"] = WebUtility.UrlEncode(mes.Value.ToString("MM/dd/yyyy HH:mm"));
            if (activas != null) query["activas"] = activas;

            if (query.Any())
                uri += $"?{string.Join("&", query.Select(x => $"{x.Key}={x.Value}").ToArray())}";

            return await HttpClient.GetFromJsonAsync<List<Oferta>>(uri);
        }

        public async ValueTask<List<Oferta>> GetOfertasEspeciales(string? nombre, string? linea, DateTime? mes, bool? activas = null, string idCliente = null)
        {
            string uri = $"{URI_BASE}/especiales";
            Dictionary<string, object> query = new();
            if (nombre != null) query["nombre"] = nombre;
            if (linea != null) query["linea"] = linea;
            if (mes != null) query["mes"] = WebUtility.UrlEncode(mes.Value.ToString("MM/dd/yyyy HH:mm"));
            if (activas != null) query["activas"] = activas;
            if (idCliente != null) query["codCliente"] = idCliente;

            if (query.Any())
                uri += $"?{string.Join("&", query.Select(x => $"{x.Key}={x.Value}").ToArray())}";

            return await HttpClient.GetFromJsonAsync<List<Oferta>>(uri);
        }

        public async ValueTask<Response> PostOferta(Oferta oferta)
        {
            oferta.AltaUsuario = SesionDTO.Nombre;
            var response = await HttpClient.PostAsJsonAsync($"{URI_BASE}", oferta);
            if (response.StatusCode == HttpStatusCode.OK)
                return new(response.StatusCode);
            else if (response.StatusCode == HttpStatusCode.BadRequest)
                return new(response.StatusCode, await response.Content.ReadFromJsonAsync<Dictionary<string, List<string>>>());
            else
                return new(response.StatusCode, "Ha ocurrido un error inesperado! Por favor contacte a sistemas!");
        }

        public async ValueTask<Response> PutOferta(Oferta oferta)
        {
            oferta.EdicionUsuario = SesionDTO.Nombre;
            var response = await HttpClient.PutAsJsonAsync($"{URI_BASE}", oferta);
            if (response.StatusCode == HttpStatusCode.OK)
                return new(response.StatusCode);
            else if (response.StatusCode == HttpStatusCode.BadRequest)
                return new(response.StatusCode, await response.Content.ReadFromJsonAsync<Dictionary<string, List<string>>>());
            else
                return new(response.StatusCode, "Ha ocurrido un error inesperado! Por favor contacte a sistemas!");
        }

        public async ValueTask<Response> PostOfertaEspecial(Oferta oferta)
        {
            oferta.AltaUsuario = SesionDTO.Nombre;
            var response = await HttpClient.PostAsJsonAsync($"{URI_BASE}/Especial", oferta);
            if (response.StatusCode == HttpStatusCode.OK)
                return new(response.StatusCode);
            else if (response.StatusCode == HttpStatusCode.BadRequest)
                return new(response.StatusCode, await response.Content.ReadFromJsonAsync<Dictionary<string, List<string>>>());
            else
                return new(response.StatusCode, "Ha ocurrido un error inesperado! Por favor contacte a sistemas!");
        }

        public async ValueTask<Response> PutOfertaEspecial(Oferta oferta)
        {
            oferta.EdicionUsuario = SesionDTO.Nombre;
            var response = await HttpClient.PutAsJsonAsync($"{URI_BASE}/Especial", oferta);
            if (response.StatusCode == HttpStatusCode.OK)
                return new(response.StatusCode);
            else if (response.StatusCode == HttpStatusCode.BadRequest)
                return new(response.StatusCode, await response.Content.ReadFromJsonAsync<Dictionary<string, List<string>>>());
            else
                return new(response.StatusCode, "Ha ocurrido un error inesperado! Por favor contacte a sistemas!");
        }

        public async ValueTask<Response> ProcesarArchivoOfertas(OfertaExcelRequest req)
        {
            try
            {
                var response = await HttpClient.PostAsJsonAsync($"{URI_BASE}/procesar", req);
                if (response.StatusCode == HttpStatusCode.OK)
                    return new(response.StatusCode, await response.Content.ReadFromJsonAsync<List<Oferta>>());
                else if (response.StatusCode == HttpStatusCode.BadRequest)
                    return new(response.StatusCode, await response.Content.ReadFromJsonAsync<Dictionary<string, List<string>>>());
                else
                    throw new Exception(await response.Content.ReadAsStringAsync());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new(HttpStatusCode.InternalServerError);
            }
        }

        public async ValueTask<Response> PostOfertasMasivo(List<Oferta> ofertas)
        {
            foreach (var o in ofertas)
                o.AltaUsuario = SesionDTO.Nombre;

            var response = await HttpClient.PostAsJsonAsync($"{URI_BASE}/masivo", ofertas);
            if (response.StatusCode == HttpStatusCode.OK)
                return new(response.StatusCode);
            else if (response.StatusCode == HttpStatusCode.BadRequest)
                return new(response.StatusCode, await response.Content.ReadFromJsonAsync<Dictionary<string, List<string>>>());
            else
                return new(response.StatusCode, "Ha ocurrido un error inesperado! Por favor contacte a sistemas!");
        }
    }
}
