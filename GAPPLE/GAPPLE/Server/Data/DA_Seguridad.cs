using System.Data.SqlClient;
using System.Data;
using GAPPLE.Shared.Model;

namespace GAPPLE.Server.Data
{
    public class DA_Seguridad
    {
        private string ConnectionString { get; }

        public DA_Seguridad(string connectionString) => ConnectionString = connectionString;

        public DataTable GetUsuarios(int? idUsuario, string? nombreUsuario, string? apellidoYNombre, int? idPerfil, bool? habilitado)
        {

            SqlConnection cnn;
            SqlCommand cmd = new();
            cnn = new(ConnectionString);
            DataTable dt = new();
            cmd.Parameters.Clear();
            cmd.Connection = cnn;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "prc_get_Usuarios";
            if(idUsuario != null) cmd.Parameters.AddWithValue("@pIdUsuario", idUsuario);
            if (nombreUsuario != null) cmd.Parameters.AddWithValue("@pNombreUsuario", nombreUsuario);
            if (apellidoYNombre != null) cmd.Parameters.AddWithValue("@pApellidoYNombre", apellidoYNombre);
            if (idPerfil != null) cmd.Parameters.AddWithValue("@pIdPerfil", idPerfil);
            if (habilitado != null) cmd.Parameters.AddWithValue("@pHabilitado", habilitado);
            SqlDataAdapter dataAdapter = new(cmd);
            dataAdapter.Fill(dt);

            return dt;
        }

        public void PostUsuario(string nombreUsuario, string apellidoYNombre, int idPerfil, string email, string Provincia, bool habilitado, string contraseña)
        {
            SqlConnection cnn;
            SqlCommand cmd = new();
            cnn = new(ConnectionString);
            cmd.Connection = cnn;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "prc_ins_Usuario";
            cmd.Parameters.AddWithValue("@pNombreUsuario", nombreUsuario);
            cmd.Parameters.AddWithValue("@pApellidoYNombre", apellidoYNombre);
            cmd.Parameters.AddWithValue("@pIdPerfil", idPerfil);
            cmd.Parameters.AddWithValue("@pCorreo", email);
            cmd.Parameters.AddWithValue("@pContrasenia", contraseña);
            cmd.Parameters.AddWithValue("@pProvincia", Provincia);
            cmd.Parameters.AddWithValue("@pHabilitado", habilitado);
            cnn.Open();
            cmd.ExecuteNonQuery();
            cnn.Close();
        }

        public void PutUsuario(int idUsuario, string nombreUsuario, string apellidoYNombre, int idPerfil, string email, string Provincia, bool habilitado, string contraseña)
        {
            SqlConnection cnn;
            SqlCommand cmd = new();
            cnn = new(ConnectionString);
            cmd.Connection = cnn;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "prc_upd_Usuario";
            cmd.Parameters.AddWithValue("@pIdUsuario", idUsuario);
            cmd.Parameters.AddWithValue("@pNombreUsuario", nombreUsuario);
            cmd.Parameters.AddWithValue("@pApellidoYNombre", apellidoYNombre);
            cmd.Parameters.AddWithValue("@pIdPerfil", idPerfil);
            cmd.Parameters.AddWithValue("@pCorreo", email);
            cmd.Parameters.AddWithValue("@pContrasenia", contraseña);
            cmd.Parameters.AddWithValue("@pProvincia", Provincia);
            cmd.Parameters.AddWithValue("@pHabilitado", habilitado);
            cnn.Open();
            cmd.ExecuteNonQuery();
            cnn.Close();
        }

