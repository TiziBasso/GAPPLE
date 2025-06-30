using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAPPLE.Shared.Model
{
    public class ByteArrayRequest
    {
        public byte[] File { get; set; }
        public string ConnectionId { get; set; }
        public string CodCliente { get; set; }
        public string CodListaPrecio { get; set; }
        public string OfertasSeleccionadas { get; set; }
    }
}
