namespace GAPPLE.Shared.Model
{
    /// <summary>
    /// Respuesta liviana para consumidores externos (ej. Reclamos) que necesitan
    /// el cliente y el detalle de una NC sin traer toda la cabecera del comprobante.
    /// </summary>
    public class NotaCreditoResumen
    {
        public int IdComprobante { get; set; }
        public string CodCliente { get; set; }
        public string ClienteRazonSocial { get; set; }
        public List<ComprobanteDetalle> Detalle { get; set; } = [];
    }
}
