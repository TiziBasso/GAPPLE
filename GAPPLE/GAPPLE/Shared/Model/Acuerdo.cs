using GAPPLE.Shared.Helpers;
using System.ComponentModel.DataAnnotations;

namespace GAPPLE.Shared.Model
{
    public class Acuerdo : RegistroUsuario, ICloneable
    {
        public int IdAcuerdo { get; set; }

        public int? IdCliente { get; set; }

        [Required(ErrorMessage = "Debe seleccionar una linea")]
        public string Linea { get; set; }

        public bool Aprobado { get; set; }

        public string Condicion { get; set; }

        public DateTime? FechaDesde { get; set; }

        public DateTime? FechaHasta { get; set; }

        public bool Activo { get; set; }

        public object Clone() => MemberwiseClone();
    }
}
