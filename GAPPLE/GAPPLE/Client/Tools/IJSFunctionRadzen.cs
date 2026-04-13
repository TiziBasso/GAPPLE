using GAPPLE.Client.Helpers;

namespace GAPPLE.Client.Tools
{
    public interface IJSFunctionRadzen
    {
        public ValueTask ScrollGrilla(string idGrilla, ScrollEdgeEnum scrollEdge);
        public ValueTask ScrollDataList(string idGrilla);
        public ValueTask ScrollGridToIndex(string idGrid, int index);
        public ValueTask ScrollTwoGrids(string id1, string id2);
        public ValueTask NumberFocusAndSelectByName(string name);
        public ValueTask NumberFocusAndSelectById(string id);
        public ValueTask FocusAndSelectById(string id);
        public ValueTask DataGridNavigation(string arrow, string idGrid, int cantCols);
        public ValueTask ScrollGridToSelected(string idGrid);
        public ValueTask OnClickById(string id);
        public ValueTask RadzenNumericPasteHandler(string id);
    }
}
