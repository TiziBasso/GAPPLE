namespace GAPPLE.Shared.Model
{
    /// <summary>
    /// Una fila de la pantalla de Stock: un producto (codigo) con su saldo ya pivoteado
    /// por deposito. El SP prc_get_Stock agrupa las filas de dbo.Stock (que vienen una por
    /// producto y deposito) en una sola fila por codigo.
    /// </summary>
    public class StockConsolidado
    {
        public const string SinMarca = "(Sin marca)";

        /// <summary>Codigo / SKU del producto (columna <c>codigo</c> de dbo.Stock).</summary>
        public string Codigo { get; set; }

        public string Descripcion { get; set; }

        /// <summary>Marca comercial (columna <c>marca</c> de dbo.Stock).</summary>
        public string Marca { get; set; }

        /// <summary>Unidades en el deposito 01 (Avellaneda), disponibles para ventas.</summary>
        public decimal Dep01 { get; set; }

        /// <summary>Unidades en el deposito 6 (producto terminado), pendientes de envio al 01.</summary>
        public decimal Dep06 { get; set; }

        /// <summary>Unidades pendientes (columna <c>Cant_Pend</c>), sumadas entre ambos depositos.</summary>
        public decimal Pendientes { get; set; }

        /// <summary>Sumatoria automatica por fila: deposito 01 + deposito 6.</summary>
        public decimal TotalStock => Dep01 + Dep06;
    }
}
