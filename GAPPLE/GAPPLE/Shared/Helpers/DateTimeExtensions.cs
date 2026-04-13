namespace GAPPLE.Shared.Helpers
{
    public static class DateTimeExtensions
    {
        /// <summary>
        /// Devuelve el último instante del día para la fecha especificada
        /// (23:59:59.9999999).  
        /// Si el valor es <c>null</c>, retorna <c>null</c>.
        /// </summary>
        /// <param name="date">Fecha base sobre la cual calcular el fin del día.</param>
        /// <returns>
        /// Un <see cref="DateTime"/> con el último tick del día,
        /// o <c>null</c> si la fecha es <c>null</c>.
        /// </returns>
        public static DateTime? EndOfDay(this DateTime? date) =>
                    date.HasValue
                    ? date.Value.Date.AddDays(1).AddTicks(-1)
                    : null;

        /// <summary>
        /// Devuelve el último instante del día para la fecha especificada
        /// (23:59:59.9999999).  
        /// </summary>
        /// <param name="date">Fecha base sobre la cual calcular el fin del día.</param>
        /// <returns>
        /// Un <see cref="DateTime"/> con el último tick del día.
        /// </returns> 
        public static DateTime EndOfDay(this DateTime date) =>
                    date.Date.AddDays(1).AddTicks(-1);

        /// <summary>
        /// Devuelve una nueva instancia de <see cref="DateTime"/> que representa
        /// el primer día del mes de la fecha especificada, a las 00:00:00.
        /// </summary>
        /// <param name="date">
        /// Fecha base desde la cual se calcula el inicio del mes.
        /// </param>
        /// <returns>
        /// Un <see cref="DateTime"/> correspondiente al primer día del mes.
        /// </returns>
        public static DateTime FirstDayOfMonth(this DateTime date) =>
                    new(date.Year, date.Month, 1);

        /// <summary>
        /// Devuelve una nueva instancia de <see cref="DateTime"/> que representa
        /// el último día del mes de la fecha especificada, a las 00:00:00.
        /// </summary>
        /// <param name="date">
        /// Fecha base desde la cual se calcula el fin del mes.
        /// </param>
        /// <returns>
        /// Un <see cref="DateTime"/> correspondiente al último día del mes.
        /// </returns>
        public static DateTime LastDayOfMonth(this DateTime date) =>
                    new DateTime(date.Year, date.Month, 1).AddMonths(1).AddDays(-1).EndOfDay();

        /// <summary>
        /// Devuelve el primer día del mes de la fecha especificada,
        /// o <c>null</c> si la fecha es nula.
        /// </summary>
        /// <param name="date">
        /// Fecha base nullable desde la cual se calcula el inicio del mes.
        /// </param>
        /// <returns>
        /// El primer día del mes o <c>null</c> si <paramref name="date"/> es nulo.
        /// </returns>
        public static DateTime? FirstDayOfMonth(this DateTime? date) =>
                    date.HasValue
                    ? new DateTime(date.Value.Year, date.Value.Month, 1)
                    : null;

        /// <summary>
        /// Devuelve el último día del mes de la fecha especificada,
        /// o <c>null</c> si la fecha es nula.
        /// </summary>
        /// <param name="date">
        /// Fecha base nullable desde la cual se calcula el fin del mes.
        /// </param>
        /// <returns>
        /// El último día del mes o <c>null</c> si <paramref name="date"/> es nulo.
        /// </returns>
        public static DateTime? LastDayOfMonth(this DateTime? date) =>
                    date.HasValue
                    ? new DateTime(date.Value.Year, date.Value.Month, 1).AddMonths(1).AddDays(-1).EndOfDay()
                    : null;
    }
}
