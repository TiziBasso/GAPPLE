using GAPPLE.Shared.Helpers;
using System.ComponentModel.DataAnnotations;

namespace GAPPLE.Shared.Model
{
    public class Acuerdo : RegistroUsuario
    {
        [ColumnName("IdAcuerdo")]
        public int IdAcuerdo { get; set; }

        [ColumnName("IdCliente")]
        [Required(ErrorMessage = "Debe seleccionar un cliente")]
        public int? IdCliente { get; set; }

        [ColumnName("CodigoCliente")]
        public string CodigoCliente { get; set; }

        [ColumnName("RazonSocial")]
        public string RazonSocial { get; set; }

        [ColumnName("CUIT")]
        public string CUIT { get; set; }

        [ColumnName("Linea")]
        [Required(ErrorMessage = "Debe seleccionar una linea")]
        public string Linea { get; set; }

        [ColumnName("Condicion")]
        public string Condicion { get; set; }

        [ColumnName("FechaDesde")]
        public DateTime? FechaDesde { get; set; }

        [ColumnName("FechaHasta")]
        public DateTime? FechaHasta { get; set; }

        [ColumnName("Activo")]
        public bool Activo { get; set; }
    }
}
