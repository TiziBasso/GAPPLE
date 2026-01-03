namespace GAPPLE.Client.Tools
{
    public interface IJSFunction : IDisposable
    {
        #region FUNCIONES PARA MENÚ
        public ValueTask BuscarNodoActivo(string uri);
        public ValueTask ActivarNodoMenu(int id);
        public ValueTask ExpandirMenu(int id);
        #endregion

        public ValueTask DownloadFile(byte[] file, string fileName);
        public ValueTask DownloadFile(object obj, string fileName, string fileType = "text/json");
        public ValueTask OnClickById(string id);
        public ValueTask InputFocusAndSelect(string id);
        public ValueTask Open(string uri);
        public ValueTask Imprimir(string componentId, object dotNetObjectReference = null);
        public ValueTask AddStyleToClass(string className, string style, int index);
        public ValueTask CambioCSSAlternativo(bool result);
        public ValueTask CambioAlternativo(object dotNetObjectReference);
        public ValueTask OnBlurInput(string id);
        public ValueTask FocusInput(string id);
        public ValueTask GenerateBarcode(object obj);
        public ValueTask<bool> ValidarImagen(string url);
        public ValueTask GenerarGrafico(string tipoGrafico, object[] obj);
        public ValueTask ImprimirEtiquetasGondola(string componentId = "asd");
    }
}
