using Microsoft.Data.SqlClient;
using System.Data;

namespace GAPPLE.Server.Data
{
    public class DA_Parametro
    {
        private string ConnectionString { get; }

        public DA_Parametro(string connectionString) => ConnectionString = connectionString;

        public DataTable ObtenerPermisos(int? idUsuario, char? tipo, string url, int? idPadre, string nombre)
        {
            DataTable dt = new();

            using (SqlConnection cnn = new(ConnectionString))
            {
                using SqlCommand cmd = new();
                cmd.Connection = cnn;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "prc_get_PermisosAcceso";
                cmd.Parameters.AddWithValue("@pIdUsuario", idUsuario);
                if (tipo != null) cmd.Parameters.AddWithValue("@pTipo", tipo);
                if (url != null) cmd.Parameters.AddWithValue("@pHRef", url);
                if (idPadre != null) cmd.Parameters.AddWithValue("@pIdPadre", idPadre);
                if (nombre != null) cmd.Parameters.AddWithValue("@pNombre", nombre);
                SqlDataAdapter da = new(cmd);
                da.Fill(dt);
            }
            return dt;
        }

        public DataTable GetPermisosTotal(int? idUsuario, int? idPerfil)
        {
            DataTable dt = new();
            using (SqlConnection cnn = new(ConnectionString))
            {
                using SqlCommand cmd = new();
                cmd.Connection = cnn;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "prc_get_PermisosAccesoTotal";
                if (idUsuario != null) cmd.Parameters.AddWithValue("@pIdUsuario", idUsuario);
                if (idPerfil != null) cmd.Parameters.AddWithValue("@pIdPerfil", idPerfil);
                SqlDataAdapter da = new(cmd);
                da.Fill(dt);
            }
            return dt;
        }
    }
}
