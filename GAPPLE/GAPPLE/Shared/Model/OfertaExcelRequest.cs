namespace GAPPLE.Shared.Model
{
    /// <summary>
    /// Request para procesar un archivo Excel de carga masiva de ofertas.
    /// El archivo debe tener 3 columnas (a partir de la fila 2): Titulo de Oferta, SKU, Descuento.
    /// Si CodCliente está informado se crearán ofertas especiales, sino ofertas comunes.
    /// </summary>
    public class OfertaExcelRequest
    {
        public byte[] File { get; set; }
        public string Linea { get; set; }
        public string CodCliente { get; set; }
        public DateTime Desde { get; set; }
        public DateTime Hasta { get; set; }
        public string ConnectionId { get; set; }
    }
}
