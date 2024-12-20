using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Text;
using System.Text.Json;

namespace GAPPLE.Client.Tools
{
    public class JSFunction : IJSFunction
    {
        [Inject]
        private IJSRuntime JS { get; set; }
        private IJSInProcessObjectReference mod;

        public JSFunction(IJSRuntime js) => JS = js;

        public async ValueTask FocusInput(string id)
        {
            await JS.InvokeVoidAsync("focusInput", id);
        }

        public async ValueTask FocusAndSelectInput(string id)
        {
            await JS.InvokeVoidAsync("focusAndSelectInput", id);
        }

        public async ValueTask DownloadFile(byte[] bytes, string fileName)
        {
            var fileStream = new MemoryStream(bytes);
            using var streamRef = new DotNetStreamReference(stream: fileStream);
            await JS.InvokeVoidAsync("downloadFileFromStream", fileName, streamRef);
        }

        public async ValueTask DownloadFile(object obj, string fileName)
        {
            var json = JsonSerializer.Serialize(obj);
            var data = Encoding.UTF8.GetBytes(json);
            await JS.InvokeVoidAsync("downloadFileFromObject", fileName, Convert.ToBase64String(data));
        }

        public async ValueTask<string> GetItem(string key)
        {
            var item = await JS.InvokeAsync<string>("getItem", key);
            return item;
        }

        public async ValueTask SetItem(string key, string value) => await JS.InvokeVoidAsync("setItem", key, value);

        public async ValueTask SetItem(string key, object value)
        {
            var json = JsonSerializer.Serialize(value);
            await JS.InvokeVoidAsync("setItem", key, json);
        }

        public async ValueTask RemoveItem(string key) => await JS.InvokeVoidAsync("removeItem", key);

        public async ValueTask<string> GetCookie(string nombre) => await JS.InvokeAsync<string>("getCookie", nombre);

        public async ValueTask OnClickById(string id) => await JS.InvokeVoidAsync("onClickbyId", id);

        public async ValueTask ScrollGrilla(string idGrilla, ScrollEdge scrollEdge) => await JS.InvokeVoidAsync("ScrollGrilla", idGrilla, scrollEdge);

        public async ValueTask RadzenNumberFocusAndSelectByName(string name) => await JS.InvokeVoidAsync("radzenNumberFocusAndSelectByName", name);

        public async ValueTask RadzenNumberFocusAndSelectById(string id) => await JS.InvokeVoidAsync("radzenNumberFocusAndSelectById", id);

        public async ValueTask Imprimir(string componentId = "asd") => await JS.InvokeVoidAsync("Imprimir", componentId);

        public async ValueTask RadzenFocusByName(string name) => await JS.InvokeVoidAsync("radzenFocusByName", name);

        public async ValueTask RadzenFocusAndSelectById(string id) => await JS.InvokeVoidAsync("radzenFocusAndSelectById", id);

        public async ValueTask ScrolltwoGrids(string id1, string id2) => await JS.InvokeVoidAsync("ScrolltwoGrids", id1, id2);

        public async ValueTask ScrollGridToIndex(string idGrid, int index) => await JS.InvokeVoidAsync("ScrollGridToIndex", idGrid, index);

        public async ValueTask AddStyleToClass(string className, string style, int index) => await JS.InvokeVoidAsync("AddStyleToClass", className, style, index);

        public async ValueTask HorizontalScrollGrilla(string idGrilla, ScrollEdge scrollEdge) => await JS.InvokeVoidAsync("HorizontalScrollGrilla", idGrilla, scrollEdge);

        public async ValueTask ScrollGridToSelected(string idGrid) => await JS.InvokeVoidAsync("ScrollGridToSelected", idGrid);

        public async ValueTask NavigationOnGrid(string arrow, string idGrid, int cantCols) => await JS.InvokeVoidAsync("navigationOnGrid", arrow, idGrid, cantCols);

        public void Dispose()
        {
            GC.SuppressFinalize(this);
            mod?.Dispose();
        }
    }
}
