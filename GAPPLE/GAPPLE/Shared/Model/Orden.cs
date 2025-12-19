using System.Collections;
using System.ComponentModel.DataAnnotations;

namespace GAPPLE.Shared.Model
{
    public class Orden
    {
        public int Id { get; set; }

        public int? ID_GVA01 { get; set; }
        public int? ID_GVA14 { get; set; }
        public int? ID_GVA24 { get; set; }
        public int? ID_GVA10 { get; set; }
        public int? ID_GVA23 { get; set; }
        public int? ID_STA22 { get; set; } = 11;

        public string? CodigoOrden { get; set; }
        public string? CodigoOrdenOriginal { get; set; }

        [Required(ErrorMessage = "Debe ingresar una linea")]
        public string? Linea { get; set; }

        public bool Factura { get; set; }

        public bool Presupuesto { get; set; }

        public List<OrdenDetalle>? Detalle { get; set; } = new();

        public string? Cliente { get; set; }

        [Required(ErrorMessage = "Debe ingresar un cliente")]
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

        [Required(ErrorMessage = "Debe ingresar una lista de precios")]
        public string CodListaPrecio { get; set; }

        public string? Zona { get; set; }
        public string? ZonaDescripcion { get; set; }

        [Required(ErrorMessage = "Debe ingresar una condición de venta")]
        public string? CondicionVenta { get; set; }

        public string? CodVendedor { get; set; }

        public List<int>? Ofertas { get; set; } = new();

        [StringLength(60, ErrorMessage = "El maximo de caracteres son 60")]
        [Required(ErrorMessage = "Debe ingresar una dirección de entrega")]
        public string? Entrega { get; set; }
        [StringLength(60, ErrorMessage = "El maximo de caracteres son 60")]

        public string? Notas { get; set; }

        public DateTime? FechaEntrega { get; set; }

        public DateTime? Creacion { get; set; }

        public int? IdEstado { get; set; }

        public string? DescripcionEstado { get; set; }

        public string? NROTANGO { get; set; }

        public string? NumeroFactura { get; set; }

        public int Unidades { get; set; }

        public bool Aprobado { get; set; }

        public string Usuario { get; set; } = null;

        public bool AprobadoVentas { get; set; }

        public bool AprobadoFinanzas { get; set; }

        public bool AprobadoContaduria { get; set; }

        public bool TieneOrdenDoble { get; set; }
        public string? ObservacionesZentra { get; set; }
        public string? ObservacionesCancelacion { get; set; }
        public string CodigoTangoNormal { get; set; }
        public string CodigoTangoProbador { get; set; }
        public string CodigoTangoObsequio { get; set; }
    }
    public class NotEmptyAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (value is ICollection collection)
            {
                return collection.Count > 0;
            }
            return false; // No es una colección válida
        }

        public override string FormatErrorMessage(string name)
        {
            return ErrorMessage ?? $"{name} no debe estar vacía.";
        }
    }

    public class OrdenDetalle
    {
        public int Id { get; set; }
        public int ID_STA11 { get; set; }

        public int NumeroLinea { get; set; }

        public string? Descripcion { get; set; }

        public int IdProducto { get; set; }

        public string? CodProducto { get; set; }
        public decimal Precio { get; set; }

        public int Cantidad { get; set; }

        public int CantidadAprobada { get; set; }

        public int CantidadPendiente => Cantidad - CantidadAprobada;

        public bool Probador { get; set; }

        public int CantidadProbador { get; set; }

        public int CantidadProbadorAprobada { get; set; }

        public int CantidadProbadorPendiente => CantidadProbador - CantidadProbadorAprobada;

        public decimal Descuento { get; set; }

        public int CantidadObsequio { get; set; }

        public int CantidadObsequioAprobada { get; set; }

        public int CantidadObsequioPendiente => CantidadObsequio - CantidadObsequioAprobada;

        public bool TieneObsequio { get; set; }
    }
}