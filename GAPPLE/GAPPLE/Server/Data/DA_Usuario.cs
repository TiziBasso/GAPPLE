using System.Data.SqlClient;
using System.Data;

namespace GAPPLE.Server.Data
{
    public class DA_Usuario
    {
        private string ConnectionString { get; }

        public DA_Usuario(string connectionString) => ConnectionString = connectionString;

        public DataTable ObtenerUsuarioPerfiles(int? idPerfil, string descripcion)
        {
            DataTable dt = new();
            using (SqlConnection cnn = new(ConnectionString))
            {
                var cmd = cnn.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "prc_get_UsuariosPerfiles";
                if (idPerfil != null) cmd.Parameters.AddWithValue("@pIdPerfil", idPerfil);
                if (descripcion != null) cmd.Parameters.AddWithValue("@pDescripcion", descripcion);
                SqlDataAdapter da = new(cmd);
                da.Fill(dt);
            }
            return dt;
        }

        public void PostPermisoPorPerfil(int idPerfil, int idPermiso, IDbTransaction trans)
        {
            SqlConnection cnn;
            SqlCommand cmd = new();
            if (trans == null)
            {
                cnn = new(ConnectionString);
            }
            else
            {
                cnn = (SqlConnection)trans.Connection!;
                cmd.Transaction = (SqlTransaction)trans;
            }
            cmd.Parameters.Clear();
            cmd.Connection = cnn;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "prc_ins_PermisoPorPerfil";
            cmd.Parameters.AddWithValue("@pIdPerfil", idPerfil);
            cmd.Parameters.AddWithValue("@pIdPermiso", idPermiso);
            if (trans == null)
            {
                cnn.Open();
                cmd.ExecuteNonQuery();
                cnn.Close();
                cnn.Dispose();
            }
            else
                cmd.ExecuteNonQuery();
            cmd.Dispose();
        }

        public DataTable ObtenerUsuariosPorPerfil(int idPerfil)
        {
            DataTable dt = new();
            using (SqlConnection cnn = new(ConnectionString))
            {
                using SqlCommand cmd = new();
                cmd.Connection = cnn;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "prc_get_UsuariosPorPerfil";
                cmd.Parameters.AddWithValue("@pIdPerfil", idPerfil);
                SqlDataAdapter da = new(cmd);
                da.Fill(dt);
            }
            return dt;
        }

        public void PostPermisoPorUsuario(int idUsuario, int idPermiso, bool habilitado, IDbTransaction trans)
        {
            SqlConnection cnn;
            SqlCommand cmd = new();
            if (trans == null)
            {
                cnn = new(ConnectionString);
            }
            else
            {
                cnn = (SqlConnection)trans.Connection!;
                cmd.Transaction = (SqlTransaction)trans;
            }
            cmd.Parameters.Clear();
            cmd.Connection = cnn;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "prc_ins_PermisoPorUsuario";
            cmd.Parameters.AddWithValue("@pIdUsuario", idUsuario);
            cmd.Parameters.AddWithValue("@pIdPermiso", idPermiso);
            cmd.Parameters.AddWithValue("@pHabilitado", habilitado);
            if (trans == null)
            {
                cnn.Open();
                cmd.ExecuteNonQuery();
                cnn.Close();
                cnn.Dispose();
            }
            else
                cmd.ExecuteNonQuery();
            cmd.Dispose();
        }

        public void DeletePermisoPorUsuario(int idUsuario, int idPermiso, IDbTransaction trans)
        {
            SqlConnection cnn;
            SqlCommand cmd = new();
            if (trans == null)
            {
                cnn = new(ConnectionString);
            }
            else
            {
                cnn = (SqlConnection)trans.Connection!;
                cmd.Transaction = (SqlTransaction)trans;
            }
            cmd.Parameters.Clear();
            cmd.Connection = cnn;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "prc_del_PermisoPorUsuario";
            cmd.Parameters.AddWithValue("@pIdUsuario", idUsuario);
            cmd.Parameters.AddWithValue("@pIdPermiso", idPermiso);
            if (trans == null)
            {
                cnn.Open();
                cmd.ExecuteNonQuery();
                cnn.Close();
                cnn.Dispose();
            }
            else
                cmd.ExecuteNonQuery();
            cmd.Dispose();
        }

        public void DeletePermisoPorPerfil(int idPerfil, int idPermiso, IDbTransaction trans)
        {
            SqlConnection cnn;
            SqlCommand cmd = new();
            if (trans == null)
            {
                cnn = new(ConnectionString);
            }
            else
            {
                cnn = (SqlConnection)trans.Connection!;
                cmd.Transaction = (SqlTransaction)trans;
            }
            cmd.Parameters.Clear();
            cmd.Connection = cnn;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "prc_del_PermisoPorPerfil";
            cmd.Parameters.AddWithValue("@pIdPerfil", idPerfil);
            cmd.Parameters.AddWithValue("@pIdPermiso", idPermiso);
            if (trans == null)
            {
                cnn.Open();
                cmd.ExecuteNonQuery();
                cnn.Close();
                cnn.Dispose();
            }
            else
                cmd.ExecuteNonQuery();
            cmd.Dispose();
        }

        public DataTable ObtenerUsuario(int? idUsuario = null, string nombreUsuario = null, string apellidoYNombre = null, string descripcionPerfil = null, bool? pasivo = null)
        {
            DataTable dt = new();
            using (SqlConnection cnn = new(ConnectionString))
            {
                using SqlCommand cmd = new();
                cmd.Connection = cnn;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "prc_get_Usuarios";
                if (idUsuario != null) cmd.Parameters.AddWithValue("@pIdUsuario", idUsuario);
                if (nombreUsuario != null) cmd.Parameters.AddWithValue("@pNombreUsuario", nombreUsuario);
                if (apellidoYNombre != null) cmd.Parameters.AddWithValue("@pApellidoYNombre", apellidoYNombre);
                if (descripcionPerfil != null) cmd.Parameters.AddWithValue("@pDescripcionPerfil", descripcionPerfil);
                if (pasivo != null) cmd.Parameters.AddWithValue("@pPasivo", pasivo);
                SqlDataAdapter da = new(cmd);
                da.Fill(dt);
            }
            return dt;
        }
    }
}
