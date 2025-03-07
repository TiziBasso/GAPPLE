using System.ComponentModel.DataAnnotations;

namespace GAPPLE.Shared.Model
{
    public class Producto : RegistroUsuario
    {
        public int Id_STA { get; set; }

        public int IdProducto { get; set; }

        public string? CodigoProducto { get; set; }

        public string? Descripcion { get; set; }

        public bool Pasivo { get; set; }
        public int Orden { get; set; }

        public decimal? PorcentajeIVA { get; set; }

        public string? Observaciones { get; set; }

        public int IdMarca { get; set; }

        public string? Clasificacion { get; set; }

        public string? Linea { get; set; }

        public decimal? Precio { get; set; }
    }
}
