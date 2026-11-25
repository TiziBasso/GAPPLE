using GAPPLE.Client.Entities;
using GAPPLE.Shared.Model;
using GAPPLE.Shared.Requests;
using System.Net;
using System.Net.Http.Json;

namespace GAPPLE.Client.Services
{
    /// <summary>
    /// Listas de precios cargadas desde Excel y su asignacion por cliente y vendedor.
    /// </summary>
    public class ListasPrecioService
    {
        private HttpClient HttpClient { get; }
        private SesionDTO SesionDTO { get; }
        private const string URI_BASE = "api/listasprecio";

        public ListasPrecioService(HttpClient httpClient, SesionDTO sesionDTO)
        {
            HttpClient = httpClient;
            SesionDTO = sesionDTO;
        }

        #region Listas
        public async ValueTask<List<ListaPrecioImportada>> GetListas()
        {
            return await HttpClient.GetFromJsonAsync<List<ListaPrecioImportada>>(URI_BASE) ?? new();
        }

        /// <summary>Lee el Excel en el server sin guardar. Data: <see cref="ListaPrecioPrevisualizacion"/>.</summary>
        public async ValueTask<Response> Previsualizar(string nombreArchivo, byte[] archivo)
        {
            var response = await HttpClient.PostAsJsonAsync($"{URI_BASE}/previsualizar",
                new ListaPrecioArchivoRequest { NombreArchivo = nombreArchivo, Archivo = archivo });

            if (response.StatusCode == HttpStatusCode.OK)
                return new(response.StatusCode, (object)await response.Content.ReadFromJsonAsync<ListaPrecioPrevisualizacion>());

            return await Response.CreateAsync(response);
        }

        /// <summary>Crea la lista o, con IdListaPrecio, reemplaza sus precios. Data: id de la lista.</summary>
        public async ValueTask<Response> Guardar(ListaPrecioArchivoRequest request)
        {
            request.Usuario = SesionDTO.Nombre;
            var response = await HttpClient.PostAsJsonAsync(URI_BASE, request);

            if (response.StatusCode == HttpStatusCode.OK)
                return new(response.StatusCode, (object)await response.Content.ReadFromJsonAsync<int>());

            return await Response.CreateAsync(response);
        }

        public async ValueTask<bool> Eliminar(int idListaPrecio)
        {
            var response = await HttpClient.DeleteAsync($"{URI_BASE}/{idListaPrecio}");
            return response.IsSuccessStatusCode;
        }

        /// <summary>Precios de la lista, o null si el usuario no la puede ver.</summary>
        public async ValueTask<List<ListaPrecioImportadaItem>> GetItems(int idListaPrecio)
        {
            var response = await HttpClient.GetAsync($"{URI_BASE}/{idListaPrecio}/items?idUsuario={SesionDTO.IdUsuario}");
            if (response.StatusCode == HttpStatusCode.OK)
                return await response.Content.ReadFromJsonAsync<List<ListaPrecioImportadaItem>>() ?? new();

            return null;
        }

        /// <summary>Excel original de la lista. DataBytes: el archivo; Message: su nombre.</summary>
        public async ValueTask<Response> GetArchivo(int idListaPrecio)
        {
            var response = await HttpClient.GetAsync($"{URI_BASE}/{idListaPrecio}/archivo?idUsuario={SesionDTO.IdUsuario}");
            if (response.StatusCode != HttpStatusCode.OK)
                return new(response.StatusCode);

            var disposition = response.Content.Headers.ContentDisposition;
            return new(response.StatusCode, await response.Content.ReadAsByteArrayAsync())
            {
                Message = (disposition?.FileNameStar ?? disposition?.FileName)?.Trim('"')
            };
        }
        #endregion

        #region Asignacion por cliente y vendedor
        public async ValueTask<List<ListaPrecioClienteVendedor>> GetPorCliente(int idCliente)
        {
            return await HttpClient.GetFromJsonAsync<List<ListaPrecioClienteVendedor>>($"{URI_BASE}/cliente?idCliente={idCliente}") ?? new();
        }

        public async ValueTask<bool> PostPorCliente(int idCliente, List<ListasPrecioVendedor> vendedores)
        {
            var request = new ListasPrecioClienteRequest { IdCliente = idCliente, Usuario = SesionDTO.Nombre, Vendedores = vendedores };
            var response = await HttpClient.PostAsJsonAsync($"{URI_BASE}/cliente", request);
            return response.IsSuccessStatusCode;
        }

        /// <summary>Listas que puede ver el usuario logueado (las de sus clientes, o todas si es administrador).</summary>
        public async ValueTask<List<ListaPrecioClienteVendedor>> GetMisListas()
        {
            return await HttpClient.GetFromJsonAsync<List<ListaPrecioClienteVendedor>>($"{URI_BASE}/vendedor?idUsuario={SesionDTO.IdUsuario}") ?? new();
        }
        #endregion
    }
}
