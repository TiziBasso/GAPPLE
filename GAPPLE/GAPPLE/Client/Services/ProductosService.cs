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

        public async ValueTask<List<Producto>> GetProductos(string? codigoProducto, string? descripcion, bool? clasificado, bool? pasivo, string? linea,
            CancellationTokenSource? cancellationToken = null)
        {
            Dictionary<string, object> query = new();
            if (!string.IsNullOrWhiteSpace(codigoProducto)) query["codigoProducto"] = codigoProducto;
            if (descripcion != null) query["descripcion"] = WebUtility.UrlEncode(descripcion.Trim());
            if (clasificado != null) query["clasificado"] = clasificado;
            if (pasivo != null) query["pasivo"] = pasivo;
            if (linea != null) query["linea"] = linea;
            var stringJoin = string.Join("&", query.Select(x => $"{x.Key}={x.Value}").ToArray());

            if (cancellationToken == null)
                return await HttpClient.GetFromJsonAsync<List<Producto>>($"{REQUEST_URI_BASE}?{stringJoin}");
            else
                return await HttpClient.GetFromJsonAsync<List<Producto>>($"{REQUEST_URI_BASE}?{stringJoin}", cancellationToken.Token);
        }

        public async ValueTask<ProductoOrden> GetProductoOrden(string codigoProducto, string codListaPrecio)
        {
            Dictionary<string, object> query = new();
            query["codProducto"] = codigoProducto;
            query["codListaPrecio"] = codListaPrecio;

            var stringJoin = string.Join("&", query.Select(x => $"{x.Key}={x.Value}").ToArray());

            var response = await HttpClient.GetAsync($"{REQUEST_URI_BASE}/orden?{stringJoin}");
            if (response.StatusCode == HttpStatusCode.OK)
                return await response.Content.ReadFromJsonAsync<ProductoOrden>();
            else
                return null;
        }

        public async ValueTask<List<ProductoOrden>> GetProductosOrden(string codigoProducto, string descripcion, string linea, string codListaPrecio,
                                                                        bool? pasivo = null,
                                                                        CancellationTokenSource cancellationToken = null)
        {
            Dictionary<string, object> query = new();
            if (!string.IsNullOrEmpty(codigoProducto)) query["codProducto"] = WebUtility.UrlEncode($"%{codigoProducto.Trim()}%");
            if (descripcion != null) query["descripcion"] = WebUtility.UrlEncode($"%{descripcion.Trim()}%");
            if (linea != null) query["linea"] = linea;
            query["codListaPrecio"] = codListaPrecio;
            if (pasivo != null) query["pasivo"] = pasivo;
            var stringJoin = string.Join("&", query.Select(x => $"{x.Key}={x.Value}").ToArray());

            var response = cancellationToken == null
                            ? await HttpClient.GetAsync($"{REQUEST_URI_BASE}/orden/varios?{stringJoin}")
                            : await HttpClient.GetAsync($"{REQUEST_URI_BASE}/orden/varios?{stringJoin}", cancellationToken.Token);

            if (response.StatusCode == HttpStatusCode.OK)
                return await response.Content.ReadFromJsonAsync<List<ProductoOrden>>();
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
                    return new(response.StatusCode);
                }
                else
                {
                    return response.StatusCode == HttpStatusCode.BadRequest
                        ? new(response.StatusCode, await response.Content.ReadFromJsonAsync<Dictionary<string, List<string>>>()!)
                        : new(response.StatusCode, "Ha ocurrido un error inesperado!");
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "PostProducto");
                return new(HttpStatusCode.InternalServerError);
            }
        }

        public async ValueTask<Response> PutProducto(Producto producto)
        {
            var response = await HttpClient.PutAsJsonAsync($"{REQUEST_URI_BASE}", producto);
            return new(response.StatusCode);
            //if (response.StatusCode == HttpStatusCode.OK)
            //{
            //    var prod = await response.Content.ReadFromJsonAsync<Producto>();
            //    producto.IdProducto = prod!.IdProducto;
            //    producto.CodigoProducto = prod.CodigoProducto;
            //    return new(true);
            //}
            //else
            //{
            //    return response.StatusCode == HttpStatusCode.BadRequest
            //        ? new(false, await response.Content.ReadFromJsonAsync<Dictionary<string, List<string>>>()!)
            //        : new(false, "Ha ocurrido un error inesperado!");
            //}
        }

        public async ValueTask<List<string>> GetLineas()
        {
            Dictionary<string, object> query = new();
            var stringJoin = string.Join("&", query.Select(x => $"{x.Key}={x.Value}").ToArray());

            var response = await HttpClient.GetAsync($"{REQUEST_URI_BASE}/lineas");
            if (response.StatusCode == HttpStatusCode.OK)
                return await response.Content.ReadFromJsonAsync<List<string>>()!;
            else
                return null;
        }

        public async ValueTask<List<ProductoParaOfertas>> GetProductosParaOfertas(string linea, string codListaPrecio = null)
        {
            var response = await HttpClient.GetAsync($"{REQUEST_URI_BASE}/productosparaofertas?linea={linea}&codListaPrecio={codListaPrecio}");
            if (response.StatusCode == HttpStatusCode.OK)
                return await response.Content.ReadFromJsonAsync<List<ProductoParaOfertas>>()!;
            else
                return null;
        }

        public async ValueTask<List<ProductosComplementos>> GetProductosComplementos()
        {
            var response = await HttpClient.GetAsync($"{REQUEST_URI_BASE}/complementos");
            if (response.StatusCode == HttpStatusCode.OK)
                return await response.Content.ReadFromJsonAsync<List<ProductosComplementos>>();
            else
                return null;
        }

        public async ValueTask<bool> InsertProductosComplementos(List<ProductosComplementos> productosComplementos)
        {
            var response = await HttpClient.PostAsJsonAsync($"{REQUEST_URI_BASE}/complementos/insert", productosComplementos);
            if (response.StatusCode == HttpStatusCode.OK)
                return true;
            else
                return false;
        }

        public async ValueTask<bool> DeleteProductosComplementos(List<ProductosComplementos> productosComplementos)
        {
            var response = await HttpClient.PostAsJsonAsync($"{REQUEST_URI_BASE}/complementos/delete", productosComplementos);
            if (response.StatusCode == HttpStatusCode.OK)
                return true;
            else
                return false;
        }

        public async ValueTask<Response> ProcesarArchivo(ByteArrayRequest req)
        {
            try
            {
                var response = await HttpClient.PostAsJsonAsync($"{REQUEST_URI_BASE}/procesar", req);
                if (response.StatusCode == HttpStatusCode.OK)
                    return new(response.StatusCode, await response.Content.ReadFromJsonAsync<List<ProductoArchivo>>());
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
    }
}