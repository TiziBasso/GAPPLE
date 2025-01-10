using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAPPLE.Shared.Model
{
    public class OrdenExpedicion
    {
        public int IdPedido { get; set; }

        public string Ordenes { get; set; }

        public DateTime Fecha { get; set; }

        public string Linea { get; set; }

        public string CodCliente { get; set; }

        public string RazonSocial { get; set; }

        public int Articulos { get; set; }

        public DateTime FechaImpresion { get; set; }
    }

    public class OrdenExpedicionDetalle
    {
        public int NumLinea { get; set; }

        public string CodProducto { get; set; }

        public string DescripcionProducto { get; set; }

        public int Cantidad { get; set; }

        public int CantidadAprobada { get; set; }

        public int CantidadCancelada { get; set; }

        public int CantidadPendiente => Cantidad - CantidadAprobada - CantidadCancelada;

        public int CantidadProbador { get; set; }
    }
}
