using System.ComponentModel.DataAnnotations;

namespace GAPPLE.Shared.Model
{
    public class Menu
    {
        public int IdPermiso { get; set; }

        public int? IdPadre { get; set; }

        [StringLength(100, ErrorMessage = "No puede superar los 100 caracteres")]
        public string? Nombre { get; set; }

        public char Tipo { get; set; }

        [StringLength(200, ErrorMessage = "No puede superar los 200 caracteres")]
        public string? Url { get; set; }

        [StringLength(200, ErrorMessage = "No puede superar los 200 caracteres")]
        public string? Icono { get; set; }

        public int? Orden { get; set; }

        public bool TieneHijos { get; set; }
    }
}
