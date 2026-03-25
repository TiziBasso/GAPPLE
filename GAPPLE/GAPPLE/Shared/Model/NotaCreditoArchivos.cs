namespace GAPPLE.Shared.Model
{
    public class NotaCreditoArchivo
    {
        public int IdArchivo { get; set; }
        public int IdComprobante { get; set; }
        public string NombreArchivo { get; set; }
        public string Path { get; set; }
        public string TipoArchivo { get; set; }
        public DateTime FechaSubida { get; set; }
    }
}
