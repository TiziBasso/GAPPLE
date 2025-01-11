using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAPPLE.Shared.Model
{
    public class OrdenExpedicion
    {
        public string IdPedidos { get; set; }

        public string Orden { get; set; }

        public DateTime FechaEntrega { get; set; }

        public DateTime Fecha { get; set; }

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

        public string Observaciones { get; set; }

        public int Articulos { get; set; }

        public bool Impreso { get; set; }

        public List<OrdenExpedicionDetalle> Detalle { get; set; }
    }

    public class OrdenExpedicionDetalle
    {
        public int NumLinea { get; set; }

        public string IdProducto { get; set; }

        public string CodProducto { get; set; }

        public string DescripcionProducto { get; set; }

        public int CantidadF { get; set; }

        public int CantidadX { get; set; }

        public int Cantidad { get => CantidadF + CantidadX; }

        public int CantidadAprobadaX { get; set; }

        public int CantidadAprobadaF { get; set; }

        public int CantidadAprobada { get => CantidadAprobadaF + CantidadAprobadaX; }

        public int CantidadCanceladaX { get; set; }

        public int CantidadCanceladaF { get; set; }

        public int CantidadCancelada { get => CantidadCanceladaF + CantidadCanceladaX; }

        public int CantidadProbadorF { get; set; }

        public int CantidadProbadorX { get; set; }

        public int CantidadProbador { get => CantidadProbadorF + CantidadProbadorX; }

        public int CantidadProbadorCanceladaX { get; set; }

        public int CantidadProbadorCanceladaF { get; set; }

        public int CantidadProbadorCancelada { get => CantidadProbadorCanceladaF + CantidadProbadorCanceladaX; }
    }
}
