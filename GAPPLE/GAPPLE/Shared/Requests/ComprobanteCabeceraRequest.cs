using GAPPLE.Shared.Enums;

namespace GAPPLE.Shared.Requests
{
    public class ComprobanteCabeceraRequest
    {
        private string _codigoOrden, _codigoTango, _razonSocialCliente;

        public int IdComprobante { get; set; }
        public DateTime FechaDesde { get; set; }
        public DateTime FechaHasta { get; set; }
        public string CodigoOrden { get => string.IsNullOrWhiteSpace(_codigoOrden) ? null : _codigoOrden; set => _codigoOrden = value; }
        public string CodigoTango { get => string.IsNullOrWhiteSpace(_codigoTango) ? null : _codigoTango; set => _codigoTango = value; }
        public string RazonSocialCliente { get => string.IsNullOrWhiteSpace(_razonSocialCliente) ? null : _razonSocialCliente; set => _razonSocialCliente = value; }
        public bool? MercaderiaIngresada { get; set; }
        public ComprobanteCabeceraEstadoEnum? IdEstado { get; set; }
        public bool ConDetalle { get; set; }
    }
}
