using System.Numerics;

namespace GAPPLE.Shared.Requests
{
    public class AcuerdosRequest
    {
        public DateTime FechaDesde { get; set; }
        public DateTime FechaHasta { get; set; }
        public int? IdCliente { get; set; }
        public string CodCliente { get; set; }
        public string RazonSocial { get; set; }
        public string CUIT { get; set; }
        public string Linea { get; set; }
        public bool? Activo { get; set; }
    }
}
