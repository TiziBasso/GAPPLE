namespace GAPPLE.Shared.Model
{
    public class AcuerdoCliente : ICloneable
    {
        public int IdCliente { get; set; }
        public string CodigoCliente { get; set; }
        public string RazonSocial { get; set; }
        public List<Acuerdo> Acuerdos { get; set; } = [];
        public object Clone() => MemberwiseClone();
    }
}
