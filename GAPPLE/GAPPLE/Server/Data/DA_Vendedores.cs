using System;
using System.Data;
using System.Data.SqlClient;


namespace GAPPLE.Server.Data
{
    public class DA_Vendedores
    {
        private string ConnectionString { get; }

        public DA_Vendedores(string connectionString) => ConnectionString = connectionString;


        public DataTable ObtenerVendedores()
        {
            DataTable dt = new();
            using (SqlConnection cnn = new(ConnectionString))
            {
                SqlCommand cmd = cnn.CreateCommand();

                cmd.Parameters.Clear();
                cmd.Connection = cnn;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "prc_get_Vendedores";
                SqlDataAdapter dataAdapter = new(cmd);
                dataAdapter.Fill(dt);
            }

            return dt;

        }

    }
}
