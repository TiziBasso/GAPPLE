using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAPPLE.Shared.Model
{
    public class ProductosComplementos
    {
        public string? CodigoPrincipal { get; set; }

        public string? DescripcionPrincipal { get; set; }

        public string? LineaPrincipal { get; set; }

        public string? CodigoRelacionado { get; set; }

        public string? DescripcionRelacionado { get; set; }

        public string? LineaRelacionado { get; set; }
    }
}
