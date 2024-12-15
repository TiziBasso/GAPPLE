namespace GAPPLE.Shared.Model
{
    public abstract class RegistroUsuario
    {
        public string AltaUsuario { get; set; }
        public DateTime AltaRegistro { get; set; }
        public string EdicionUsuario { get; set; }
        public DateTime EdicionRegistro { get; set; }
    }
}
