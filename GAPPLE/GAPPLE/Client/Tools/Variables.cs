using GAPPLE.Client.Entities;

namespace GAPPLE.Client.Tools
{
    internal static class Variables
    {
        internal static IEnumerable<Opcion> EstadosNum
        {
            get
            {
                return
                [
                    new Opcion(1, "Ambos"),
                    new Opcion(2, "Activo"),
                    new Opcion(3, "Pasivo")
                ];
            }
        }

        internal static IEnumerable<Opcion> Visibilidad
        {
            get
            {
                return
                [
                    new Opcion(1, "Ambos"),
                    new Opcion(2, "Visible"),
                    new Opcion(3, "Invisible")
                ];
            }
        }

        internal static IEnumerable<Opcion> EstadosBool
        {
            get
            {
                return
                [
                    new Opcion((object)null, "(Todos)"),
                    new Opcion(false, "Activo"),
                    new Opcion(true, "Pasivo")
                ];
            }
        }

        internal static IEnumerable<Opcion> EstadosSiNo
        {
            get
            {
                return
                [
                    new Opcion((object)null, "Ambos"),
                    new Opcion(false, "Sí"),
                    new Opcion(true, "No")
                ];
            }
        }

        internal static IEnumerable<Opcion> Clasificaciones
        {
            get
            {
                return
                [
                    new Opcion((object)null, "Todos"),
                    new Opcion(true, "Sí"),
                    new Opcion(false, "No")
                ];
            }
        }

        internal static class ErrorPages
        {
            internal const string Invalido = "errorpage/invalid";
            internal const string Desautorizado = "errorpage/unauthorized";
            internal const string Desconfigurado = "errorpage/unconfigured";
            internal const string Inaccesible = "errorpage/unavailable";
            internal const string Main = "errorpage";
            internal const string UsuarioNoEncontrado = "errorpage/usernotfound";
            internal const string ReparacionEcommerce = "errorpage/reparacionecommerce";
        }
    }
}
