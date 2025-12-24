using GAPPLE.Shared.Helpers;
using System.Data;
using System.Reflection;

namespace GAPPLE.Server.Tools
{
    public static class DataRowHelper
    {
        public static T MapDataRowTo<T>(DataRow row) where T : new()
        {
            T obj = new();
            var props = typeof(T).GetProperties();

            foreach (var prop in props)
            {
                // 1) Buscamos si la propiedad tiene atributo ColumnName
                var colAttr = prop.GetCustomAttribute<ColumnNameAttribute>();
                string columnName = colAttr?.Name ?? prop.Name;

                // 2) Si la columna no existe, seguimos
                if (!row.Table.Columns.Contains(columnName))
                    continue;

                object value = row[columnName];

                if (value == DBNull.Value)
                {
                    // 3) Si la propiedad admite null, asignamos null
                    if (Nullable.GetUnderlyingType(prop.PropertyType) != null
                        || !prop.PropertyType.IsValueType)
                    {
                        prop.SetValue(obj, null);
                    }
                    continue;
                }

                Type targetType = Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType;

                // 4) Si es enum
                if (targetType.IsEnum)
                {
                    prop.SetValue(obj, Enum.ToObject(targetType, value));
                    continue;
                }

                // 5) Conversión estándar
                prop.SetValue(obj, Convert.ChangeType(value, targetType));
            }

            return obj;
        }
    }
}
