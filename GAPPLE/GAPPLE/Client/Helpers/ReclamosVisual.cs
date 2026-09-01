using GAPPLE.Shared.Enums;

namespace GAPPLE.Client.Helpers
{
    /// <summary>
    /// Etiquetas y colores compartidos por las vistas de reclamos (dashboard).
    /// </summary>
    public static class ReclamosVisual
    {
        public static string TipoLabel(ReclamoTipoEnum tipo) => tipo switch
        {
            ReclamoTipoEnum.Grave => "Grave",
            ReclamoTipoEnum.Moderado => "Moderado",
            ReclamoTipoEnum.Leve => "Leve",
            _ => "—"
        };

        public static string TipoColor(ReclamoTipoEnum tipo) => tipo switch
        {
            ReclamoTipoEnum.Grave => "#ef4444",
            ReclamoTipoEnum.Moderado => "#f59e0b",
            ReclamoTipoEnum.Leve => "#3b82f6",
            _ => "#94a3b8"
        };

        /// <summary>Gravedades ordenadas de menor a mayor severidad, como en el mock.</summary>
        public static readonly ReclamoTipoEnum[] Gravedades =
        [
            ReclamoTipoEnum.Leve,
            ReclamoTipoEnum.Moderado,
            ReclamoTipoEnum.Grave
        ];

        public static string MotivoLabel(ReclamoMotivoEnum motivo) => motivo switch
        {
            ReclamoMotivoEnum.Faltantes => "Faltante",
            ReclamoMotivoEnum.ProblemaDeCalidad => "Problema de calidad",
            ReclamoMotivoEnum.ProblemaDeEmpaque => "Problema de empaque",
            ReclamoMotivoEnum.ErrorEnvio => "Error de envío",
            ReclamoMotivoEnum.ProductoIncompleto => "Producto incompleto",
            ReclamoMotivoEnum.FaltanteCDA => "Faltantes CDA",
            ReclamoMotivoEnum.ErrorCargaVendedor => "Error carga vendedor",
            ReclamoMotivoEnum.ErrorAdministrativo => "Error administrativo",
            ReclamoMotivoEnum.Vencidos => "Vencidos",
            ReclamoMotivoEnum.BultosCruzados => "Bultos cruzados",
            ReclamoMotivoEnum.CortoVencimiento => "Corto vencimiento",
            ReclamoMotivoEnum.Devolucion => "Devolución",
            ReclamoMotivoEnum.ErrorFabricacion => "Error de fabricación",
            ReclamoMotivoEnum.ErrorCliente => "Error cliente",
            _ => "Otros"
        };

        /// <summary>Paleta para el gráfico de dona: una entrada por motivo, en orden de uso.</summary>
        public static readonly string[] Paleta =
        [
            "#f97316", "#ef4444", "#eab308", "#3b82f6", "#a855f7", "#14b8a6",
            "#ec4899", "#84cc16", "#6366f1", "#f59e0b", "#0ea5e9", "#8b5cf6",
            "#10b981", "#64748b", "#cbd5e1"
        ];

        public static string ColorMarca(int indice) => Paleta[indice % Paleta.Length];
    }
}
