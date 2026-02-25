namespace GAPPLE.Shared.Helpers
{
    public static class EnumerableExtensions
    {
        /// <summary>
        /// Deduplica la colección usando la key indicada y devuelve una lista con
        /// esos valores únicos. Usa el mismo selector tanto para determinar la
        /// unicidad como para proyectar el resultado, evitando repetir lógica.
        /// <br></br>
        /// <b>Ejemplo:</b>
        ///     var ids = lista.DistinctSelect(x => x.IdProveedor);
        /// <br></br>
        /// Esto devuelve solo los IdProveedor únicos de la colección.
        /// </summary>
        /// <typeparam name="TSource">Tipo de los elementos de origen.</typeparam>
        /// <typeparam name="TKey">Tipo de la key usada para deduplicar y devolver.</typeparam>
        /// <param name="source">Colección de entrada.</param>
        /// <param name="keySelector">Función que obtiene la key para deduplicar y retornar.</param>
        /// <returns>Lista con los valores únicos basados en la key indicada.</returns>
        public static List<TKey> DistinctSelect<TSource, TKey>(this IEnumerable<TSource> source,
                                                    Func<TSource, TKey> keySelector,
                                                    NullHandling nullHandling = NullHandling.Exclude)
        {
            ArgumentNullException.ThrowIfNull(source);
            ArgumentNullException.ThrowIfNull(keySelector);

            var distinctItems = source.DistinctBy(keySelector);

            if (nullHandling == NullHandling.Exclude)
                distinctItems = distinctItems.Where(x => keySelector(x) is not null);

            return [.. distinctItems.Select(keySelector)];
        }

        /// <summary>
        /// Deduplica la colección usando la key indicada y luego proyecta cada
        /// elemento único utilizando otro selector distinto. Permite separar la
        /// lógica de unicidad de la lógica de resultado.
        /// <br></br>
        /// <b>Ejemplo:</b>
        ///     var listaFiltrada = lista.DistinctSelect(
        ///         x => x.IdProveedor,
        ///         x => x.NombreProveedor
        ///     );
        /// <br></br>
        /// Esto devuelve los nombres de proveedor correspondientes a cada
        /// IdProveedor único en la colección.
        /// </summary>
        /// <typeparam name="TSource">Tipo de los elementos de origen.</typeparam>
        /// <typeparam name="TKey">Tipo de la key usada para deduplicar.</typeparam>
        /// <typeparam name="TResult">Tipo del valor proyectado.</typeparam>
        /// <param name="source">Colección de entrada.</param>
        /// <param name="keySelector">Función que obtiene la key para deduplicar.</param>
        /// <param name="resultSelector">Función que proyecta el valor a devolver.</param>
        /// <returns>Lista con los valores proyectados asociados a keys únicas.</returns>
        public static List<TResult> DistinctSelect<TSource, TKey, TResult>(this IEnumerable<TSource> source,
                                                                        Func<TSource, TKey> keySelector,
                                                                        Func<TSource, TResult> resultSelector,
                                                                        NullHandling nullHandling = NullHandling.Exclude)
        {
            ArgumentNullException.ThrowIfNull(source);
            ArgumentNullException.ThrowIfNull(keySelector);
            ArgumentNullException.ThrowIfNull(resultSelector);

            var distinctItems = source.DistinctBy(keySelector);

            if (nullHandling == NullHandling.Exclude)
                distinctItems = distinctItems.Where(x => keySelector(x) is not null);

            return [.. distinctItems.Select(resultSelector)];
        }

        /// <summary>
        /// Deduplica la colección usando la key indicada y devuelve la cantidad
        /// de elementos únicos según esa key.
        /// 
        /// Ejemplo:
        ///     var cant = lista.DistinctCount(x => x.IdProveedor);
        /// </summary>
        /// <typeparam name="TSource">Tipo de los elementos de origen.</typeparam>
        /// <typeparam name="TKey">Tipo de la key para deduplicar.</typeparam>
        /// <param name="source">Colección de entrada.</param>
        /// <param name="keySelector">Función que obtiene la key para deduplicar.</param>
        /// <returns>Cantidad de elementos únicos basados en la key indicada.</returns>
        public static int DistinctCount<TSource, TKey>(this IEnumerable<TSource> source,
                                                        Func<TSource, TKey> keySelector,
                                                        NullHandling nullHandling = NullHandling.Exclude)
        {
            ArgumentNullException.ThrowIfNull(source);
            ArgumentNullException.ThrowIfNull(keySelector);

            var distinctKeys = source
                .Select(keySelector)
                .Distinct();

            if (nullHandling == NullHandling.Exclude)
                distinctKeys = distinctKeys.Where(x => x is not null);

            return distinctKeys.Count();
        }

        public enum NullHandling
        {
            Include,
            Exclude
        }
    }
}
