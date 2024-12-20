using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAPPLE.Shared.Model
{
    public class IssueGroup
    {
        public int Count { get; set; }
        public DateTime Week { get; set; }
    }

    public class LabelGroup
    {
        public int Count { get; set; }
        public string Label { get; set; }
        public string Color { get; set; }
    }

    public class UserGroup
    {
        public int Count { get; set; }
        public string Name { get; set; }
    }

    public class OrdenDashboard
    {
        public string CodigoOrden { get; set; }
        public DateTime? AltaRegistro { get; set; }
        public DateTime? FechaAprobacion { get; set; }
    }
}