        public DataTable GetUsuariosPerfiles(int? idPerfil, string? descripcion)
        {

            SqlConnection cnn;
            SqlCommand cmd = new();
            cnn = new(ConnectionString);
            DataTable dt = new();
            cmd.Parameters.Clear();
            cmd.Connection = cnn;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "prc_get_UsuariosPerfiles";
            if (idPerfil != null) cmd.Parameters.AddWithValue("@pIdPerfil", idPerfil);
            if (descripcion != null) cmd.Parameters.AddWithValue("@pDescripcion", descripcion);
            SqlDataAdapter dataAdapter = new(cmd);
            dataAdapter.Fill(dt);

            return dt;
        }
        public void PostUsuarioPerfiles(string? descripcion)
        {
			SqlConnection cnn;
			SqlCommand cmd = new();
			cnn = new(ConnectionString);
			cmd.Connection = cnn;
			cmd.CommandType = CommandType.StoredProcedure;
			cmd.CommandText = "prc_ins_UsuarioPerfiles";
			cmd.Parameters.AddWithValue("@pDescripcion", descripcion);
			cnn.Open();
			cmd.ExecuteNonQuery();
			cnn.Close();
		}
        public void PutUsuarioPerfiles(int? idPerfil, string? descripcion)
        {

			SqlConnection cnn;
			SqlCommand cmd = new();
			cnn = new(ConnectionString);
			cmd.Connection = cnn;
			cmd.CommandType = CommandType.StoredProcedure;
			cmd.CommandText = "prc_upd_UsuarioPerfiles";
            cmd.Parameters.AddWithValue("@pIdPerfeil", idPerfil);
			cmd.Parameters.AddWithValue("@pDescripcion", descripcion);
			cnn.Open();
			cmd.ExecuteNonQuery();
			cnn.Close();
		}
        internal int InsertarPermiso(int idPadre, string nombre, char tipo, string href, string icono, int orden)
        {
            int idPermiso = 0;
            using (SqlConnection cnn = new(ConnectionString))
            {
                var cmd = cnn.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "prc_ins_Permiso";
                cmd.Parameters.AddWithValue("@pIdPadre", idPadre);
                if (nombre != null) cmd.Parameters.AddWithValue("@pNombre", nombre);
                cmd.Parameters.AddWithValue("@pTipo", tipo);
                if (href != null) cmd.Parameters.AddWithValue("@pHRef", href);
                if (icono != null) cmd.Parameters.AddWithValue("@pIcono", icono);
                cmd.Parameters.AddWithValue("@pOrden", orden);

                SqlParameter returnValue = new("@Return", idPermiso)
                {
                    Direction = ParameterDirection.ReturnValue
                };

                cmd.Parameters.Add(returnValue);

                cnn.Open();
                cmd.ExecuteNonQuery();
                idPermiso = (int)returnValue.Value;
                cnn.Close();
            }
            return idPermiso;
        }

        internal void ActualizarPermiso(int idPermiso, int idPadre, string nombre, char tipo, string href, string icono, int orden)
        {
            using SqlConnection cnn = new(ConnectionString);
            var cmd = cnn.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "prc_upd_Permiso";
            cmd.Parameters.AddWithValue("@pIdPermiso", idPermiso);
            cmd.Parameters.AddWithValue("@pIdPadre", idPadre);
            if (nombre != null) cmd.Parameters.AddWithValue("@pNombre", nombre);
            cmd.Parameters.AddWithValue("@pTipo", tipo);
            cmd.Parameters.AddWithValue("@pHRef", href);
            cmd.Parameters.AddWithValue("@pIcono", icono);
            cmd.Parameters.AddWithValue("@pOrden", orden);
            cnn.Open();
            cmd.ExecuteNonQuery();
            cnn.Close();
        }

        internal void EliminarPermiso(int idPermiso, SqlTransaction trans)
        {
            SqlConnection cnn = trans.Connection;
            using SqlCommand cmd = cnn.CreateCommand();
            cmd.Transaction = trans;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "prc_del_Permiso";
            cmd.Parameters.AddWithValue("@pIdPermiso", idPermiso);
            cmd.ExecuteNonQuery();
        }

        internal void EliminarPermisoPorPerfil(int? idPerfil, int idPermiso, SqlTransaction trans)
        {
            SqlConnection cnn = trans.Connection;
            using SqlCommand cmd = cnn.CreateCommand();
            cmd.Transaction = trans;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "prc_del_PermisoPorPerfil";
            cmd.Parameters.AddWithValue("@pIdPerfil", idPerfil);
            cmd.Parameters.AddWithValue("@pIdPermiso", idPermiso);
            cmd.ExecuteNonQuery();
        }

        internal void EliminarPermisoPorUsuario(int? idUsuario, int idPermiso, SqlTransaction trans)
        {
            SqlConnection cnn = trans.Connection;
            using SqlCommand cmd = cnn.CreateCommand();
            cmd.Transaction = trans;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "prc_del_PermisoPorUsuario";
            cmd.Parameters.AddWithValue("@pIdUsuario", idUsuario);
            cmd.Parameters.AddWithValue("@pIdPermiso", idPermiso);
            cmd.ExecuteNonQuery();
        }

        internal DataTable ObtenerPermisos()
        {
            DataTable dt = new();
            using (SqlConnection cnn = new(ConnectionString))
            {
                SqlCommand cmd = cnn.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "prc_get_Permisos";
                SqlDataAdapter da = new(cmd);
                da.Fill(dt);
            }
            return dt;
        }
    }
}
