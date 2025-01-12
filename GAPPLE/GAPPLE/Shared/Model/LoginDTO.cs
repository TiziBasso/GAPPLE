using System.ComponentModel.DataAnnotations;

namespace GAPPLE.Shared.Model
{
    public class LoginDTO
    {
        [Required(ErrorMessage = "Debe ingresar un usario")]
        public string Correo { get; set; }
        [Required(ErrorMessage = "Debe ingresar su contraseña")]
        public string Clave { get; set; }
    }
}
