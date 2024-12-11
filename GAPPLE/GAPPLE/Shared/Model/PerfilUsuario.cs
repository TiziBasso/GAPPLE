using System.ComponentModel.DataAnnotations;

namespace GAPPLE.Shared.Model
{
    public class PerfilUsuario
    {
        /// <summary>
        /// Perfil Web
        /// Se usa para obtener el menú y los permisos
        /// </summary>
        public int? IdPerfil { get; set; }

        /// <summary>
        /// Descripción de perfil web
        /// </summary>
        [Required(ErrorMessage = "Debe ingresar una descripción"),
            StringLength(50, ErrorMessage = "No puede superar los 50 caracteres")]
        public string DescripcionPerfil { get; set; }

        /// <summary>
        /// Perfil de Integra WinForms
        /// Se usa para los reportes y otras cosas
        /// </summary>
        public int IdPerfilIntegra { get; set; }

        public PerfilUsuario() { }
        public PerfilUsuario(int idPerfil, string descripcionPerfil) => (IdPerfil, DescripcionPerfil) = (idPerfil, descripcionPerfil);
    }
}
