using GAPPLE.Shared.Enums;
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

        [ColumnName("ClienteRazonSocial")]
        public string ClienteRazonSocial { get; set; }

        [ColumnName("ClienteCuit")]
        public string ClienteCuit { get; set; }

        [ColumnName("ClienteCategoriaIVA")]
        public string ClienteCategoriaIVA { get; set; }

        [ColumnName("IdMotivo")]
        public int? IdMotivo { get; set; }

        [ColumnName("Motivo")]
        public string MotivoDescripcion { get; set; }

        [ColumnName("IdDeposito")]
        public string IdDeposito { get; set; }

        [ColumnName("Deposito")]
        public string DepositoDescripcion { get; set; }

        [ColumnName("IdEstado")]
        public ComprobanteCabeceraEstadoEnum IdEstado { get; set; }

        [ColumnName("Estado")]
        public string EstadoDescripcion { get; set; }

        /// <summary>
        /// Null = No ingrsa, False = Pendiente de ingreso mercadería, True = Mercadería ingresada
        /// </summary>
        [ColumnName("MercaderiaIngresada")]
        public bool? MercaderiaIngresada { get; set; }

        [ColumnName("Fecha")]
        public DateOnly FechaComprobante { get; set; }

        [ColumnName("Observaciones")]
        public string Observaciones { get; set; }

        [ColumnName("ImporteTotal")]
        public decimal ImporteTotal { get; set; }

        public List<ComprobanteDetalle> Detalle { get; set; }
    }
}