using GAPPLE.Shared.Enums;

namespace GAPPLE.Shared.Requests
{
    public class ComprobanteCabeceraRequest
    {
        private string _codigoOrden, _codigoTango, _razonSocialCliente;

        public int? IdComprobante { get; set; }
        public DateTime FechaDesde { get; set; } = DateTime.Today.AddMonths(-5);
        public DateTime FechaHasta { get; set; } = DateTime.Today.AddDays(1).AddMinutes(-1);
        public string CodigoOrden { get => string.IsNullOrWhiteSpace(_codigoOrden) ? null : _codigoOrden; set => _codigoOrden = value; }
        public string CodigoTango { get => string.IsNullOrWhiteSpace(_codigoTango) ? null : _codigoTango; set => _codigoTango = value; }
        public bool? MercaderiaIngresada { get; set; }
        public ComprobanteCabeceraEstadoEnum? IdEstado { get; set; }
        public string RazonSocialCliente { get => string.IsNullOrWhiteSpace(_razonSocialCliente) ? null : _razonSocialCliente; set => _razonSocialCliente = value; }
        public bool ConDetalle { get; set; }
        public int? IdUsuario { get; set; }
    }
}
