namespace GAPPLE.Shared.Model
{
    public class ProductoOrden
    {
        public int IdProducto { get; set; }
        public string CodigoProducto { get; set; }
        public string Descripcion { get; set; }
        public string Linea { get; set; }
        public int CantidadProbador { get; set; }
        public int CantidadObsequio { get; set; }
        public int CantidadSeleccionada { get; set; }
        public decimal DescuentoCliente { get; set; } = 0;
        public decimal DescuentoOferta { get; set; } = 0;
        /// <summary>
        /// Descuento ingresado manualmente por el usuario. Cuando es null,
        /// DescuentoTotal devuelve el valor calculado (cliente + oferta en cascada).
        /// Se asigna únicamente desde la UI; el sistema nunca lo toca.
        /// </summary>
        public decimal? DescuentoManual { get; set; } = null;
        private decimal _descuentoCalculado => 100m - (100m * (1 - (DescuentoCliente / 100)) * (1 - (DescuentoOferta / 100)));
        public decimal DescuentoTotal
        {
            get => DescuentoManual ?? _descuentoCalculado;
            set => DescuentoManual = value;
        }
        public decimal Precio { get; set; }
        public decimal PrecioConDescuento { get => Precio * (1 - DescuentoTotal / 100); }
        public decimal PrecioTotal { get => PrecioConDescuento * CantidadSeleccionada; }
        public bool Pasivo { get; set; }
        public string CodComplemento { get; set; }
    }
}
