using System.Data;
using Microsoft.Data.SqlClient;

namespace GAPPLE.Server.Data
{
    public class DA_VendedoresPorCliente
    {
        private string ConnectionString { get; }
        public DA_VendedoresPorCliente(string connectionString) => ConnectionString = connectionString;

        public DataTable ObtenerVendedoresPorCliente(int idCliente)
        {
            DataTable dt = new();
            using SqlConnection cnn = new(ConnectionString);
            SqlCommand cmd = cnn.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "prc_get_VendedoresPorCliente";
            cmd.Parameters.AddWithValue("@pIdCliente", idCliente);
            SqlDataAdapter da = new(cmd);
            da.Fill(dt);
            return dt;
        }

        public string ObtenerVendedorDefaultPorCliente(int idCliente)
        {
            using SqlConnection cnn = new(ConnectionString);
            SqlCommand cmd = cnn.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "prc_get_VendedorDefaultPorCliente";
            cmd.Parameters.AddWithValue("@pIdCliente", idCliente);
            cnn.Open();
            var result = cmd.ExecuteScalar();
            return result == null || result == DBNull.Value ? null : result.ToString();
        }

        public void PersistirVendedoresPorCliente(int idCliente, string idUsuariosCsv)
        {
            using SqlConnection cnn = new(ConnectionString);
            SqlCommand cmd = cnn.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "prc_ins_VendedoresPorCliente";
            cmd.Parameters.AddWithValue("@pIdCliente", idCliente);
            cmd.Parameters.AddWithValue("@pIdUsuarios", idUsuariosCsv);
            cnn.Open();
            cmd.ExecuteNonQuery();
        }

        public void EliminarVendedoresPorCliente(int idCliente)
        {
            using SqlConnection cnn = new(ConnectionString);
            SqlCommand cmd = cnn.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "prc_del_VendedoresPorCliente";
            cmd.Parameters.AddWithValue("@pIdCliente", idCliente);
            cnn.Open();
            cmd.ExecuteNonQuery();
        }
    }
}
