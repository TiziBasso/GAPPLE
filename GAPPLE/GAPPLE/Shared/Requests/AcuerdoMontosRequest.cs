using GAPPLE.Shared.Helpers;

namespace GAPPLE.Shared.Requests
{
    public class AcuerdoMontosRequest
    {
        public int? Id { get; set; }

        public int? IdAcuerdo { get; set; }

        public DateTime? FechaDesde { get; set; }

        public DateTime? FechaHasta { get; set; }
        public DateTime? FechaHastaFinDia => FechaHasta ?? FechaHasta.EndOfDay();

        public int? IdCliente { get; set; }

        public string CodCliente { get; set; }
        public string CodClienteLike => string.IsNullOrWhiteSpace(CodCliente) ? null : $"%{CodCliente}%";

        public string RazonSocial { get; set; }
        public string RazonSocialLike => string.IsNullOrWhiteSpace(RazonSocial) ? null : $"%{RazonSocial}%";

        public string CUIT { get; set; }
        public string CUITLike => string.IsNullOrWhiteSpace(CUIT) ? null : $"%{CUIT}%";

        public string Linea { get; set; }
        public string LineaLike => string.IsNullOrWhiteSpace(Linea) ? null : $"%{Linea}%";
    }
}