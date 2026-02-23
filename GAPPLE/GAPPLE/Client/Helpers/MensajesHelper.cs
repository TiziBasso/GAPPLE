namespace GAPPLE.Client.Helpers
{
    public static class MensajesHelper
    {
        public static string ErrorSummary500 => "Ha ocurrido un error inesperado!";
        public static string ErrorDetail500 => "Por favor contacte a Sistemas!";
        public static string ErrorSummaryModel => "No se puede completar la operación";
        public static string ErrorDetailModel => "Compruebe todos los datos ingresados!";
        public static string Title => "Zentra";
        public static string Export => "Exportando...";
        public static string EmptyResponse => "La búsqueda no ha devuelto resultados!";
        public static string ErrorFileFormat => "El archivo ingresado no contiene el formato requerido";
        public static string ErrorEmptyFile => "El archivo cargado está vacío";
        public static string ErrorFileType => "El tipo de archivo ingresado no es admitido";
        public static string EmptySearch => "Debe ingresar al menos un filtro para la búsqueda!";
        public static string ProductoPasivo => "El producto se encuentra en estado pasivo";
        public static string SKUPasivo => "La variante se encuentra en estado pasivo";
        public static string Saving => "Guardando...";
        public static string Cargando => "Cargando...";
        public static string SinConexionSummary => "No hay conexión!";
        public static string SinConexionDetail => "Conectate a la red para continuar";
        public static string Procesando => "Procesando...";
        public static string Buscando => "Buscando...";
        public static string InvalidURL => "La URL ingresada no es correcta";
        public static string HttpRequestException => "No se pudo conectar con el servidor";
        public static string TokenCanceled => "La petición fue cancelada";
        public static string Todos => "Todos";
        public static string Todas => "Todas";
        public static string Varias => "Varias seleccionadas";
        public static string Varios => "Varios seleccionados";
        public static string ErrorFileSize(string size) => $"El archivo excede el tamaño maximo de {size}";
        public static string IncorrectParameterFormat(string value) => $"El {value} ingresado es incorrecto";
        public static string SaveSuccess() => "Los datos se han guardado correctamente!";
        public static string SaveSuccess(string value) => $"{value} se ha guardado correctamente!";
        public static string SaveSuccess(string value, string operacion) => $"{value} {operacion} correctamente!";
        /// <summary>
        /// "¿Desea confirmar la operación?"
        /// </summary>
        public static string Confirm() => "¿Desea confirmar la operación?";
        /// <summary>
        /// "¿Desea {operacion} la operación?"
        /// </summary>
        /// <param name="operacion"></param>
        public static string Confirm(string operacion) => $"¿Desea {operacion} la operación?";
        /// <summary>
        /// "¿Desea {operacion} {value}?"
        /// </summary>
        /// <param name="operacion"></param>
        /// <param name="value"></param>
        public static string Confirm(string operacion, string value) => $"¿Desea {operacion} {value}?";
        public static string Cancel() => "¿Desea cancelar la operación?";
        public static string Delete() => "Los datos se eliminaron correctamente";
        public static string Delete(string value) => $"{value} se eliminó correctamente";
        public static string DeleteObjectFromGrid(string value) => $"¿Desea eliminar {value}?";
        public static string DateRange() => "La fecha desde no puede ser mayor a la fecha hasta";
        public static string DateRange(int range) => $"Las fechas a filtrar no pueden ser mayor a {range} días";
        public static string Range(string prop) => $"{prop} desde no puede ser mayor a {prop.ToLower()} hasta";
        public static string ErrorSummary748(int idProducto) => $"Error 748 en el producto {idProducto}"; //este error es cuando el precio de un sku se pide de una fecha que ya no existe y hay que buscarlo en historico
        public static string Unselected(string value) => $"Debe seleccionar {value}";
    }
}
