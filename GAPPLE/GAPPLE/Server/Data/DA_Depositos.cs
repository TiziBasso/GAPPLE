using System.Data;
using System.Data.SqlClient;

namespace GAPPLE.Server.Data
{
    public class DA_Depositos
    {
        private string ConnectionString { get; }

        public DA_Depositos(string connectionString) => ConnectionString = connectionString;

        public DataTable ObtenerDepositos(string codigoTango, string descripcion, bool? visible)
        {
            DataTable dt = new();
            SqlConnection cnn = new(ConnectionString);
            SqlCommand cmd = cnn.CreateCommand();

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "prc_get_Depositos";
            cmd.Parameters.Clear();
            if (codigoTango != null) cmd.Parameters.AddWithValue("@pCodigoTango", codigoTango);
            if (descripcion != null) cmd.Parameters.AddWithValue("@pDescripcion", descripcion);
            if (visible != null) cmd.Parameters.AddWithValue("@pVisible", visible);
            SqlDataAdapter da = new(cmd);
            da.Fill(dt);

            return dt;
        }

        public void EditarDepositos(int idDeposito, bool visible)
        {
            SqlConnection cnn = new(ConnectionString);
            SqlCommand cmd = new()
            {
                Connection = cnn,
                CommandType = CommandType.StoredProcedure,
                CommandText = "prc_upd_Depositos"
            };
            cmd.Parameters.AddWithValue("@pIdDeposito", idDeposito);
            cmd.Parameters.AddWithValue("@pVisible", visible);
            cnn.Open();
            cmd.ExecuteNonQuery();
            cnn.Close();
        }
    }
}
