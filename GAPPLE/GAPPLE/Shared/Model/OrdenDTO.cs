namespace GAPPLE.Shared.Model
{
    public class OrdenDTO
    {
        public OrdenDTO() { }
        public OrdenDTO(Orden orden)
        {
            Id = orden.Id;
            CodigoOrden = orden.CodigoOrden;
            IdEstado = orden.IdEstado;
            DescripcionEstado = orden.DescripcionEstado;
            Usuario = orden.Usuario;
            AprobadoVentas = orden.AprobadoVentas;
            AprobadoFinanzas = orden.AprobadoFinanzas;
            AprobadoContaduria = orden.AprobadoContaduria;
            Creacion = orden.Creacion;
            FechaEntrega = orden.FechaEntrega;
            Linea = orden.Linea;
            CodListaPrecio = orden.CodListaPrecio;
            Factura = orden.Factura;
            Presupuesto = orden.Presupuesto;
        }
        public int Id { get; set; }
        public string CodigoOrden { get; set; }
        public int? IdEstado { get; set; }
        public string DescripcionEstado { get; set; }
        public string Usuario { get; set; } = null;
        public bool AprobadoVentas { get; set; }
        public bool AprobadoFinanzas { get; set; }
        public bool AprobadoContaduria { get; set; }
        public DateTime? Creacion { get; set; }
        public DateTime? FechaEntrega { get; set; }
        public string Linea { get; set; }
        public string CodListaPrecio { get; set; }
        public string ObservacionCancelacion { get; set; }
        public bool Factura { get; set; }
        public bool Presupuesto { get; set; }
        public string EdicionUsuario { get; set; }

        //public string TipoIVA { get; set; } = "RI";
        //public string CUITCliente { get; set; }
        //public int? ID_GVA01 { get; set; }
        //public int? ID_GVA14 { get; set; }
        //public int? ID_GVA24 { get; set; }
        //public int? ID_GVA10 { get; set; }
        //public int? ID_GVA23 { get; set; }
        //public int? ID_STA22 { get; set; } = 11;
        //public string CodigoOrdenOriginal { get; set; }
        //public List<OrdenDetalle> Detalle { get; set; } = new();
        //public string Cliente { get; set; }
        //public string CodCliente { get; set; }
        //public string DomicilioCliente { get; set; }
        //public string TipoIVADescripcion
        //{
        //    get
        //    {
        //        return TipoIVA switch
        //        {
        //            "RI" => "Responsable Inscripto",
        //            "EX" => "Exento",
        //            "CF" => "Consumidor Final",
        //            "MT" => "Monotributista",
        //            _ => null,
        //        };
        //    }
        //}
        //public string Transporte { get; set; }
        //public string CodTransporte { get; set; }
        //public string Zona { get; set; }
        //public string ZonaDescripcion { get; set; }
        //public string CondicionVenta { get; set; }
        //public string CodVendedor { get; set; }
        //public List<int> Ofertas { get; set; } = new();
        //public string Entrega { get; set; }
        //public string Notas { get; set; }
        //public string NROTANGO { get; set; }
        //public string NumeroFactura { get; set; }
        //public int Unidades { get; set; }
        //public bool Aprobado { get; set; }
        //public bool TieneOrdenDoble { get; set; }
        //public string ObservacionesZentra { get; set; }
    }
}
