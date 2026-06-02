using GAPPLE.Client.Entities;
using GAPPLE.Shared.Enums;
using GAPPLE.Shared.Model;
using GAPPLE.Shared.Requests;
using System.Net;
using System.Net.Http.Json;

namespace GAPPLE.Client.Services
{
    public class ReclamosService
    {
        private HttpClient HttpClient { get; }
        private SesionDTO  SesionDTO  { get; }
        private const string URI_BASE = "api/reclamos";

        public ReclamosService(HttpClient httpClient, SesionDTO sesionDTO)
            => (HttpClient, SesionDTO) = (httpClient, sesionDTO);

        // ─── GET lista de reclamos ────────────────────────────────────────────────
        public async ValueTask<List<Reclamo>> GetReclamos(ReclamoRequest request)
        {
            var query = new Dictionary<string, object>
            {
                ["fechaDesde"] = request.FechaDesde.ToString("yyyy-MM-dd"),
                ["fechaHasta"] = request.FechaHasta.ToString("yyyy-MM-dd")
            };
            if (request.RazonSocialCliente != null)
                query["razonSocialCliente"] = request.RazonSocialCliente;
            if (request.Tipo != null)
                query["tipo"]   = (int)request.Tipo;
            if (request.Motivo != null)
                query["motivo"] = (int)request.Motivo;

            string uri = $"{URI_BASE}?{string.Join("&", query.Select(x => $"{x.Key}={x.Value}"))}";
            var response = await HttpClient.GetAsync(uri);

            if (response.StatusCode == HttpStatusCode.OK)
                return await response.Content.ReadFromJsonAsync<List<Reclamo>>();
            return null;
        }

        // ─── GET detalle (SKUs) de un reclamo ─────────────────────────────────────
        public async ValueTask<List<ReclamoDetalle>> GetReclamoDetalle(int idReclamo)
        {
            var response = await HttpClient.GetAsync($"{URI_BASE}/{idReclamo}/detalle");
            if (response.StatusCode == HttpStatusCode.OK)
                return await response.Content.ReadFromJsonAsync<List<ReclamoDetalle>>();
            return null;
        }

        // ─── POST (nuevo reclamo) ─────────────────────────────────────────────────
        public async ValueTask<Response> PostReclamo(Reclamo reclamo)
        {
            reclamo.AltaUsuario = SesionDTO.Nombre;
            var response = await HttpClient.PostAsJsonAsync(URI_BASE, reclamo);
            if (response.StatusCode == HttpStatusCode.OK)
                return new(response.StatusCode, await response.Content.ReadFromJsonAsync<Reclamo>());
            return await Response.CreateAsync(response);
        }

        // ─── PUT (editar reclamo) ─────────────────────────────────────────────────
        public async ValueTask<Response> PutReclamo(Reclamo reclamo)
        {
            reclamo.EdicionUsuario = SesionDTO.Nombre;
            var response = await HttpClient.PutAsJsonAsync(URI_BASE, reclamo);
            if (response.StatusCode == HttpStatusCode.OK)
                return new(response.StatusCode, await response.Content.ReadFromJsonAsync<Reclamo>());
            return await Response.CreateAsync(response);
        }

        // ─── DELETE ───────────────────────────────────────────────────────────────
        public async ValueTask<Response> DeleteReclamo(int idReclamo)
        {
            var response = await HttpClient.DeleteAsync($"{URI_BASE}/{idReclamo}");
            if (response.StatusCode == HttpStatusCode.OK)
                return new(response.StatusCode);
            return await Response.CreateAsync(response);
        }
    }
}
