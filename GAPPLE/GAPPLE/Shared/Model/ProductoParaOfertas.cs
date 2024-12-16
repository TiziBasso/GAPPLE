using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAPPLE.Shared.Model
{
    public class ProductoParaOfertas
    {
        public string Familia { get; set; }
        public string CodigoProducto { get; set; }
        public string Descripcion { get; set; }
        public bool Selected { get; set; }
    }
}
