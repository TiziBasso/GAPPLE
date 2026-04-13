namespace GAPPLE.Shared.Structs
{
    public struct TipoComprobante
    {
        public string IdTipoComprobante { get; set; }
        public string DescripcionComprobante { get; set; }

        public TipoComprobante(string idTipoComprobante, string descripcionComprobante)
        {
            IdTipoComprobante = idTipoComprobante;
            DescripcionComprobante = descripcionComprobante;
        }
    }
}
