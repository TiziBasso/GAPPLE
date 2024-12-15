namespace GAPPLE.Client.Helpers
{
    public static class MensajesHelper
    {
        public static string ErrorSummary500 => "Ha ocurrido un error inesperado!";

        public static string ErrorDetail500 => "Por favor contacte a sistemas!";

        public static string ErrorSummaryModel => "No se puede completar la operación";

        public static string ErrorDetailModel => "Compruebe todos los datos ingresados!";

        public static string Title => "Integra";

        public static string EmptyResponse => "La búsqueda no ha devuelto resultados!";

        public static string ErrorFileFormat => "El archivo ingresado no contiene el formato requerido";

        public static string ErrorEmptyFile => "El archivo cargado está vacío";

        public static string ErrorFileType => "El tipo de archivo ingresado no es admitido";

        public static string EmptySearch => "Debe ingresar al menos un filtro para la búsqueda!";

        public static string ErrorFileSize(string size) => $"El archivo excede el tamaño maximo de {size}";

        public static string SaveSuccess() => "Los datos se han guardado correctamente!";

        public static string SaveSuccess(string value) => $"{value} se ha guardado correctamente!";

        public static string Confirm() => "¿Desea confirmar la operación?";

        public static string Confirm(string operacion) => $"¿Desea {operacion} la operación?";

        public static string Confirm(string operacion, string value) => $"¿Desea {operacion} {value}?";

        public static string Cancel() => "¿Desea cancelar la operación?";

        public static string Delete() => "Los datos se eliminaron correctamente";

        public static string Delete(string value) => $"{value} se eliminó correctamente";

        public static string DateRange() => "La fecha desde no puede ser mayor a la fecha hasta";

        public static string DateRange(int range) => $"Las fechas a filtrar no pueden ser mayor a {range} días";

        public static string Range(string prop) => $"{prop} desde no puede ser mayor a {prop.ToLower()} hasta";

        public static string ErrorSummary748(int idProducto) => $"Error 748 en el producto {idProducto}";
    }
}
