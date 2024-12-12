using System.Collections.Generic;
using System.Linq;

namespace GAPPLE.Client.Tools
{
    internal class ParametrosDeConsulta
    {
        internal Pages Page { get; set; }
        internal Dictionary<string, object> Parametros { get; set; } = new();

        internal void Save(Pages page, Dictionary<string, object> parametros)
        {
            Page = page;
            Parametros = parametros;
        }

        internal void Clear()
        {
            Page = Pages.Null;
            Parametros.Clear();
        }

        internal string Query()
        {
            return string.Join("&", Parametros.Select(x => $"{x.Key}={x.Value}").ToArray());
        }

        internal enum Pages
        {
            Null,
            Ordenes,
            Usuarios,
        }
    }
}
