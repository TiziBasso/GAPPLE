using System.ComponentModel.DataAnnotations;

namespace GAPPLE.Shared.Model
{
    public class ComprobanteDetalleLibre
    {
        [Required(ErrorMessage = "Debe ingresar el concepto")]
        public string Concepto { get; set; }
    }
}
