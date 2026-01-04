using System.ComponentModel.DataAnnotations;

namespace GAPPLE.Shared.Model
{
    public class Motivo : ICloneable
    {
        public int IdMotivo { get; set; }
        public string Descripcion { get; set; }
        public bool Pasivo { get; set; }
        public int? IdDeposito { get; set; }
        public string DescripcionDeposito { get; set; }
        public bool? VisibleDeposito { get; set; }

        public DateTime AltaRegistro { get; set; }
        public string AltaUsuario { get; set; }
        public DateTime? EdicionRegistro { get; set; }
        public string EdicionUsuario { get; set; }

        public object Clone() => MemberwiseClone();
    }
}