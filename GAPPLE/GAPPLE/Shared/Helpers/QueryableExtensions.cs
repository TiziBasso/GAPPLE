using System.Linq.Expressions;

namespace GAPPLE.Shared.Helpers
{
    public static class QueryableExtensions
    {
        public static IQueryable<T> FilterDate<T>(this IQueryable<T> query,
                                                    Expression<Func<T, DateTime?>> selector,
                                                    DateTime? fecha)
        {
            if (fecha == null)
                return query;

            var inicio = (DateTime?)fecha.Value.Date;
            var fin = (DateTime?)fecha.Value.Date.AddDays(1).AddTicks(-1);

            var param = selector.Parameters[0];

            // selector.Body es DateTime?
            var left = selector.Body;
            var rightInicio = Expression.Constant(inicio, typeof(DateTime?));
            var rightFin = Expression.Constant(fin, typeof(DateTime?));

            var ge = Expression.GreaterThanOrEqual(left, rightInicio);
            var le = Expression.LessThanOrEqual(left, rightFin);
            var body = Expression.AndAlso(ge, le);

            return query.Where(Expression.Lambda<Func<T, bool>>(body, param));
        }
    }
}
