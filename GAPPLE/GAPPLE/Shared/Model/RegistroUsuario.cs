using GAPPLE.Shared.Helpers;

namespace GAPPLE.Shared.Model
{
    public abstract class RegistroUsuario
    {
        [ColumnName("AltaUsuario")]
        public string AltaUsuario { get; set; }
        [ColumnName("AltaRegistro")]
        public DateTime? AltaRegistro { get; set; }

        [ColumnName("EdicionUsuario")]
        public string EdicionUsuario { get; set; }
        [ColumnName("EdicionRegistro")]
        public DateTime? EdicionRegistro { get; set; }
    }
}
