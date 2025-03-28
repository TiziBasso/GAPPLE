using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAPPLE.Shared.Model
{
    public class SucursalesPorCliente
    {
        public string CodCliente { get; set; }
        public string Direccion { get; set; }
        public string Localidad { get; set; }
        public bool Habitual { get; set; }
        public string CodigoPostal { get; set; }
        public string DireccionCompleta
        {
            get
            {
                return $"{Direccion}, {Localidad}, {CodigoPostal}";
            }
        }
    }
}
