using GAPPLE.Shared.Enums;

namespace GAPPLE.Shared.Model
{
    /// <summary>
    /// Una linea de detalle de reclamo aplanada con los datos de la cabecera y la marca
    /// (Linea) del producto. Es la unidad minima con la que el dashboard de reclamos
    /// calcula todas sus metricas: casos (por IdReclamo distinto) y unidades (por Cantidad).
    /// </summary>
    public class ReclamoDashboardLinea
    {
        public const string SinMarca = "(Sin marca)";

        public int IdReclamo { get; set; }

        public DateTime Fecha { get; set; }

        public string CodigoCliente { get; set; }

        public string RazonSocial { get; set; }

        public ReclamoTipoEnum Tipo { get; set; }

        public ReclamoMotivoEnum Motivo { get; set; }

        public string SKU { get; set; }

        public int Cantidad { get; set; }

        /// <summary>
        /// Marca comercial del producto. Se resuelve en el server cruzando el SKU contra
        /// el maestro de productos (campo Linea). Si el SKU no existe se usa <see cref="SinMarca"/>.
        /// </summary>
        public string Marca { get; set; }
    }
}
