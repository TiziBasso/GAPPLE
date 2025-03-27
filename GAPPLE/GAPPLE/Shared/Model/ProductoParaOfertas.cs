using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAPPLE.Shared.Model
{
    public class ProductoParaOfertas : ICloneable
    {
        public string Familia { get; set; }
        public string CodigoProducto { get; set; }
        public string Descripcion { get; set; }
        public bool Probador { get; set; }
        public bool ModificadoManual { get; set; }
        public int CantidadProbador { get; set; }
        public int CantidadSeleccionada { get; set; }
        public decimal DescuentoFinal { get; set; }

        public object Clone() => MemberwiseClone();
    }
}
