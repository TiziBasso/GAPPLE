namespace GAPPLE.Shared.Model
{
    public class ProductoOrden
    {
        public string CodigoProducto { get; set; }
        public string Descripcion { get; set; }
        public string Linea { get; set; }
        public int CantidadProbador { get; set; }
        public int CantidadSeleccionada { get; set; }
        public decimal Descuento { get; set; }
        public decimal Precio { get; set; }
        public decimal PrecioConDescuento { get => Precio * (1 - Descuento / 100); }
        public decimal PrecioTotal { get => PrecioConDescuento * CantidadSeleccionada; }

    }
}
