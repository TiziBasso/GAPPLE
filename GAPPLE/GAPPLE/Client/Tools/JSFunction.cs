using Microsoft.JSInterop;
using System.Text;
using System.Text.Json;


namespace GAPPLE.Client.Tools
{
    public class JSFunction(IJSRuntime js) : IJSFunction
    {
        #region FUNCIONES PARA MENÚ
        public async ValueTask BuscarNodoActivo(string uri) => await js.InvokeVoidAsync("buscarNodoActivo", uri);
        public async ValueTask ActivarNodoMenu(int id) => await js.InvokeVoidAsync("activarNodoMenu", id);
        public async ValueTask ExpandirMenu(int id) => await js.InvokeVoidAsync("expandirMenu", id);
        #endregion
        public async ValueTask DownloadFile(byte[] bytes, string fileName)
        {
            var fileStream = new MemoryStream(bytes);
            using var streamRef = new DotNetStreamReference(stream: fileStream);
            await js.InvokeVoidAsync("downloadFileFromStream", fileName, streamRef);
        }
        public async ValueTask DownloadFile(object obj, string fileName, string fileType = "text/json")
        {
            var json = JsonSerializer.Serialize(obj);
            var data = Encoding.UTF8.GetBytes(json);
            await js.InvokeVoidAsync("downloadFileFromObject", fileName, Convert.ToBase64String(data), fileType);
        }
        public async ValueTask FocusInput(string id) => await js.InvokeVoidAsync("focusInput", id);
        public async ValueTask OnClickById(string id) => await js.InvokeVoidAsync("onClickbyId", id);
        public async ValueTask InputFocusAndSelect(string id) => await js.InvokeVoidAsync("focusAndSelectInput", id);
        public async ValueTask Open(string uri) => await js.InvokeVoidAsync("open", uri, "_blank");
        public async ValueTask Imprimir(string componentId, object dotNetObjectReference) => await js.InvokeVoidAsync("Imprimir", componentId, dotNetObjectReference);
        public async ValueTask AddStyleToClass(string className, string style, int index) => await js.InvokeVoidAsync("AddStyleToClass", className, style, index);
        public async ValueTask CambioCSSAlternativo(bool result) => await js.InvokeVoidAsync("CambioCSSAlternativo", result);
        public async ValueTask CambioAlternativo(object dotNetObjectReference) => await js.InvokeVoidAsync("CambioAlternativo", dotNetObjectReference);
        public async ValueTask OnBlurInput(string id) => await js.InvokeVoidAsync("BlurInput", id);
        public async ValueTask GenerateBarcode(object obj) => await js.InvokeVoidAsync("generateBarcode", "#barcode", obj);
        public async ValueTask<bool> ValidarImagen(string url) => await js.InvokeAsync<bool>("image", url);
        public async ValueTask GenerarGrafico(string tipoGrafico, object[] obj) => await js.InvokeVoidAsync(tipoGrafico, obj);
        public async ValueTask ImprimirEtiquetasGondola(string componentId = "asd") => await js.InvokeVoidAsync("ImprimirEtiquetasGondola", componentId);
        public void Dispose() => GC.SuppressFinalize(this);
    }
}
