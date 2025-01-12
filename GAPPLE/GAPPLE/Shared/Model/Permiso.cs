using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAPPLE.Shared.Model
{
    public class Permiso
    {
        public int? IdPerfilOUsuario { get; set; }
        public int IdPermiso { get; set; }
        public int? IdPadre { get; set; }
        public string Descripcion { get; set; }
        public string TipoPermiso { get; set; }
        public bool TieneHijos { get; set; }
        public bool HuboCambios { get; set; }
        public bool Eliminar { get; set; }
        public bool HabilitadoPorPerfil { get; set; }
        public bool? HabilitadoPorUsuario { get; set; }
        public List<Permiso> Permisos { get; set; } = new();
    }
}
