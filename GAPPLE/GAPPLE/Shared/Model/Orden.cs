using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAPPLE.Shared.Model
{
    public class Orden
    {
        public int Id { get; set; }

        public int CodigoOrden { get; set; }

        public string? Linea { get; set; }

        public bool Factura { get; set; }

        public bool Presupuesto { get; set; }

        public bool Probadores { get; set; }

        public bool Obsequios { get; set; }

        public bool Exhibidor { get; set; }

        public List<OrdenDetalle>? Detalle { get; set; }

        [Required(ErrorMessage = "Debe ingresar un cliente")]
        public string? Cliente { get; set; }

        public string? CodCliente { get; set; }

        public string? DomicilioCliente { get; set; }

        public string? CUITCliente { get; set; }

        public string? TipoIVA { get; set; } = "RI";

        public string? TipoIVADescripcion
        {
            get
            {
                return TipoIVA switch
                {
                    "RI" => "Responsable Inscripto",
                    "EX" => "Exento",
                    "CF" => "Consumidor Final",
                    "MT" => "Monotributista",
                    _ => null,
                };
            }
        }

        public string? Transporte { get; set; }

        public string? CodTransporte { get; set; }

        public string CodListaPrecio { get; set; }

        public string? Zona { get; set; }

        public string? CondicionVenta { get; set; }

        public string? CodVendedor { get; set; }

        public List<int>? Ofertas { get; set; }

        public string? Entrega { get; set; }

        public string? Notas { get; set; }

        public DateTime? FechaEntrega { get; set; }

        public DateTime? Creacion { get; set; }

        public int? IdEstado { get; set; }

        public string? DescripcionEstado { get; set; }

        public string? IdTango { get; set; }

        public string? NumeroFactura { get; set; }

        public int Unidades { get; set; }

        public bool Aprobado { get; set; }
    }

    public class OrdenDetalle
    {
        public int Id { get; set; }

        public int NumeroLinea { get; set; }

        public string? Descripcion { get; set; }

        public int IdProducto { get; set; }

        public string? CodProducto { get; set; }

        public int Cantidad { get; set; }

        public int CantidadAprobada { get; set; }

        public int CantidadPendiente => Cantidad - CantidadAprobada;

        public decimal Descuento1 { get; set; }

        public decimal Descuento2 { get; set; }

        public decimal TotalDescuento
        {
            get
            {
                decimal factor1 = 1 - (Descuento1 / 100);
                decimal factor2 = 1 - (Descuento2 / 100);
                decimal descuentoFinal = 1 - (factor1 * factor2);
                return descuentoFinal * 100;
            }
        }

    }
}