namespace GAPPLE.Shared.Model
{
    public class Deposito
    {
        public int IdDeposito { get; set; }
        public string CodigoTango { get; set; }
        public string Descripcion { get; set; }
        public string DescripcionCod => $"{CodigoTango} - {Descripcion}";
        public bool Visible { get; set; }

    }
}
