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

        [ColumnName("IdCliente")]
        [Required(ErrorMessage = "Debe seleccionar un cliente")]
        public int? IdCliente { get; set; }

        [ColumnName("CodigoCliente")]
        public string CodigoCliente { get; set; }

        [ColumnName("ClienteRazonSocial")]
        public string ClienteRazonSocial { get; set; }

        [ColumnName("CodPedido")]
        public string CodPedido { get; set; }

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
