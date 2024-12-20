namespace GAPPLE.Client.Tools
{
    public interface IJSFunction : IDisposable
    {
        internal ValueTask FocusInput(string id);
        internal ValueTask FocusAndSelectInput(string id);
        internal ValueTask SetItem(string key, string value);
        internal ValueTask SetItem(string key, object value);
        internal ValueTask<string> GetItem(string key);
        internal ValueTask RemoveItem(string key);
        internal ValueTask DownloadFile(byte[] file, string fileName);
        internal ValueTask DownloadFile(object obj, string fileName);
        internal ValueTask<string> GetCookie(string nombre);
        internal ValueTask OnClickById(string id);
        internal ValueTask ScrollGrilla(string idGrilla, ScrollEdge scroll);
        internal ValueTask Imprimir(string componentId = "asd");
        internal ValueTask RadzenNumberFocusAndSelectByName(string name);
        internal ValueTask RadzenNumberFocusAndSelectById(string id);
        internal ValueTask RadzenFocusByName(string name);
        internal ValueTask RadzenFocusAndSelectById(string id);
        internal ValueTask ScrolltwoGrids(string id1, string id2);
        internal ValueTask ScrollGridToIndex(string idGrid, int index);
        internal ValueTask AddStyleToClass(string className, string style, int index);
        internal ValueTask HorizontalScrollGrilla(string idGrilla, ScrollEdge scroll);
        internal ValueTask ScrollGridToSelected(string idGrid);
        internal ValueTask NavigationOnGrid(string arrow, string idGrid, int cantCols);
    }
}
