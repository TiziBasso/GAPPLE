namespace GAPPLE.Shared.Model
{
    public class OrdenExpedicion
    {
        public string IdPedidos { get; set; }

        public string Orden { get; set; }

        public string LetrasOrden { get; set; }

        public DateTime? FechaEntrega { get; set; }

        public DateTime? Fecha { get; set; }

        public string Linea { get; set; }

        public string CodCliente { get; set; }

        public string CodTango { get; set; }

        public string RazonSocial { get; set; }

        public string Vendedor { get; set; }

        public string CUIT { get; set; }

        public string CondicionIVA { get; set; }

        public string EntregarEn { get; set; }

        public string Transporte { get; set; }

        public string Zona { get; set; }

        public string OrdenCompra { get; set; }

        public string ObservacionesZentra { get; set; }

        public int Articulos { get; set; }

        public bool Impreso { get; set; }

        public List<OrdenExpedicionDetalle> Detalle { get; set; }
    }

    public class OrdenExpedicionDetalle : ICloneable
    {
        public int NumLinea { get; set; }

        public string IdProducto { get; set; }

        public string CodProducto { get; set; }

        public string DescripcionProducto { get; set; }

        public int CantidadF { get; set; }

        public int CantidadX { get; set; }

        public int Cantidad { get => CantidadF + CantidadX; }

        public int CantidadPendienteX => CantidadX - CantidadAprobadaX;

        public int CantidadPendienteF => CantidadF - CantidadAprobadaF;

        public int CantidadPendiente { get => CantidadPendienteF + CantidadPendienteX; }

        public int CantidadAprobadaX { get; set; }

        public int CantidadAprobadaF { get; set; }

        public int CantidadAprobada { get => CantidadAprobadaF + CantidadAprobadaX; }

        public int CantidadProbadorF { get; set; }

        public int CantidadProbadorX { get; set; }

        public int CantidadProbador { get => CantidadProbadorF + CantidadProbadorX; }

        public int CantidadProbadorAprobadaX { get; set; }

        public int CantidadProbadorAprobadaF { get; set; }

        public int CantidadProbadorAprobada { get => CantidadProbadorAprobadaF + CantidadProbadorAprobadaX; }

        public int CantidadObsequioF { get; set; }

        public int CantidadObsequioX { get; set; }

        public int CantidadObsequio { get => CantidadObsequioF + CantidadObsequioX; }

        public int CantidadObsequioAprobadaX { get; set; }

        public int CantidadObsequioAprobadaF { get; set; }

        public int CantidadObsequioAprobada { get => CantidadObsequioAprobadaF + CantidadObsequioAprobadaX; }

        public int CantidadProbadorPendienteF => CantidadProbadorF - CantidadProbadorAprobadaF;

        public int CantidadProbadorPendienteX => CantidadProbadorX - CantidadProbadorAprobadaX;

        public int CantidadProbadorPendiente { get => CantidadProbadorPendienteF + CantidadProbadorPendienteX; }
        
        public int CantidadObsequioPendienteF => CantidadObsequioF - CantidadObsequioAprobadaF;
        
        public int CantidadObsequioPendienteX => CantidadObsequioX - CantidadObsequioAprobadaX;

        public int CantidadObsequioPendiente { get => CantidadObsequioPendienteF + CantidadObsequioPendienteX; }

        public bool HuboCambios { get; set; }

        public object Clone() => MemberwiseClone();
    }
}
