using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace GAPPLE.Shared.Model
{
    public class Usuario : ICloneable
    {
        public int IdUsuario { get; set; }

        [Required(ErrorMessage = "Debe ingresar un nombre de usuario"),
            StringLength(50, ErrorMessage = "Maximo de caracteres 50")]
        public string NombreUsuario { get; set; }

        [Required(ErrorMessage = "Debe ingresar apellido y nombre"),
            StringLength(50, ErrorMessage = "Maximo de caracteres 50")]
        public string ApellidoYNombre { get; set; }

        public bool HabilitadoPOS { get; set; }

        public string IdUsuarioPOS { get; set; }

        [Required(ErrorMessage = "Debe seleccionar al menos un perfil")]
        public List<int> Perfiles { get; set; }
        public List<PerfilUsuario> PerfilesCompleto { get; set; } = new List<PerfilUsuario>();

        public string Domicilio { get; set; }

        public string Localidad { get; set; }

        public string CodigoPostal { get; set; }

        [Required(ErrorMessage = "Debe seleccionar la provincia")]
        public string Provincia { get; set; }

        public string Telefono { get; set; }

        [RegularExpression(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$", ErrorMessage = "Email invalido"),
            StringLength(50, ErrorMessage = "Maximo de caracteres 50")]
        public string Email { get; set; }

        public DateTime FechaExpiracion { get; set; }

        public string PerfilesJoin
        {
            get
            {
                if (Perfiles != null && Perfiles.Any())
                    return string.Join(',', Perfiles.ToArray());
                else
                    return string.Empty;
            }
        }
        //TODO: borrar?
        public int IdTerminal { get; set; }

        public bool Pasivo { get; set; }

        public string Pais { get; set; }

        [Required(ErrorMessage = "Debe seleccionar un perfil")]
        public int? IdPerfilIntegra { get; set; }

        public object Clone()
        {
            Usuario usuario = (Usuario)MemberwiseClone();
            List<int> perfiles = new();
            Perfiles.ForEach(x => perfiles.Add(x));
            usuario.Perfiles = perfiles;
            return usuario;
        }
    }
}
