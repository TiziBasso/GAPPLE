using System.Globalization;
using Microsoft.AspNetCore.Components;
using Radzen;

namespace GAPPLE.Client.Helpers
{
    public class RadzenHelper
    {
        public static DateTime? ParseDate(string input)
        {
            string[] formats = ["dd-MM-yyyy", "dd/MM/yyyy", "dd-MM-yy", "dd/MM/yy", "ddMMyyyy", "ddMMyy", "dd-MM", "dd/MM", "ddMM"];

            foreach (var format in formats)
            {
                if (DateTime.TryParseExact(input, format, null, DateTimeStyles.None, out var result))
                {
                    return result;
                }
            }

            return null;
        }

        public static Dictionary<string, string> Traducciones => new() { { "GroupPanelText", "Arrastre una columna aquí para agrupar" } };

        public static string PagingSummaryFormat => "Pagina {0} de {1} <b>({2} registros)</b>";

        public static decimal ValueConvert(string value)
        {
            var decimalSeparator = CultureInfo.CurrentUICulture.NumberFormat.NumberDecimalSeparator;
            return decimal.Parse(value.Replace(".", decimalSeparator).Replace(",", decimalSeparator));
        }

        public static Dictionary<string, Type> DynamicGridGetHeaders(Dictionary<string, string> data)
        {
            Dictionary<string, Type> headers = [];
            foreach (var p in data)
            {
                if (int.TryParse(p.Value, out int outInt))
                    headers.Add(p.Key, typeof(int));
                else if (long.TryParse(p.Value, out long outLong))
                    headers.Add(p.Key, typeof(long));
                else if (decimal.TryParse(p.Value, out decimal outDecimal))
                    headers.Add(p.Key, typeof(decimal));
                else if (DateTime.TryParseExact(p.Value, "dd/MM/yyyy", null, DateTimeStyles.AssumeUniversal, out DateTime outDateTime))
                    headers.Add(p.Key, typeof(DateTime));
                else
                    headers.Add(p.Key, typeof(string));
            }
            return headers;
        }

        public static ConfirmOptions Confirm()
        {
            return new()
            {
                CancelButtonText = "No",
                OkButtonText = "Sí",
                AutoFocusFirstElement = false,
                ShowClose = false,
                Width = "700px"
            };
        }

        public static ConfirmOptions Confirm(string okButtonText, string cancelButtonText)
        {
            return new()
            {
                CancelButtonText = cancelButtonText,
                OkButtonText = okButtonText,
                AutoFocusFirstElement = false,
                ShowClose = false,
                Width = "700px"
            };
        }

        public static ConfirmOptions Confirm(bool autoFocusFirstElement = false, string bottom = "50vh", string cancelButtonText = "No", bool closeDialogOnEsc = true,
                                      bool closeDialogOnOverlayClick = false, int closeTabIndex = 0, string cssClass = "", bool draggable = false,
                                      string height = "fit-content", string left = "50vw", string okButtonText = "si", bool resizable = false, bool showClose = false,
                                      bool showTitle = true, string style = "", string top = "50vh", string width = "700px")
        {
            return new()
            {
                AutoFocusFirstElement = autoFocusFirstElement,
                Bottom = bottom,
                CancelButtonText = cancelButtonText,
                CloseDialogOnEsc = closeDialogOnEsc,
                CloseDialogOnOverlayClick = closeDialogOnOverlayClick,
                CloseTabIndex = closeTabIndex,
                CssClass = cssClass,
                Draggable = draggable,
                Height = height,
                Left = left,
                OkButtonText = okButtonText,
                Resizable = resizable,
                ShowClose = showClose,
                ShowTitle = showTitle,
                Style = style,
                Top = top,
                Width = width
            };
        }

        public static AlertOptions Alert()
        {
            return new()
            {
                CloseDialogOnEsc = false,
                OkButtonText = "Aceptar"
            };
        }

        public static RenderFragment GetRenderFragment(string message)
        {
            return __builder =>
            {
                __builder.AddMarkupContent(0, message);
            };
        }
    }
}
