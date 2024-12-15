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

        public async ValueTask<List<Producto>> GetProductos(int? idProducto = null, int? codigoInterno = null,
            string? ean = null, string? descripcion = null, string? razonSocialProveedor = null, string? idFamilia = null, string? descripcionFamilia = null, string? descripcionMarca = null,
            string? codSegunProveedor = null, int? idProveedor = null, int? idSitioWeb = null, bool? pasivo = null, int? cantRecs = null, bool? conSku = false,
            CancellationTokenSource cancellationToken = null)
        {
            Dictionary<string, object> query = new();
            if (idProducto != null && idProducto != 0) query["idProducto"] = idProducto;
            if (codigoInterno != null && codigoInterno != 0) query["codigoInterno"] = codigoInterno;
            if (ean != null) query["ean"] = WebUtility.UrlEncode(ean.Trim());
            if (descripcion != null) query["descripcion"] = WebUtility.UrlEncode(descripcion.Trim());
            if (razonSocialProveedor != null) query["razonSocialProveedor"] = WebUtility.UrlEncode(razonSocialProveedor.Trim());
            if (idFamilia != null) query["idFamilia"] = idFamilia.Trim();
            if (descripcionFamilia != null) query["descripcionFamilia"] = WebUtility.UrlEncode(descripcionFamilia.Trim());
            if (descripcionMarca != null) query["descripcionMarca"] = WebUtility.UrlEncode(descripcionMarca.Trim());
            if (codSegunProveedor != null) query["codSegunProveedor"] = WebUtility.UrlEncode(codSegunProveedor.Trim());
            if (idProveedor != null) query["idProveedor"] = idProveedor;
            if (idSitioWeb != null) query["idSitioWeb"] = idSitioWeb;
            if (pasivo != null) query["pasivo"] = pasivo;
            if (cantRecs != null && cantRecs != 0) query["cantRecs"] = cantRecs;
            if (conSku != null) query["conSku"] = conSku;
            var stringJoin = string.Join("&", query.Select(x => $"{x.Key}={x.Value}").ToArray());

            if (cancellationToken == null)
            {
                return await HttpClient.GetFromJsonAsync<List<Producto>>($"{REQUEST_URI_BASE}/dto?{stringJoin}");
            }
            else
            {
                return await HttpClient.GetFromJsonAsync<List<Producto>>($"{REQUEST_URI_BASE}/dto?{stringJoin}", cancellationToken.Token);
            }
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
                    producto.CodigoInterno = prod.CodigoInterno;
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
                producto.CodigoInterno = prod.CodigoInterno;
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