using Blazored.LocalStorage;
using System.Text.Json;

namespace GAPPLE.Client.Services
{
    public class BackupService(ILocalStorageService localStorage)
    {
        public async Task GuardarObjeto(string nombrePagina, string idUsuario, object objeto)
        {
            await localStorage.RemoveItemAsync($"{nombrePagina}|{idUsuario}");
            await localStorage.SetItemAsync($"{nombrePagina}|{idUsuario}", JsonSerializer.Serialize(objeto));
        }

        public async Task<string> CargarObjeto(string nombrePagina, string idUsuario)
        {
            var response = await localStorage.GetItemAsync<string>($"{nombrePagina}|{idUsuario}");
            return response;
        }

        public async Task Delete(string nombrePagina, string idUsuario)
        {
            await localStorage.RemoveItemAsync($"{nombrePagina}|{idUsuario}");
        }
    }
}
