using GAPPLE.Shared.Enums;
using System.Numerics;

namespace GAPPLE.Shared.Requests
{
    public class AcuerdosRequest
    {
        private string _codCliente, _razonSocial, _cuit, _linea;
        public int? IdAcuerdo { get; set; }
        public DateTime? FechaDesde { get; set; }
        public DateTime? FechaHasta { get; set; }
        public int? IdCliente { get; set; }
        public string CodCliente { get => string.IsNullOrWhiteSpace(_codCliente) ? null : _codCliente; set => _codCliente = value; }
        public string RazonSocial { get => string.IsNullOrWhiteSpace(_razonSocial) ? null : _razonSocial; set => _razonSocial = value; }
        public string CUIT { get => string.IsNullOrWhiteSpace(_cuit) ? null : _cuit; set => _cuit = value; }
        public string Linea { get => string.IsNullOrWhiteSpace(_linea) ? null : _linea; set => _linea = value; }
        public AcuerdosEstadoEnum? IdEstado { get; set; }
    }
}
