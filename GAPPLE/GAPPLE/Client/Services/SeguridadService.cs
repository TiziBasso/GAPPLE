using GAPPLE.Client.Entities;
using GAPPLE.Shared.Model;
using Microsoft.AspNetCore.Components;
using System.Net;
using System.Net.Http.Json;
using Menu = GAPPLE.Shared.Model.Menu;

namespace GAPPLE.Client.Services
{
    internal class SeguridadService
    {
        [Inject]
        private HttpClient HttpClient { get; }
        [Inject]
        private SesionDTO SesionDTO { get; }
        private NavigationManager NavigationManager { get; }
        private const string URI_BASE = "api/seguridad";

        public SeguridadService(HttpClient httpClient, NavigationManager navigationManager, SesionDTO sesionDTO)
        {
            HttpClient = httpClient;
            NavigationManager = navigationManager;
            SesionDTO = sesionDTO;
        }

        public async ValueTask<Usuario> GetUsuario(int? idUsuario)
        {
            var response = await HttpClient.GetAsync($"{URI_BASE}/usuario/{idUsuario}");
            if (response.StatusCode == HttpStatusCode.OK)
                return await response.Content.ReadFromJsonAsync<Usuario>();
            else
                return null;
        }

        internal async ValueTask ValidatePageAccess(int? idUsuario = null)
        {
            if (idUsuario == null) idUsuario = SesionDTO.IdUsuario;

            string href = NavigationManager.ToBaseRelativePath(NavigationManager.Uri);

            if (href.Contains('?'))
            {
                href = href.Remove(href.IndexOf('?'));
            }

            var response = await HttpClient.GetAsync($"{URI_BASE}/validaracceso?href={WebUtility.UrlEncode(href.ToLower())}&idUsuario={idUsuario}");
            bool? res = await response.Content.ReadAsStringAsync() == "" ? null : bool.Parse(await response.Content.ReadAsStringAsync());

            if (res == false)
                NavigationManager.NavigateTo(Tools.Variables.ErrorPages.Desautorizado);
            else if (res == null)
                NavigationManager.NavigateTo(Tools.Variables.ErrorPages.UsuarioNoEncontrado);
        }

        internal async ValueTask<List<Menu>> GetPermisos(int? idUsuario)
        {
            return await HttpClient.GetFromJsonAsync<List<Menu>>($"{URI_BASE}/permisos");
        }

        internal async Task<List<string>> GetPermisos(string nombrePermiso, char tipoPermiso = 'P', int? idUsuario = null)
        {
            string uri = $"{URI_BASE}/permisos/componente";
            Dictionary<string, object> query = new();
            query["nombre"] = nombrePermiso;
            query["tipoPermiso"] = tipoPermiso;
            if (idUsuario == null) idUsuario = SesionDTO.IdUsuario;
            query["idUsuario"] = idUsuario;

            uri += $"?{string.Join("&", query.Select(x => $"{x.Key}={x.Value}").ToArray())}";

            return await HttpClient.GetFromJsonAsync<List<string>>(uri);
        }

        internal async ValueTask<List<string>> ValidatePageAccess(string nombrePermiso, int? idUsuario = null)
        {
            if (idUsuario == null) idUsuario = SesionDTO.IdUsuario;
            var permisos = await GetPermisos(nombrePermiso, idUsuario: idUsuario);

            if (!permisos.Any())
                NavigationManager.NavigateTo(Tools.Variables.ErrorPages.Desautorizado);

            return permisos;
        }

        internal async ValueTask<Response> PostPermisosPorUsuario(List<Permiso> lstCambios)
        {
            var response = await HttpClient.PostAsJsonAsync($"{URI_BASE}/permisosUsuario", lstCambios);
            if (response.IsSuccessStatusCode)
                return new(response.IsSuccessStatusCode);
            else if (response.StatusCode == HttpStatusCode.BadRequest)
                return new(false, await response.Content.ReadFromJsonAsync<Dictionary<string, List<string>>>());
            else
                return new(false, null, await response.Content.ReadAsStringAsync());
        }

        public async ValueTask<List<Usuario>> GetUsuarios(string nombreDeUsuario = null, string apellidoYNombre = null, string perfil = null, bool? pasivo = null, bool conDetalle = true)
        {
            string uri = $"{URI_BASE}/usuarios";
            Dictionary<string, object> query = new();
            if (!string.IsNullOrWhiteSpace(nombreDeUsuario)) query["nombreDeUsuario"] = nombreDeUsuario;
            if (apellidoYNombre != null) query["apellidoYNombre"] = apellidoYNombre;
            if (perfil != null) query["perfil"] = perfil;
            if (pasivo != null) query["pasivo"] = pasivo;
            query["conDetalle"] = conDetalle;

            if (query.Any())
            {
                uri += $"?{string.Join("&", query.Select(x => $"{x.Key}={x.Value}").ToArray())}";
            }

            return await HttpClient.GetFromJsonAsync<List<Usuario>>(uri);
        }

        public async ValueTask<List<PerfilUsuario>> GetUsuariosPerfiles(int? idPerfil, string? descripcion)
        {
            string uri = $"{URI_BASE}";
            Dictionary<string, object> query = new();
            if (idPerfil != null) query["idPerfil"] = idPerfil;
            if (descripcion != null) query["descripcion"] = WebUtility.UrlEncode(descripcion.Trim());

            if (query.Any())
                uri += $"?{string.Join("&", query.Select(x => $"{x.Key}={x.Value}").ToArray())}";

            return await HttpClient.GetFromJsonAsync<List<PerfilUsuario>>($"{uri}/usuarios/perfiles");
        }


