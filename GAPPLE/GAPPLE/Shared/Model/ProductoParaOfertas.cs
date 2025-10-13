namespace GAPPLE.Shared.Model
{
    public class ProductoParaOfertas : ICloneable
    {
        public string Familia { get; set; }
        public string CodigoProducto { get; set; }
        public string Descripcion { get; set; }
        public bool Probador { get; set; }
        public bool ModificadoManual { get; set; }
        public int CantidadProbador { get; set; }
        public decimal Precio { get; set; }
        public decimal PrecioConDescuento => Precio * (1 - DescuentoFinal / 100);
        public decimal DescuentoFinal { get; set; }
        public int CantidadSeleccionadaAnterior { get; set; }
        public int CantidadSeleccionada { get; set; }
        public int CantidadObsequio { get; set; }
        public string Sinonimo { get; set; }
        public string CodigoComplemento { get; set; }

        public object Clone() => MemberwiseClone();
    }
}
