using GAPPLE.Shared.Helpers;
using System.ComponentModel.DataAnnotations;

namespace GAPPLE.Shared.Model
{
    public class AcuerdoMonto : RegistroUsuario, ICloneable
    {
        [ColumnName("Id")]
        public int Id { get; set; }

        [ColumnName("IdAcuerdo")]
        public int IdAcuerdo { get; set; }

        [ColumnName("IdCliente")]
        [Required(ErrorMessage = "Debe seleccionar un cliente")]
        public int? IdCliente { get; set; }

        [ColumnName("CodigoCliente")]
        public string CodigoCliente { get; set; }

        [ColumnName("RazonSocial")]
        public string RazonSocial { get; set; }

        [ColumnName("Linea")]
        [Required(ErrorMessage = "Debe seleccionar una linea")]
        public string Linea { get; set; }

        [ColumnName("Condicion")]
        public string Condicion { get; set; }

        [ColumnName("Fecha")]
        public DateTime Fecha { get; set; }

        [ColumnName("IdComprobante")]
        public int? IdComprobante { get; set; }

        [ColumnName("NumeroNC")]
        public string NumeroNC { get; set; }

        [ColumnName("IdPedido")]
        public int? IdPedido { get; set; }

        [ColumnName("Monto")]
        public decimal Monto { get; set; }

        [ColumnName("Notas")]
        public string Notas { get; set; }

        public object Clone() => MemberwiseClone();
    }
}