        public async ValueTask<Response> PostUsuario(Usuario usuario)
        {
            var response = await HttpClient.PostAsJsonAsync($"{URI_BASE}/usuario", usuario);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                return new(true);
            }
            else if (response.StatusCode == HttpStatusCode.BadRequest)
                return new(false, await response.Content.ReadFromJsonAsync<Dictionary<string, List<string>>>());
            else
                return new(false, "Ha ocurrido un error inesperado! Por favor contacte a sistemas!");
        }

        public async ValueTask<Response> PutUsuario(Usuario usuario)
        {
            var response = await HttpClient.PutAsJsonAsync($"{URI_BASE}/usuario", usuario);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                return new(true);
            }
            else if (response.StatusCode == HttpStatusCode.BadRequest)
                return new(false, await response.Content.ReadFromJsonAsync<Dictionary<string, List<string>>>());
            else
                return new(false, "Ha ocurrido un error inesperado! Por favor contacte a sistemas!");
        }

        internal async ValueTask<Menu> PostPermiso(Menu permiso)
        {
            var response = await HttpClient.PostAsJsonAsync($"{URI_BASE}/permiso", permiso);
            if (response.StatusCode == HttpStatusCode.OK)
                return await response.Content.ReadFromJsonAsync<Menu>();
            else
            {
                return null;
            }
        }

        internal async ValueTask<bool> PutPermiso(Menu permiso)
        {
            var response = await HttpClient.PutAsJsonAsync($"{URI_BASE}/permiso", permiso);
            if (response.StatusCode == HttpStatusCode.OK)
                return true;
            else
            {
                return false;
            }
        }

        internal async ValueTask<bool> DeletePermiso(int idPermiso)
        {
            var response = await HttpClient.DeleteAsync($"{URI_BASE}/permiso/{idPermiso}");
            if (response.StatusCode == HttpStatusCode.OK)
                return true;
            else
            {
                return false;
            }
        }

        internal async ValueTask<List<PerfilUsuario>> GetPerfiles(int? idPerfil = null, string? descripcion = null)
        {
            Dictionary<string, object> query = new();
            if (idPerfil != null) query["idPerfil"] = idPerfil;
            if (descripcion != null) query["descripcion"] = descripcion;
            var stringJoin = "?" + string.Join("&", query.Select(x => $"{x.Key}={x.Value}").ToArray());
            return await HttpClient.GetFromJsonAsync<List<PerfilUsuario>>($"{URI_BASE}/perfiles{stringJoin}");
        }

        internal async ValueTask<List<Permiso>> GetPermisosTotal(int? idUsuario, int? idPerfil)
        {
            Dictionary<string, object> query = new();
            if (idUsuario != null) query["idUsuario"] = idUsuario.ToString();
            if (idPerfil != null) query["idPerfil"] = idPerfil.ToString();
            var stringJoin = string.Join("&", query.Select(x => $"{x.Key}={x.Value}").ToArray());
            return await HttpClient.GetFromJsonAsync<List<Permiso>>($"{URI_BASE}/totalpermisos?{stringJoin}");
        }

        public async ValueTask<List<string>> GetUsuariosPorPerfil(int idPerfil)
        {
            return await HttpClient.GetFromJsonAsync<List<string>>($"{URI_BASE}/usuariosporperfil?idPerfil={idPerfil}");
        }

        internal async ValueTask<Response> PostPermisosPorPerfil(List<Permiso> lstCambios)
        {
            var response = await HttpClient.PostAsJsonAsync($"{URI_BASE}/permisosPerfil", lstCambios);
            if (response.IsSuccessStatusCode)
                return new(response.IsSuccessStatusCode);
            else if (response.StatusCode == HttpStatusCode.BadRequest)
                return new(false, await response.Content.ReadFromJsonAsync<Dictionary<string, List<string>>>());
            else
                return new(false, null, await response.Content.ReadAsStringAsync());
        }

        internal async ValueTask<List<MenuNew>> GetMenu(int idUsuario)
        {
            var response = await HttpClient.GetAsync($"{URI_BASE}/menu?idUsuario={idUsuario}");
            return response.StatusCode == HttpStatusCode.OK ? await response.Content.ReadFromJsonAsync<List<MenuNew>>() : null;
        }

        internal async ValueTask<Response> PostPerfil(PerfilUsuario perfilUsuario)
        {
            var response = await HttpClient.PostAsJsonAsync($"{URI_BASE}/Perfilusuario", perfilUsuario);
            if (response.IsSuccessStatusCode)
                return new(true, await response.Content.ReadAsStringAsync());
            else if (response.StatusCode == HttpStatusCode.BadRequest)
                return new(false, await response.Content.ReadFromJsonAsync<Dictionary<string, List<string>>>());
            else
                return new(false, null, await response.Content.ReadAsStringAsync());
        }

        internal async ValueTask<Response> PutPerfil(PerfilUsuario perfilUsuario)
        {
            var response = await HttpClient.PutAsJsonAsync($"{URI_BASE}/Perfilusuario", perfilUsuario);
            if (response.IsSuccessStatusCode)
                return new(response.IsSuccessStatusCode);
            else if (response.StatusCode == HttpStatusCode.BadRequest)
                return new(false, await response.Content.ReadFromJsonAsync<Dictionary<string, List<string>>>());
            else
                return new(false, null, await response.Content.ReadAsStringAsync());
        }

        public async ValueTask<List<Vendedor>> GetVendedores()
        {
            return await HttpClient.GetFromJsonAsync<List<Vendedor>>($"{URI_BASE}/vendedores");
        }
    }
}
