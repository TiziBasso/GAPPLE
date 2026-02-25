using GAPPLE.Shared.Helpers;

namespace GAPPLE.Shared.Model
{
    public class Acuerdo : RegistroUsuario
    {
        [ColumnName("IdAcuerdo")]
        public int IdAcuerdo { get; set; }

        [ColumnName("IdCliente")]
        public int? IdCliente { get; set; }

        [ColumnName("CodigoCliente")]
        public string CodigoCliente { get; set; }

        [ColumnName("RazonSocial")]
        public string RazonSocial { get; set; }

        [ColumnName("CUIT")]
        public string CUIT { get; set; }

        [ColumnName("Linea")]
        public string Linea { get; set; }

        [ColumnName("Condicion")]
        public string Condicion { get; set; }

        [ColumnName("FechaDesde")]
        public DateTime? FechaDesde { get; set; }

        [ColumnName("FechaHasta")]
        public DateTime? FechaHasta { get; set; }

        [ColumnName("Activo")]
        public bool Activo { get; set; }
    }
}
