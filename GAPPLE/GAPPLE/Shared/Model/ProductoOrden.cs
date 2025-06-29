namespace GAPPLE.Shared.Model
{
    public class ProductoOrden
    {
        public string CodigoProducto { get; set; }
        public string Descripcion { get; set; }
        public int CantidadProbador { get; set; }
        public int CantidadSeleccionada { get; set; }
        public decimal Descuento { get; set; }
        public decimal Precio { get; set; }
        public decimal PrecioTotal { get => Precio * CantidadSeleccionada; }
    }
}
