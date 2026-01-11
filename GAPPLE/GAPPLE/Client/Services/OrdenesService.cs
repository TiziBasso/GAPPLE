using GAPPLE.Client.Entities;
using GAPPLE.Shared.Model;
using GAPPLE.Shared.Requests;
using Microsoft.AspNetCore.Components;
using System.Net;
using System.Net.Http.Json;

namespace GAPPLE.Client.Services
{
    public class OrdenesService
    {
        [Inject]
        private HttpClient HttpClient { get; set; }
        private ILogger<OrdenesService> Logger { get; }
        [Inject]
        private SesionDTO SesionDTO { get; }

        private const string URI_BASE = "api/ordenes";

        public OrdenesService(HttpClient httpClient, ILogger<OrdenesService> logger, SesionDTO sesionDTO) => (HttpClient, Logger, SesionDTO) = (httpClient, logger, sesionDTO);

        public async ValueTask<List<Orden>> GetOrdenes(DateTime desde, DateTime hasta, int? idPedido, string? codOrden, bool? presupuesto, string? razonSocial,
                                        string? linea, string? zona, int? idEstado, string? codTango, int idUsuario, CancellationTokenSource cancellationToken)
        {
            string uri = $"{URI_BASE}/lista";
            Dictionary<string, object> query = new();
            query["desdeStr"] = WebUtility.UrlEncode(desde.ToString()!);
            query["hastaStr"] = WebUtility.UrlEncode(hasta.ToString()!);
            if (idPedido != null) query["idPedido"] = idPedido;
            if (presupuesto != null) query["presupuesto"] = presupuesto;
            if (!string.IsNullOrWhiteSpace(razonSocial)) query["razonSocial"] = razonSocial;
            if (!string.IsNullOrWhiteSpace(linea)) query["linea"] = linea;
            if (!string.IsNullOrWhiteSpace(zona)) query["zona"] = zona;
            if (!string.IsNullOrWhiteSpace(codOrden)) query["codOrden"] = codOrden;
            if (idEstado != null) query["idEstado"] = idEstado;
            if (!string.IsNullOrWhiteSpace(codTango)) query["codTango"] = codTango;
            query["idUsuario"] = idUsuario;

            uri += $"?{string.Join("&", query.Select(x => $"{x.Key}={x.Value}").ToArray())}";

            if (cancellationToken == null)
                return await HttpClient.GetFromJsonAsync<List<Orden>>(uri);
            else
                return await HttpClient.GetFromJsonAsync<List<Orden>>(uri, cancellationToken.Token);
        }

        public async ValueTask<List<Orden>> GetOrdenesPendientes(DateTime desde, DateTime hasta, int idUsuario, CancellationTokenSource cancellationToken)
        {
            string uri = $"{URI_BASE}/listaconpendientes";
            Dictionary<string, object> query = new();
            query["desdeStr"] = WebUtility.UrlEncode(desde.ToString()!);
            query["hastaStr"] = WebUtility.UrlEncode(hasta.AddHours(23).AddMinutes(59).AddSeconds(59).ToString()!);
            query["idUsuario"] = idUsuario;

            uri += $"?{string.Join("&", query.Select(x => $"{x.Key}={x.Value}").ToArray())}";

            if (cancellationToken == null)
                return await HttpClient.GetFromJsonAsync<List<Orden>>(uri);
            else
                return await HttpClient.GetFromJsonAsync<List<Orden>>(uri, cancellationToken.Token);
        }

        public async ValueTask<Orden?> GetOrden(string codOrden, bool conDetalle = true)
        {
            string uri = $"{URI_BASE}?{string.Join("&", new Dictionary<string, object>
            {
                ["codOrden"] = codOrden,
                ["conDetalle"] = conDetalle
            }.Select(x => $"{x.Key}={x.Value}"))}";

            var httpResponse = await HttpClient.GetAsync(uri);

            if (!httpResponse.IsSuccessStatusCode)
                return null;

            if (httpResponse.Content.Headers.ContentLength == 0)
                return null;

