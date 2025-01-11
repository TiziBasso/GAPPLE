using GAPPLE.Client.Entities;
using GAPPLE.Shared.Model;
using Microsoft.AspNetCore.Components;
using System.Net.Http.Json;
using System.Net;
using GAPPLE.Client.Tools;

namespace GAPPLE.Client.Services
{
    public class OrdenesService
    {
        [Inject]
        private HttpClient HttpClient { get; set; }
        private const string URI_BASE = "api/ordenes";

        public OrdenesService(HttpClient httpClient) => HttpClient = httpClient;

        public async ValueTask<List<Orden>> GetOrdenes(DateTime desde, DateTime hasta, int? idPedido, string? codOrden, bool? presupuesto, string? razonSocial,
                                        string? linea, string? zona, int? idEstado, string? codTango)
        {
            string uri = $"{URI_BASE}/lista";
            Dictionary<string, object> query = new();
            query["desdeStr"] = WebUtility.UrlEncode(desde.ToString()!);
            query["hastaStr"] = WebUtility.UrlEncode(hasta.AddHours(23).AddMinutes(59).AddSeconds(59).ToString()!);
            if (idPedido != null) query["idPedido"] = idPedido;
            if (presupuesto != null) query["presupuesto"] = presupuesto;
            if (!string.IsNullOrWhiteSpace(razonSocial)) query["razonSocial"] = razonSocial;
            if (!string.IsNullOrWhiteSpace(linea)) query["linea"] = linea;
            if (!string.IsNullOrWhiteSpace(zona)) query["zona"] = zona;
            if (!string.IsNullOrWhiteSpace(codOrden)) query["codOrden"] = codOrden;
            if (idEstado != null) query["idEstado"] = idEstado;
            if (!string.IsNullOrWhiteSpace(codTango)) query["codTango"] = codTango;

            uri += $"?{string.Join("&", query.Select(x => $"{x.Key}={x.Value}").ToArray())}";

            return await HttpClient.GetFromJsonAsync<List<Orden>>(uri);
        }

        public async ValueTask<Orden?> GetOrden(string codOrden, bool conDetalle = true)
        {
            string uri = $"{URI_BASE}";
            Dictionary<string, object> query = new();
            query["codOrden"] = codOrden;
            query["conDetalle"] = conDetalle;

            uri += $"?{string.Join("&", query.Select(x => $"{x.Key}={x.Value}").ToArray())}";

            return await HttpClient.GetFromJsonAsync<Orden?>(uri);
        }

        public async ValueTask<List<Transporte>> GetTransportes()
        {
            return await HttpClient.GetFromJsonAsync<List<Transporte>>($"{URI_BASE}/transportes");
        }
        public async ValueTask<List<OrdenDashboard>> GetOrdenDashboard()
        {
            return await HttpClient.GetFromJsonAsync<List<OrdenDashboard>>($"{URI_BASE}/ordenDashboard");
        }
        public async ValueTask<List<CondicionDeVenta>> GetCondicionesDeVenta()
        {
            return await HttpClient.GetFromJsonAsync<List<CondicionDeVenta>>($"{URI_BASE}/condicionesdeventa");
        }
        public async ValueTask<List<ListaDePrecios>> GetListasDePrecio()
        {
            return await HttpClient.GetFromJsonAsync<List<ListaDePrecios>>($"{URI_BASE}/listasdeprecio");
        }
        public async ValueTask<List<Zonas>> GetZonas()
        {
            return await HttpClient.GetFromJsonAsync<List<Zonas>>($"{URI_BASE}/zonas");
        }

        public async ValueTask<List<Opcion>> GetEstados()
        {
            return await HttpClient.GetFromJsonAsync<List<Opcion>>($"{URI_BASE}/estados");
        }

        public async ValueTask<Response> PostPedido(Orden pedido)
        {
            try
            {
                var response = await HttpClient.PostAsJsonAsync($"{URI_BASE}", pedido);
                if (response.IsSuccessStatusCode)
                {
                    Orden p = await response.Content.ReadFromJsonAsync<Orden>();
                    pedido.CodigoOrden = p.CodigoOrden;
                    return new(true);
                }
                else if (response.StatusCode == HttpStatusCode.BadRequest)
                    return new(false, await response.Content.ReadFromJsonAsync<Dictionary<string, List<string>>>());
                else
                    throw new Exception(await response.Content.ReadAsStringAsync());
            }
            catch (Exception ex)
            {
                return new(false);
            }
        }

        public async ValueTask<Response> CambiarEstadoPedidos(string id, int idEstado)
        {
            try
            {
                var response = await HttpClient.PutAsJsonAsync($"{URI_BASE}/{idEstado}", id);
                if (response.IsSuccessStatusCode)
                    return new(true);
                else if (response.StatusCode == HttpStatusCode.BadRequest)
                    return new(false, await response.Content.ReadFromJsonAsync<Dictionary<string, List<string>>>());
                else
                    throw new Exception(await response.Content.ReadAsStringAsync());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new(false);
            }
        }

        public async ValueTask<List<OrdenExpedicion>> GetOrdenesExpediciones()
        {
            return await HttpClient.GetFromJsonAsync<List<OrdenExpedicion>>($"{URI_BASE}/expediciones");
        }

        public async ValueTask<OrdenExpedicion> GetOrdenExpedicion(string idOrden)
        {
            return await HttpClient.GetFromJsonAsync<OrdenExpedicion>($"{URI_BASE}/expedicion?idOrden={idOrden}");
        }
    }
}
