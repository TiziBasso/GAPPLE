using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAPPLE.Shared.Model
{
    public class MenuNew : ICloneable
    {
        public int Id { get; set; }
        public int? IdPadre { get; set; }
        public string Text { get; set; }
        public string Path { get; set; }
        public string Icon { get; set; }
        public bool Expanded { get; set; }
        public int? IdMarcador { get; set; }
        public List<MenuNew> Items { get; set; }
        public object Clone() => MemberwiseClone();
    }
}
