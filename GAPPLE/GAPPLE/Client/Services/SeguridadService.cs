using Microsoft.AspNetCore.Components;
using System.Net;

namespace GAPPLE.Client.Services
{
    public class SeguridadService
    {
        [Inject]
        private HttpClient HttpClient { get; set; }
        private NavigationManager NavigationManager { get; }
        private const string URI_BASE = "api/clientes";

        public SeguridadService(HttpClient httpClient, NavigationManager navigationManager)
        {
            HttpClient = httpClient;
            NavigationManager = navigationManager;
        }
        
        //internal async ValueTask ValidatePageAccess()
        //{
        //    string href = NavigationManager.ToBaseRelativePath(NavigationManager.Uri);

        //    if (href.Contains('?'))
        //    {
        //        href = href.Remove(href.IndexOf('?'));
        //    }

        //    var response = await HttpClient.GetAsync($"{URI_BASE}/validaracceso?href={WebUtility.UrlEncode(href.ToLower())}");
        //    bool? res = await response.Content.ReadAsStringAsync() == "" ? null : bool.Parse(await response.Content.ReadAsStringAsync());

        //    if (res == false)
        //        NavigationManager.NavigateTo(Tools.Variables.ErrorPages.Desautorizado);
        //    else if (res == null)
        //        NavigationManager.NavigateTo(Tools.Variables.ErrorPages.UsuarioNoEncontrado);
        //}
    }
}
