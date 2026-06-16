namespace GAPPLE.Shared.Model
{
    /// <summary>
    /// DTO genérico que representa un producto proveniente de un archivo Excel.
    /// Puede ser casteado o mapeado a clases de producto específicas según el contexto.
    /// </summary>
    public class ProductoArchivo
    {
        public int IdProducto { get; set; }
        public string CodigoProducto { get; set; }
        public string Descripcion { get; set; }
        public string Linea { get; set; }
        public int CantidadSeleccionada { get; set; }
        public int CantidadProbador { get; set; }
        public int CantidadObsequio { get; set; }
        public decimal DescuentoCliente { get; set; }
        public decimal DescuentoOferta { get; set; }
        public decimal DescuentoTotal { get; set; }
        public decimal Precio { get; set; }
        public decimal PrecioConDescuento { get; set; }
        public decimal PrecioTotal { get; set; }
        public bool Pasivo { get; set; }
        public string CodComplemento { get; set; }
    }
}
