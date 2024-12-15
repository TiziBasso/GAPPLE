using GAPPLE.Client.Entities;
using GAPPLE.Shared.Model;
using Microsoft.AspNetCore.Components;
using System.Net;
using System.Net.Http.Json;

namespace GAPPLE.Client.Services
{
    public class ProductosService
    {
        [Inject]
        private HttpClient HttpClient { get; }
        private ILogger<ProductosService> Logger { get; }
        private const string REQUEST_URI_BASE = "api/productos";

        public ProductosService(HttpClient httpClient, ILogger<ProductosService> logger) => (HttpClient, Logger) = (httpClient, logger);

        public async ValueTask<List<Producto>> GetProductos(string? codigoProducto, string? descripcion, bool? clasificado,
            CancellationTokenSource? cancellationToken = null)
        {
            Dictionary<string, object> query = new();
            if (!string.IsNullOrWhiteSpace(codigoProducto)) query["codigoProducto"] = codigoProducto;
            if (descripcion != null) query["descripcion"] = WebUtility.UrlEncode(descripcion.Trim());
            if (clasificado != null) query["clasificado"] = clasificado;
            var stringJoin = string.Join("&", query.Select(x => $"{x.Key}={x.Value}").ToArray());

            if (cancellationToken == null)
                return await HttpClient.GetFromJsonAsync<List<Producto>>($"{REQUEST_URI_BASE}?{stringJoin}");
            else
                return await HttpClient.GetFromJsonAsync<List<Producto>>($"{REQUEST_URI_BASE}?{stringJoin}", cancellationToken.Token);
        }

        public async ValueTask<Producto> GetProducto(int? codigoInterno, int? idProducto = null)
        {
            Dictionary<string, object> query = new();
            if (idProducto != null) query["idProducto"] = idProducto;
            if (codigoInterno != null) query["codigoInterno"] = codigoInterno;

            var stringJoin = string.Join("&", query.Select(x => $"{x.Key}={x.Value}").ToArray());

            var response = await HttpClient.GetAsync($"{REQUEST_URI_BASE}/prod?{stringJoin}");
            if (response.StatusCode == HttpStatusCode.OK)
                return await response.Content.ReadFromJsonAsync<Producto>();
            else
                return null;
        }

        public async ValueTask<Response> PostProducto(Producto producto)
        {
            try
            {
                var response = await HttpClient.PostAsJsonAsync($"{REQUEST_URI_BASE}", producto);
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var prod = await response.Content.ReadFromJsonAsync<Producto>();
                    producto.IdProducto = prod!.IdProducto;
                    producto.CodigoProducto = prod.CodigoProducto;
                    return new(true);
                }
                else
                {
                    return response.StatusCode == HttpStatusCode.BadRequest
                        ? new(false, await response.Content.ReadFromJsonAsync<Dictionary<string, List<string>>>())
                        : new(false, "Ha ocurrido un error inesperado!");
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "PostProducto");
                return new(false);
            }
        }

        public async ValueTask<Response> PutProducto(Producto producto)
        {
            var response = await HttpClient.PutAsJsonAsync($"{REQUEST_URI_BASE}", producto);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var prod = await response.Content.ReadFromJsonAsync<Producto>();
                producto.IdProducto = prod!.IdProducto;
                producto.CodigoProducto = prod.CodigoProducto;
                return new(true);
            }
            else
            {
                return response.StatusCode == HttpStatusCode.BadRequest
                    ? new(false, await response.Content.ReadFromJsonAsync<Dictionary<string, List<string>>>())
                    : new(false, "Ha ocurrido un error inesperado!");
            }
        }
    }
}