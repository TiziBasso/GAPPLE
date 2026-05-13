using GAPPLE.Shared.Model;
using System.Data;
using Microsoft.Data.SqlClient;

namespace GAPPLE.Server.Data
{
    public class DA_Motivos
    {
        private string ConnectionString { get; }
        public DA_Motivos(string connectionString) => ConnectionString = connectionString;

        public DataTable ObtenerMotivos(int? idMotivo, string descripcion, bool? pasivo, int? idDeposito)

        {
            DataTable dt = new();
            SqlConnection cnn = new(ConnectionString);
            SqlCommand cmd = cnn.CreateCommand();

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "prc_get_Motivos";
            cmd.Parameters.Clear();
            if (idMotivo != null) cmd.Parameters.AddWithValue("@pIdMotivo", idMotivo);
            if (!string.IsNullOrWhiteSpace(descripcion)) cmd.Parameters.AddWithValue("@pDescripcion", descripcion);
            if (pasivo != null) cmd.Parameters.AddWithValue("@pPasivo", pasivo);
            if (idDeposito != null) cmd.Parameters.AddWithValue("@pIdDeposito", idDeposito);
            SqlDataAdapter da = new(cmd);
            da.Fill(dt);

            return dt;
        }

        public void EditarMotivos(Motivo motivo)
        {
            SqlConnection cnn = new(ConnectionString);
            SqlCommand cmd = new()
            {
                Connection = cnn,
                CommandType = CommandType.StoredProcedure,
                CommandText = "prc_upd_Motivos"
            };
            cmd.Parameters.AddWithValue("@pIdMotivo", motivo.IdMotivo);
            cmd.Parameters.AddWithValue("@pDescripcion", motivo.Descripcion);
            cmd.Parameters.AddWithValue("@pPasivo", motivo.Pasivo);
            cmd.Parameters.AddWithValue("@pIdDeposito", motivo.IdDeposito);
            cmd.Parameters.AddWithValue("@pEdicionUsuario", motivo.EdicionUsuario);
            cnn.Open();
            cmd.ExecuteNonQuery();
            cnn.Close();
        }

        public int InsertarMotivo(Motivo motivo)
        {
            int idMotivo = 0;
            DataTable dt = new();
            SqlConnection cnn;
            SqlCommand cmd = new();
            cnn = new(ConnectionString);
            cmd.Connection = cnn;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "prc_ins_Motivos";
            cmd.Parameters.AddWithValue("@pDescripcion", motivo.Descripcion);
            cmd.Parameters.AddWithValue("@pPasivo", motivo.Pasivo);
            if (motivo.IdDeposito != null) cmd.Parameters.AddWithValue("@pIdDeposito", motivo.IdDeposito);
            cmd.Parameters.AddWithValue("@pAltaUsuario", motivo.AltaUsuario);
            SqlDataAdapter da = new(cmd);
            da.Fill(dt);
            idMotivo = int.Parse(dt.Rows[0]["IdMotivo"].ToString()!);

            return idMotivo;
        }
    }
}
