using GAPPLE.Shared.Helpers;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace GAPPLE.Shared.Model
{
    public class ReclamoDetalle : ICloneable
    {
        [ColumnName("IdReclamoDetalle")]
        public int IdReclamoDetalle { get; set; }

        [ColumnName("IdReclamo")]
        public int IdReclamo { get; set; }

        [ColumnName("SKU")]
        [Required(ErrorMessage = "El SKU es obligatorio")]
        public string SKU { get; set; }

        [ColumnName("DescripcionProducto")]
        public string DescripcionProducto { get; set; }

        [ColumnName("Cantidad")]
        [Range(1, int.MaxValue, ErrorMessage = "La cantidad debe ser mayor a cero")]
        public int Cantidad { get; set; } = 1;

        [ColumnName("Lote")]
        public string Lote { get; set; }

        [ColumnName("Vencimiento")]
        public DateTime? Vencimiento { get; set; }

        /// <summary>
        /// Número de línea transitorio para manejo de UI (no se persiste).
        /// </summary>
        [JsonIgnore]
        public int NumeroLinea { get; set; }

        public object Clone() => MemberwiseClone();
    }
}
