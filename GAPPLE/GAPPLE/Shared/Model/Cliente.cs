
using Newtonsoft.Json;

namespace GAPPLE.Shared.Model
{
    public class Cliente
    {
        public int Id_GVA { get; set; }
        public int? IdCliente { get; set; }
        //[JsonProperty("COD_GVA14")]
        public string? CodigoCliente { get; set; }
        //[JsonProperty("RAZON_SOCI")]
        public string? RazonSocial { get; set; }
        //[JsonProperty("NOM_COM")]
        public string? NombreComercial { get; set; }
        //[JsonProperty("CUIT")]
        public string? CUIT { get; set; }
        //[JsonProperty("CLASIFICACION")]
        public string? Clasificacion { get; set; }
        public bool ClienteEspecial { get; set; }
        public bool Activa { get; set; }
        public string? Observaciones { get; set; }
        public string? CodListaPrecioDefault { get; set; }
        public string? CondVentaDefault { get; set; }
        public string? ZonaDefault { get; set; }
    }
}
