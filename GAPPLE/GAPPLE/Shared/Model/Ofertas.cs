using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAPPLE.Shared.Model
{
    public class Oferta
    {
        public int IdOferta { get; set; }
        public string Nombre { get; set; }
        public string Linea { get; set; }
        public decimal Descuento { get; set; }
        public DateTime Desde { get; set; }
        public DateTime Hasta {  get; set; }
        public bool Activa { get; set; }
        public string? Inclusiones { get; set; }
        public string? Exclusiones { get; set; }
    }
}
