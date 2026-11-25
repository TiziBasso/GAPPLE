namespace GAPPLE.Shared.Requests
{
    /// <summary>
    /// Configuracion completa de listas de precios de un cliente: reemplaza todo lo guardado para ese cliente.
    /// </summary>
    public class ListasPrecioClienteRequest
    {
        public int IdCliente { get; set; }
        public string Usuario { get; set; }
        public List<ListasPrecioVendedor> Vendedores { get; set; } = new();
    }

    public class ListasPrecioVendedor
    {
        public int IdUsuario { get; set; }
        public List<int> IdListas { get; set; } = new();
    }

    /// <summary>
    /// Excel de lista de precios subido. Con IdListaPrecio reemplaza los precios de esa lista; sin el, crea una nueva.
    /// </summary>
    public class ListaPrecioArchivoRequest
    {
        public int? IdListaPrecio { get; set; }
        public string Nombre { get; set; }
        public DateTime? VigenciaDesde { get; set; }
        public string NombreArchivo { get; set; }
        public byte[] Archivo { get; set; }
        public string Usuario { get; set; }
    }
}
