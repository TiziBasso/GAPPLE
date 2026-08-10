using GAPPLE.Shared.Enums;
using GAPPLE.Shared.Helpers;
using System.ComponentModel.DataAnnotations;

namespace GAPPLE.Shared.Model
{
    public class Reclamo : RegistroUsuario, ICloneable
    {
        [ColumnName("IdReclamo")]
        public int IdReclamo { get; set; }

        [ColumnName("Fecha")]
        public DateTime Fecha { get; set; } = DateTime.Today;

        [ColumnName("CodigoCliente")]
        [Required(ErrorMessage = "Debe seleccionar un cliente")]
        public string CodigoCliente { get; set; }

        [ColumnName("RazonSocial")]
        public string ClienteRazonSocial { get; set; }

        [ColumnName("NumeroFactura")]
        [MaxLength(50, ErrorMessage = "El número de NC no puede superar los 50 caracteres")]
        public string NumeroFactura { get; set; }

        [ColumnName("NFAC")]
        public string NFAC { get; set; }

        [ColumnName("Tipo")]
        [Required(ErrorMessage = "Debe seleccionar el tipo de reclamo")]
        public ReclamoTipoEnum? Tipo { get; set; }

        [ColumnName("Motivo")]
        [Required(ErrorMessage = "Debe seleccionar el motivo del reclamo")]
        public ReclamoMotivoEnum? Motivo { get; set; }

        [ColumnName("Descripcion")]
        [MaxLength(2000, ErrorMessage = "La descripción no puede superar los 2000 caracteres")]
        public string Descripcion { get; set; }

        [ColumnName("Resolucion")]
        [MaxLength(2000, ErrorMessage = "La resolución no puede superar los 2000 caracteres")]
        public string Resolucion { get; set; }

        public List<ReclamoDetalle> Detalle { get; set; } = [];

        public object Clone()
        {
            var clon = (Reclamo)MemberwiseClone();
            clon.Detalle = [];
            return clon;
        }
    }
}
