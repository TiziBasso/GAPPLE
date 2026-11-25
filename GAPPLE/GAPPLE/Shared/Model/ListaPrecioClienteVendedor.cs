namespace GAPPLE.Shared.Model
{
    /// <summary>
    /// Lista de precios asignada a un cliente para un vendedor puntual.
    /// </summary>
    public class ListaPrecioClienteVendedor
    {
        /// <summary>0 = la lista no esta asignada a ningun cliente (solo se le muestra al administrador).</summary>
        public int IdCliente { get; set; }
        public string CodigoCliente { get; set; }
        public string RazonSocial { get; set; }
        public int IdUsuario { get; set; }
        public string ApellidoYNombre { get; set; }
        public int IdListaPrecio { get; set; }
        public string NombreLista { get; set; }
        public DateTime? VigenciaDesde { get; set; }
    }
}
