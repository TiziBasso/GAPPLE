using GAPPLE.Shared.Helpers;

namespace GAPPLE.Shared.Model
{
    public class ComprobanteDetalle
    {
        [ColumnName("IdComprobante")]
        public int IdComprobante { get; set; }

        [ColumnName("Linea")]
        public int NumeroLinea { get; set; }

        [ColumnName("CodProducto")]
        public string CodProducto { get; set; }

        [ColumnName("DescripcionProducto")]
        public string DescripcionProducto { get; set; }

        [ColumnName("Cantidad")]
        public int Cantidad { get; set; }

        [ColumnName("Precio")]
        public decimal Precio { get; set; }

        [ColumnName("Descuento")]
        public decimal Descuento { get; set; }

        public decimal PrecioConDescuento { get => Precio * (1 - Descuento / 100); }
        public decimal PrecioTotal { get => PrecioConDescuento * Cantidad; }

        [ColumnName("Detalle")]
        public string Detalle { get; set; }
    }
}
