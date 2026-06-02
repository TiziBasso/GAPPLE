using GAPPLE.Shared.Enums;

namespace GAPPLE.Shared.Requests
{
    public class ReclamoRequest
    {
        private string _razonSocialCliente;

        public DateTime FechaDesde { get; set; } = DateTime.Today.AddDays(-30);
        public DateTime FechaHasta { get; set; } = DateTime.Today.AddDays(1).AddMinutes(-1);

        public string RazonSocialCliente
        {
            get => string.IsNullOrWhiteSpace(_razonSocialCliente) ? null : _razonSocialCliente;
            set => _razonSocialCliente = value;
        }

        public ReclamoTipoEnum?   Tipo   { get; set; }
        public ReclamoMotivoEnum? Motivo { get; set; }
    }
}
