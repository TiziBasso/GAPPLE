using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAPPLE.Shared.Model
{
    public class PedidoCabecera
    {
        public int IdPedido { get; set; }   
        public string Linea { get; set; }
        public string CodigoCliente { get; set; }
        public int Zona { get; set; }
        public int ListaDePrecios { get; set; }
        public bool Factura {  get; set; }
        public bool FacturaMitad {  get; set; }
        public string Transporte { get; set; }
        public int CondicionVenta { get; set; }
        public string EntregarEn {  get; set; }
        public bool Probadores {  get; set; }
        public bool Obsequios_CartasColores_DescExtra {  get; set; }
        public bool Multitester_Exhibidor {  get; set; }
        public string Observaciones {  get; set; }
        public List<Oferta> Ofertas { get; set; }
        public DateTime FechaEntrega { get; set; }
    }

    public class PedidoDetalle
    {
        public int IdProducto { get; set; }
        public int Cantidad { get; set; }
    }
}
