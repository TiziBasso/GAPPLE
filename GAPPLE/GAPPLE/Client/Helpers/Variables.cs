using Microsoft.Extensions.Options;
using System.ComponentModel.DataAnnotations;

namespace GAPPLE.Client.Helpers
{
    internal static class Variables
    {
        internal static IEnumerable<Opcion> EstadosNum
        {
            get
            {
                return new List<Opcion>
                {
                    new Opcion(1, "Ambos"),
                    new Opcion(2, "Creada"),
                    new Opcion(3, "No sé")
                };
            }
        }
    }
}
