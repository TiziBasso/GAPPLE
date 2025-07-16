namespace GAPPLE.Shared.Model
{
    public class ProductoOrden
    {
        public string CodigoProducto { get; set; }
        public string Descripcion { get; set; }
        public string Linea { get; set; }
        public int CantidadProbador { get; set; }
        public int CantidadSeleccionada { get; set; }
        public decimal DescuentoCliente { get; set; } = 0;
        public decimal DescuentoOferta { get; set; } = 0;
        private bool _modificadoManual = false;
        private decimal _descuentoTotal;
        private decimal _descuentoCalculado => 100m - (100m * (1 - (DescuentoCliente / 100)) * (1 - (DescuentoOferta / 100)));
        public decimal DescuentoTotal
        {
            get
            {
                if (_modificadoManual)
                    return _descuentoTotal;
                else
                    return _descuentoCalculado;
            }
            set
            {
                if ((_modificadoManual && _descuentoTotal == value) || _descuentoCalculado == value)
                    return;
                _descuentoTotal = value;
                _modificadoManual = true;
            }
        }
        public decimal Precio { get; set; }
        public decimal PrecioConDescuento { get => Precio * (1 - DescuentoTotal / 100); }
        public decimal PrecioTotal { get => PrecioConDescuento * CantidadSeleccionada; }

    }
}
