using GAPPLE.Client.Helpers;
using Microsoft.JSInterop;

namespace GAPPLE.Client.Tools
{
    public class JSFunctionRadzen(IJSRuntime js) : IJSFunctionRadzen
    {
        public async ValueTask ScrollGrilla(string idGrilla, ScrollEdgeEnum scrollEdge) => await js.InvokeVoidAsync("scrollGrilla", idGrilla, scrollEdge);
        public async ValueTask ScrollDataList(string idGrilla) => await js.InvokeVoidAsync("scrollDataList", idGrilla);
        public async ValueTask ScrollGridToIndex(string idGrid, int index) => await js.InvokeVoidAsync("scrollGridToIndex", idGrid, index);
        public async ValueTask NumberFocusAndSelectByName(string name) => await js.InvokeVoidAsync("radzenNumberFocusAndSelectByName", name);
        public async ValueTask FocusAndSelectById(string id) => await js.InvokeVoidAsync("radzenFocusAndSelectById", id);
        public async ValueTask DataGridNavigation(string arrow, string idGrid, int cantCols) => await js.InvokeVoidAsync("dataGridNavigation", arrow, idGrid, cantCols);
        public async ValueTask NumberFocusAndSelectById(string id) => await js.InvokeVoidAsync("radzenNumberFocusAndSelectById", id);
        public async ValueTask ScrollGridToSelected(string idGrid) => await js.InvokeVoidAsync("ScrollGridToSelected", idGrid);
        public async ValueTask ScrollTwoGrids(string id1, string id2) => await js.InvokeVoidAsync("ScrollTwoGrids", id1, id2);
        public async ValueTask OnClickById(string id) => await js.InvokeVoidAsync("onClickbyId", id);
        public async ValueTask RadzenNumericPasteHandler(string id) => await js.InvokeVoidAsync("radzenNumericPasteHandler", id);
    }
}
