namespace GAPPLE.Shared.Helpers
{
    /// <summary>
    /// Descuentos con valor fijo definidos por negocio.
    /// </summary>
    public static class DescuentosHelper
    {
        /// <summary>
        /// Descuento con el que se carga todo producto complemento: el complemento acompaña al
        /// producto principal y no se factura, por eso entra bonificado al 99,99%.
        /// </summary>
        public const decimal Complemento = 99.99m;
    }
}
