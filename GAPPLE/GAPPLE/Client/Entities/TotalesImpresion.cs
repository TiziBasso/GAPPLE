namespace GAPPLE.Client.Entities
{
    /// <summary>
    /// Totales generales de unidades de un listado impreso (aprobados o pendientes).
    /// Se calculan sobre el pedido completo y se imprimen una sola vez, al final del listado.
    /// </summary>
    public class TotalesImpresion
    {
        public int Cantidad { get; set; }
        public int Probador { get; set; }
        public int Obsequio { get; set; }
    }
}