            var response = await httpResponse.Content.ReadFromJsonAsync<Orden>();
            return response;
        }


        public async ValueTask<List<OrdenDetalle>> GetOrdenConPendienteDetaLLE(string codOrden)
        {
            string uri = $"{URI_BASE}/ordenconpendiente/{codOrden}";
            return await HttpClient.GetFromJsonAsync<List<OrdenDetalle>>(uri);
        }

        public async ValueTask<List<Transporte>> GetTransportes()
        {
            return await HttpClient.GetFromJsonAsync<List<Transporte>>($"{URI_BASE}/transportes");
        }
        public async Task<List<OrdenDashboard>> GetOrdenDashboard(int idUsuario)
        {
            return await HttpClient.GetFromJsonAsync<List<OrdenDashboard>>($"{URI_BASE}/ordenDashboard?idUsuario={idUsuario}");
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

        public async ValueTask<Response> PostPedido(Orden pedido)
        {
            try
            {
                if (pedido.Usuario == null)
                    pedido.Usuario = SesionDTO.Nombre;
                var response = await HttpClient.PostAsJsonAsync($"{URI_BASE}", pedido);

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    Orden p = await response.Content.ReadFromJsonAsync<Orden>();
                    pedido.CodigoOrden = p.CodigoOrden;
                    return new(response.StatusCode);
                }
                else if (response.StatusCode == HttpStatusCode.BadRequest)
                    return new(response.StatusCode, await response.Content.ReadFromJsonAsync<Dictionary<string, List<string>>>());
                else
                    throw new Exception(await response.Content.ReadAsStringAsync());
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "PostPedido");
                return new(HttpStatusCode.InternalServerError);
            }
        }

        public async ValueTask<Response> PutPedido(Orden pedido)
        {
            try
            {
                pedido.Usuario = SesionDTO.Nombre;
                var response = await HttpClient.PutAsJsonAsync($"{URI_BASE}", pedido);
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    Orden p = await response.Content.ReadFromJsonAsync<Orden>();
                    pedido.CodigoOrden = p.CodigoOrden;
                    return new(response.StatusCode);
                }
                else if (response.StatusCode == HttpStatusCode.BadRequest)
                    return new(response.StatusCode, await response.Content.ReadFromJsonAsync<Dictionary<string, List<string>>>());
                else
                    throw new Exception(await response.Content.ReadAsStringAsync());
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "PutPedido");
                return new(HttpStatusCode.InternalServerError);
            }
        }

        public async ValueTask<Response> PutPedidoAprobacion(Orden pedido)
        {
            try
            {
                pedido.Usuario = SesionDTO.Nombre;
                OrdenDTO aux = new(pedido);
                var response = await HttpClient.PutAsJsonAsync($"{URI_BASE}/aprobacion/{SesionDTO.IdUsuario}", aux);
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    OrdenDTO r = await response.Content.ReadFromJsonAsync<OrdenDTO>();
                    pedido.IdEstado = r.IdEstado;
                    pedido.DescripcionEstado = r.DescripcionEstado;
                    return new(response.StatusCode);
                }
                else if (response.StatusCode == HttpStatusCode.BadRequest)
                    return new(response.StatusCode, await response.Content.ReadFromJsonAsync<Dictionary<string, List<string>>>());
                else
                    throw new Exception(await response.Content.ReadAsStringAsync());
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "PutPedidoAprobacion");
                return new(HttpStatusCode.InternalServerError);
            }
        }

        public async ValueTask<Response> CambiarEstadoPedidos(OrdenDTO orden)
        {
            try
            {
                orden.EdicionUsuario = SesionDTO.Nombre;
                var response = await HttpClient.PutAsJsonAsync($"{URI_BASE}/estado", orden);
                if (response.StatusCode == HttpStatusCode.OK)
                    return new(response.StatusCode);
                else if (response.StatusCode == HttpStatusCode.BadRequest)
                    return new(response.StatusCode, await response.Content.ReadFromJsonAsync<Dictionary<string, List<string>>>());
                else
                    throw new Exception(await response.Content.ReadAsStringAsync());
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "CambioEstadoPedidos");
                return new(HttpStatusCode.InternalServerError);
            }
        }

        public async ValueTask<Response> RevertirOrden(string id)
        {
            try
            {
                var response = await HttpClient.PutAsJsonAsync($"{URI_BASE}/revertirorden/{id}/{SesionDTO.Nombre}", id);
                if (response.StatusCode == HttpStatusCode.OK)
                    return new(response.StatusCode);
                else if (response.StatusCode == HttpStatusCode.BadRequest)
                    return new(response.StatusCode, await response.Content.ReadFromJsonAsync<Dictionary<string, List<string>>>());
                else
                    throw new Exception(await response.Content.ReadAsStringAsync());
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "CambioEstadoPedidos");
                return new(HttpStatusCode.InternalServerError);
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

        public async ValueTask<OrdenExpedicion> GetOrdenExpedicionImprimir(string idOrden)
        {
            return await HttpClient.GetFromJsonAsync<OrdenExpedicion>($"{URI_BASE}/expedicionImprimir/{SesionDTO.Nombre}?idOrden={idOrden}");
        }

        public async ValueTask<Response> PostExpedicionDetalle(OrdenExpedicion orden)
        {
            try
            {
                var response = await HttpClient.PostAsJsonAsync($"{URI_BASE}/expediciondetalle", orden);
                if (response.StatusCode == HttpStatusCode.OK)
                    return new(response.StatusCode);
                else if (response.StatusCode == HttpStatusCode.BadRequest)
                    return new(response.StatusCode, await response.Content.ReadFromJsonAsync<Dictionary<string, List<string>>>());
                else
                    throw new Exception(await response.Content.ReadAsStringAsync());
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "PostExpedicionDetalle");
                return new(HttpStatusCode.InternalServerError);
            }
        }

        public async ValueTask<Response> DespacharOrdenes(List<OrdenExpedicion> ordenes)
        {
            try
            {
                var response = await HttpClient.PostAsJsonAsync($"{URI_BASE}/despachar/{SesionDTO.Nombre}", ordenes);
                if (response.StatusCode == HttpStatusCode.OK)
                    return new(response.StatusCode);
                else if (response.StatusCode == HttpStatusCode.BadRequest)
                    return new(response.StatusCode, await response.Content.ReadFromJsonAsync<Dictionary<string, List<string>>>());
                else
                    throw new Exception(await response.Content.ReadAsStringAsync());
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "DespacharOrden");
                return new(HttpStatusCode.InternalServerError);
            }
        }

        public async Task<CantidadesProductosDashboard> GetCantidadesDeProductos(int idUsuario)
        {
            return await HttpClient.GetFromJsonAsync<CantidadesProductosDashboard>($"{URI_BASE}/cantidadesproductos?idUsuario={idUsuario}");
        }

        public async Task<Indicadores> GetIndicadores(int idUsuario)
        {
            return await HttpClient.GetFromJsonAsync<Indicadores>($"{URI_BASE}/indicadores?idUsuario={idUsuario}");
        }

        public async ValueTask<Response> PasarATango(Orden order)
        {
            try
            {
                order.Usuario = SesionDTO.Nombre;
                OrdenDTO aux = new(order);
                var cts = new CancellationTokenSource(TimeSpan.FromMinutes(4)); // timeout de 2 minutos (120s)
                var response = await HttpClient.PutAsJsonAsync($"{URI_BASE}/tango", aux, cts.Token);
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    OrdenDTO r = await response.Content.ReadFromJsonAsync<OrdenDTO>();
                    order.IdEstado = r.IdEstado;
                    order.DescripcionEstado = r.DescripcionEstado;
                    return new(response.StatusCode);
                }
                else if (response.StatusCode == HttpStatusCode.BadRequest)
                    return new(response.StatusCode, await response.Content.ReadAsStringAsync());
                else
                    throw new Exception(await response.Content.ReadAsStringAsync());

            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Pasar a tango");
                return new(HttpStatusCode.InternalServerError);
            }
        }

        public async ValueTask<Response> PasarATangoObsequios(Orden order)
        {
            try
            {
                order.Usuario = SesionDTO.Nombre;
                OrdenDTO aux = new(order);
                var cts = new CancellationTokenSource(TimeSpan.FromMinutes(4)); // timeout de 2 minutos (120s)
                var response = await HttpClient.PutAsJsonAsync($"{URI_BASE}/tango/obsequios", aux, cts.Token);
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    return new(response.StatusCode);
                }
                else if (response.StatusCode == HttpStatusCode.BadRequest)
                    return new(response.StatusCode, await response.Content.ReadAsStringAsync());
                else
                    throw new Exception(await response.Content.ReadAsStringAsync());

            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Pasar a tango");
                return new(HttpStatusCode.InternalServerError);
            }
        }

        public async ValueTask<Response> PasarATangoProbadores(Orden order)
        {
            try
            {
                order.Usuario = SesionDTO.Nombre;
                OrdenDTO aux = new(order);
                var cts = new CancellationTokenSource(TimeSpan.FromMinutes(4)); // timeout de 2 minutos (120s)
                var response = await HttpClient.PutAsJsonAsync($"{URI_BASE}/tango/probadores", aux, cts.Token);
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    return new(response.StatusCode);
                }
                else if (response.StatusCode == HttpStatusCode.BadRequest)
                    return new(response.StatusCode, await response.Content.ReadAsStringAsync());
                else
                    throw new Exception(await response.Content.ReadAsStringAsync());

            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Pasar a tango");
                return new(HttpStatusCode.InternalServerError);
            }
        }

        public async ValueTask<Response> CambiarListaPrecio(Orden orden)
        {
            try
            {
                OrdenDTO aux = new(orden)
                {
                    Usuario = SesionDTO.Nombre
                };
                var response = await HttpClient.PutAsJsonAsync($"{URI_BASE}/lista", aux);
                if (response.StatusCode == HttpStatusCode.OK)
                    return new(response.StatusCode);
                else if (response.StatusCode == HttpStatusCode.BadRequest)
                    return new(response.StatusCode, await response.Content.ReadAsStringAsync());
                else
                    throw new Exception(await response.Content.ReadAsStringAsync());
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Cambiar lista de precio");
                return new(HttpStatusCode.InternalServerError);
            }
        }

        public async ValueTask<Response> RevertirEstadoOrden(Orden order)
        {
            try
            {
                OrdenDTO orden = new(order)
                {
                    Usuario = SesionDTO.Nombre
                };
                var response = await HttpClient.PutAsJsonAsync($"{URI_BASE}/revertirestado", orden);
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    OrdenDTO r = await response.Content.ReadFromJsonAsync<OrdenDTO>();
                    order.IdEstado = r.IdEstado;
                    order.DescripcionEstado = r.DescripcionEstado;
                    return new(response.StatusCode);
                }
                else if (response.StatusCode == HttpStatusCode.BadRequest)
                    return new(response.StatusCode, await response.Content.ReadFromJsonAsync<Dictionary<string, List<string>>>());
                else
                    throw new Exception(await response.Content.ReadAsStringAsync());
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "RevertirEstadoOrden");
                return new(HttpStatusCode.InternalServerError);
            }
        }

        public async ValueTask<ComprobanteCabecera> GetNotaCredito(int idComprobante)
        {
            var aux = await GetNotasCredito(new() { IdComprobante = idComprobante, ConDetalle = true });

            if (aux != null && aux.Count > 0)
                return aux.First();

            return null;
        }

        public async ValueTask<List<ComprobanteCabecera>> GetNotasCredito(ComprobanteCabeceraRequest request)
        {
            var response = await HttpClient.PostAsJsonAsync($"{URI_BASE}/notacredito/obtener", request);

            if (response.StatusCode == HttpStatusCode.OK)
                return await response.Content.ReadFromJsonAsync<List<ComprobanteCabecera>>();

            return null;
        }

        public async ValueTask<Response> CancelarNotaCredito(ComprobanteCabecera comprobante)
        {
            var response = await HttpClient.PutAsJsonAsync($"{URI_BASE}/notacredito/cancelar", comprobante);
            if (response.StatusCode == HttpStatusCode.OK)
                return new(response.StatusCode, await response.Content.ReadFromJsonAsync<ComprobanteCabecera>());

            return await Response.CreateAsync(response);
        }

        public async ValueTask<Response> PostNotaCredito(ComprobanteCabecera comprobante)
        {
            var response = await HttpClient.PostAsJsonAsync($"{URI_BASE}/notacredito", comprobante);
            if (response.StatusCode == HttpStatusCode.OK)
                return new(response.StatusCode, await response.Content.ReadFromJsonAsync<ComprobanteCabecera>());

            return await Response.CreateAsync(response);
        }

        public async ValueTask<Response> PutNotaCredito(ComprobanteCabecera comprobante) =>
            await Response.CreateAsync(await HttpClient.PutAsJsonAsync($"{URI_BASE}/notacredito", comprobante));
    }
}
