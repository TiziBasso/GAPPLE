namespace GAPPLE.Shared.Model
{
    public class Indicadores
    {
        public int PedidosIngresados { get; set; }
        public int PedidosAprobados { get; set; }
        public int PedidosPreparados { get; set; }
        public int CantidadesIngresadas { get; set; }
        public int CantidadesAprobadas { get; set; }
        public int CantidadesPendientes { get; set; }
        public decimal TotalPrecioConPendientes { get; set; }
        public decimal TotalPrecioPendientes { get; set; }
        public decimal TotalPrecioNoPendientes { get; set; }
        public decimal TotalPrecioEnTango { get; set; }
    }
}
