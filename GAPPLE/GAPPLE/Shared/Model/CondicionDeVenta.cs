using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAPPLE.Shared.Model
{
    public class CondicionDeVenta
    {
        public int Id_GVA { get; set; }
        public int IdCondicionVenta { get; set; }
        public string CodigoTango { get; set; }
        public string Descripcion { get; set; }
    }
}
