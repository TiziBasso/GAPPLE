
using Newtonsoft.Json;

namespace GAPPLE.Shared.Model
{
    public class Cliente
    {
        public Cliente(string codCliente, string razonSocial, string cuit) => (CodigoCliente, RazonSocial, CUIT) = (codCliente, razonSocial, cuit);
        public int? IdCliente { get; set; }
        [JsonProperty("COD_GVA14")]
        public string CodigoCliente { get; set; }
        [JsonProperty("RAZON_SOCI")]
        public string RazonSocial { get; set; }
        [JsonProperty("NOM_COM")]
        public string NombreComercial { get; set; }
        [JsonProperty("CUIT")]
        public string CUIT { get; set; }
        [JsonProperty("CLASIFICACION")]
        public string? Clasificacion { get; set; }
        public bool ClienteEspecial { get; set; }
        public string? Observaciones { get; set; }
    }
}
