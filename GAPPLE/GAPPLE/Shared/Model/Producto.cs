using System.ComponentModel.DataAnnotations;

namespace GAPPLE.Shared.Model
{
    public class Producto : RegistroUsuario
    {
        public int IdProducto { get; set; }

        public string CodigoInterno { get; set; }

        [Required(ErrorMessage = "Debe ingresar una descripción"),
            StringLength(50, ErrorMessage = "Máximo de caracteres 50")]
        public string Descripcion { get; set; }

        public bool Pasivo { get; set; }

        [Required(ErrorMessage = "Debe ingresar un valor"),
            DisplayFormat(DataFormatString = "{0:N2}"),
            Range(0.01, 99999.99, ErrorMessage = "Debe ingresar un número valido")]
        public decimal? PorcentajeIVA { get; set; }

        [StringLength(2000, ErrorMessage = "Máximo de caracteres 2000")]
        public string Observaciones { get; set; }

        [Required(ErrorMessage = "Debe seleccionar una marca"),
            Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar una marca")]
        public int IdMarca { get; set; }

        [Required(ErrorMessage = "Debe seleccionar una familia")]
        public string IdFamilia { get; set; }

        [Required(ErrorMessage = "Debe ingresar un valor"),
            DisplayFormat(DataFormatString = "{0:N2}"),
            Range(0.01, 99999.99, ErrorMessage = "Debe ingresar un número valido")]
        public decimal? Precio { get; set; }
    }
}
