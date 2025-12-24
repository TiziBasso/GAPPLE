using GAPPLE.Shared.Helpers;

namespace GAPPLE.Shared.Model
{
    public class ComprobanteCabecera : RegistroUsuario
    {
        [ColumnName("IdComprobante")]
        public int IdComprobante { get; set; }

        [ColumnName("CodigoOrden")]
        public string CodigoOrden { get; set; }

        [ColumnName("CodigoTango")]
        public string CodigoTango { get; set; }

        [ColumnName("TipoComprobante")]
        public string TipoComprobante { get; set; }

        [ColumnName("IdCliente")]
        public int? IdCliente { get; set; }

        [ColumnName("IdMotivo")]
        public int? IdMotivo { get; set; }

        [ColumnName("Depositos")]
        public string Depositos { get; set; }

        [ColumnName("Fecha")]
        public DateOnly FechaComprobante { get; set; }

        List<>
    }
}