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

        /// <summary>
        /// Alicuota de IVA que se suma al total de las notas de credito tipo "Factura".
        /// </summary>
        public const decimal IVA = 21m;

        /// <summary>
        /// Neto mas IVA, redondeado a 2 decimales: es el total que se guarda en una NC Factura con IVA.
        /// </summary>
        public static decimal ConIVA(decimal neto) => Math.Round(neto * (1 + IVA / 100), 2);
    }
}
