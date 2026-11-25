namespace GAPPLE.Shared.Model
{
    /// <summary>
    /// Lista de precios cargada desde un Excel (ej. "Lista 174"). No es una lista de Tango: sus precios viven en GAPPLE
    /// y se reemplazan cada vez que se sube una nueva version del Excel.
    /// </summary>
    public class ListaPrecioImportada
    {
        public int IdListaPrecio { get; set; }
        public string Nombre { get; set; }
        public DateTime? VigenciaDesde { get; set; }
        public string NombreArchivo { get; set; }
        public int CantidadArticulos { get; set; }
        public int CantidadClientes { get; set; }
        public string UsuarioActualizacion { get; set; }
        public DateTime? FechaActualizacion { get; set; }
    }

    /// <summary>
    /// Un articulo de una lista de precios cargada desde Excel.
    /// </summary>
    public class ListaPrecioImportadaItem
    {
        public int Orden { get; set; }
        public string Familia { get; set; }
        public string SKU { get; set; }
        public string Descripcion { get; set; }
        public string Tono { get; set; }
        public string EAN { get; set; }
        public string Contenido { get; set; }
        public int? UnidadesPorCaja { get; set; }
        public decimal? CostoNetoSinIva { get; set; }
        public decimal? CostoNetoConIva { get; set; }
        public decimal? PrecioSugeridoPublico { get; set; }
    }

    /// <summary>
    /// Resultado de leer un Excel de lista de precios, antes de guardarlo.
    /// </summary>
    public class ListaPrecioPrevisualizacion
    {
        public string Nombre { get; set; }
        public DateTime? VigenciaDesde { get; set; }
        public int CantidadFamilias { get; set; }
        public List<ListaPrecioImportadaItem> Items { get; set; } = new();
        public List<string> Advertencias { get; set; } = new();
    }
}
