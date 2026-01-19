using GAPPLE.Shared.Enums;
using GAPPLE.Shared.Helpers;
using System.ComponentModel.DataAnnotations;

namespace GAPPLE.Shared.Model
{
    public class ComprobanteCabecera : RegistroUsuario
    {
        private decimal? _importeTotal;

        [ColumnName("IdComprobante")]
        public int IdComprobante { get; set; }

        [ColumnName("CodigoOrden")]
        public string CodigoOrden { get; set; }

        [ColumnName("CodigoTango")]
        public string CodigoTango { get; set; }

        [ColumnName("TipoComprobante")]
        public string TipoComprobante { get; set; }

        [ColumnName("IdCliente"), Required(ErrorMessage = "Debe seleccionar un cliente")]
        public int? IdCliente { get; set; }

        [ColumnName("CodCliente")]
        public string CodCliente { get; set; }

        [ColumnName("ClienteRazonSocial")]
        public string ClienteRazonSocial { get; set; }

        [ColumnName("ClienteCuit")]
        public string ClienteCuit { get; set; }

        [ColumnName("ClienteCategoriaIVA")]
        public string ClienteCategoriaIVA { get; set; }

        [ColumnName("IdMotivo"), Required(ErrorMessage = "Debe seleccionar un motivo")]
        public int? IdMotivo { get; set; }

        [ColumnName("Motivo")]
        public string MotivoDescripcion { get; set; }

        [ColumnName("IdDeposito")]
        public int? IdDeposito { get; set; }

        [ColumnName("Deposito")]
        public string DepositoDescripcion { get; set; }

        [ColumnName("IdEstado")]
        public ComprobanteCabeceraEstadoEnum IdEstado { get; set; }

        [ColumnName("Estado")]
        public string EstadoDescripcion { get; set; }

        /// <summary>
        /// Null = No ingresa, False = Pendiente de ingreso mercadería, True = Mercadería ingresada
        /// </summary>
        [ColumnName("MercaderiaIngresada")]
        public bool? MercaderiaIngresada { get; set; }

        [ColumnName("Fecha")]
        public DateTime FechaComprobante { get; set; }

        [ColumnName("Observaciones"), MaxLength(50, ErrorMessage = "La longitud máxima es de 50 caracteres")]
        public string Observaciones { get; set; }

        [ColumnName("TipoComprobanteReferencia")]
        public string TipoComprobanteReferencia { get; set; }

        [ColumnName("ComprobanteReferencia"), MaxLength(20, ErrorMessage = "La longitud máxima es de 20 caracteres")]
        public string ComprobanteReferencia { get; set; }

        [ColumnName("ImporteTotal")]
        public decimal ImporteTotal
        {
            get
            {
                if (_importeTotal != null)
                    return (decimal)_importeTotal;

                return Detalle.Sum(x => x.PrecioTotal);
            }
            set => _importeTotal = value;
        }

        [ColumnName("IdListaPrecio"), Required(ErrorMessage = "Debe seleccionar una lista de precio")]
        public int? IdListaPrecio { get; set; }

        [ColumnName("CodListaPrecio")]
        public string CodListaPrecio { get; set; }

        [ColumnName("DescripcionListaPrecio")]
        public string DescripcionListaPrecio { get; set; }

        public bool Factura { get; set; }

        public bool Presupuesto { get; set; }

        public List<ComprobanteDetalle> Detalle { get; set; } = [];
    }
}