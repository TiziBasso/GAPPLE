using System.ComponentModel.DataAnnotations;

namespace GAPPLE.Shared.Model
{
    public class Usuario : ICloneable
    {
        public int IdUsuario { get; set; }

        [Required(ErrorMessage = "Debe ingresar un nombre de usuario"),
            StringLength(50, ErrorMessage = "Maximo de caracteres 50")]
        public string NombreUsuario { get; set; }

        [Required(ErrorMessage = "Debe ingresar apellido y nombre"),
            StringLength(100, ErrorMessage = "Maximo de caracteres 100")]
        public string ApellidoYNombre { get; set; }

        [Required(ErrorMessage = "Debe seleccionar un perfil")]
        public int Perfil { get; set; }

        public PerfilUsuario? PerfilCompleto { get; set; } = null;

        [Required(ErrorMessage = "Debe seleccionar la provincia")]
        public string Provincia { get; set; }

        [Required(ErrorMessage = "Debe ingresar una contraseña")]
        public string Contraseña { get; set; }

        [Required(ErrorMessage = "Debe ingresar un email"),
            RegularExpression(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$", ErrorMessage = "Email invalido"),
                StringLength(50, ErrorMessage = "Maximo de caracteres 50")]
        public string Email { get; set; }
        public IEnumerable<string> Zonas { get; set; } = new List<string>();
        public IEnumerable<string> Vendedores { get; set; } = new List<string>();

        public int? IdVendedor { get; set; }

        public bool Habilitado { get; set; }

        public object Clone()
        {
            Usuario usuario = (Usuario)MemberwiseClone();
            return usuario;
        }
    }
}
