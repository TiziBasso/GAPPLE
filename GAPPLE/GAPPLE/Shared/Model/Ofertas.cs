using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAPPLE.Shared.Model
{
    public class Oferta
    {
        public int IdOferta { get; set; }
        [Required(ErrorMessage = "Debe ingresar un nombre"), StringLength(50, ErrorMessage = "Máximo de caracteres 50")]
        public string Nombre { get; set; }
        [StringLength(250, ErrorMessage = "Máximo de caracteres 250")]
        public string? Descripcion { get; set; }
        [Required(ErrorMessage = "Debe ingresar una linea")]
        public string Linea { get; set; }
        [Required(ErrorMessage = "Debe ingresar un valor"),
            Range(0.01, 100, ErrorMessage = "Debe ingresar un número valido")]
        public decimal Descuento { get; set; }
        [Required(ErrorMessage = "Debe ingresar una fecha desde")]
        public DateTime Desde { get; set; } = DateTime.Today;
        [Required(ErrorMessage = "Debe ingresar una fecha hasta")]
        public DateTime Hasta {  get; set; } = DateTime.Today;
        public bool Activa { get; set; }
        public string? Inclusiones { get; set; }
        public string? Exclusiones { get; set; }
    }
}
