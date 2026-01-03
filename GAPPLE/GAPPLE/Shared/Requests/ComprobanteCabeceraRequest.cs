using GAPPLE.Shared.Enums;

namespace GAPPLE.Shared.Requests
{
    public class ComprobanteCabeceraRequest
    {
        private string _codigoOrden, _codigoTango;

        public DateTime FechaDesde { get; set; }
        public DateTime FechaHasta { get; set; }
        public string CodigoOrden { get => string.IsNullOrWhiteSpace(_codigoOrden) ? null : _codigoOrden; set => _codigoOrden = value; }
        public string CodigoTango { get => string.IsNullOrWhiteSpace(_codigoTango) ? null : _codigoTango; set => _codigoTango = value; }
        public bool? MercaderiaIngresada { get; set; }
        public ComprobanteCabeceraEstadoEnum? IdEstado { get; set; }
    }
}
