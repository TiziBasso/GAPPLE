using System.Data;
using System.Data.SqlClient;

namespace GAPPLE.Server.Data
{
    public class DA_Motivos
    {
        private string ConnectionString { get; }
        public DA_Motivos(string connectionString) => ConnectionString = connectionString;
        public DataTable ObtenerMotivos(string descripcion, bool pasivo, int? idDeposito, string descripcionDeposito, DateTime altaResgistro, string alta)

        {
            DataTable dt = new();
            SqlConnection cnn = new(ConnectionString);
            SqlCommand cmd = cnn.CreateCommand();

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "prc_get_Motivos";
            cmd.Parameters.Clear();
            if (descripcion != null) cmd.Parameters.AddWithValue("@pDescripcion", descripcion);
            if (pasivo != null) cmd.Parameters.AddWithValue("@pPasivo", pasivo);
            if (idDeposito != null) cmd.Parameters.AddWithValue("@pIdDeposito", idDeposito);
            if (descripcionDeposito != null) cmd.Parameters.AddWithValue("@pDescripcionDeposito", descripcionDeposito);
            SqlDataAdapter da = new(cmd);
            da.Fill(dt);

            return dt;
        }

        public void EditarMotivos(string descripcion, bool pasivo)
        {
            SqlConnection cnn = new(ConnectionString);
            SqlCommand cmd = new()
            {
                Connection = cnn,
                CommandType = CommandType.StoredProcedure,
                CommandText = "prc_upd_Motivos"
            };
            cmd.Parameters.AddWithValue("@pDescripcion", descripcion);
            cmd.Parameters.AddWithValue("@pPasivo", pasivo);
            cnn.Open();
            cmd.ExecuteNonQuery();
            cnn.Close();
        }

        public int PostMotivos(string descripcion, bool pasivo, int? idDeposito, string descripcionDeposito)
        {
            int idMotivo = 0;
            DataTable dt = new();
            SqlConnection cnn;
            SqlCommand cmd = new();
            cnn = new(ConnectionString);
            cmd.Connection = cnn;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "prc_ins_Motivos";
            cmd.Parameters.AddWithValue("@pDescripcion", descripcion);
            cmd.Parameters.AddWithValue("@pPasivo", pasivo);
            if (idDeposito != null) cmd.Parameters.AddWithValue("@pIdDeposito", idDeposito);
            cmd.Parameters.AddWithValue("@pDescripcionDeposito", descripcionDeposito);
            SqlDataAdapter da = new(cmd);
            da.Fill(dt);
            idMotivo = int.Parse(dt.Rows[0]["IdMotivo"].ToString()!);

            return idMotivo;
        }
    }
}
