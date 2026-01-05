using GAPPLE.Shared.Helpers;
using System.ComponentModel.DataAnnotations;

namespace GAPPLE.Shared.Model
{
    public class Motivo : RegistroUsuario, ICloneable
    {
        [ColumnName("IdMotivo")]
        public int IdMotivo { get; set; }
        [ColumnName("Descripcion")]
        public string Descripcion { get; set; }
        [ColumnName("Pasivo")]
        public bool Pasivo { get; set; }
        [ColumnName("IdDeposito")]
        public int? IdDeposito { get; set; }
        [ColumnName("DescripcionDeposito")]
        public string DescripcionDeposito { get; set; }
        [ColumnName("VisibleDeposito")]
        public bool? VisibleDeposito { get; set; }
        public object Clone() => MemberwiseClone();
    }
}