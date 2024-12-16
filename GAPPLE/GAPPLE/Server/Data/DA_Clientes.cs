using System;
using System.Data;
using System.Data.SqlClient;


namespace GAPPLE.Server.Data
{
    public class DA_Clientes
    {
        private string ConnectionString { get; }

        public DA_Clientes(string connectionString) => ConnectionString = connectionString;


        public DataTable ObtenerClientes(string? codCliente, string? razonSocial, string? cuit, bool? clienteEspecial, SqlTransaction? transaction = null)
        {

            SqlConnection cnn;
            SqlCommand cmd = new();

            if (transaction == null)
            {
                cnn = new(ConnectionString);
            }
            else
            {
                cnn = transaction.Connection;
                cmd.Transaction = transaction;
            }

            DataTable dt = new();
            cmd.Parameters.Clear();
            cmd.Connection = cnn;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "prc_get_Clientes";
            if (codCliente != null) cmd.Parameters.AddWithValue("@pCodCliente", codCliente);
            if (razonSocial != null) cmd.Parameters.AddWithValue("@pRazonSocial", razonSocial);
            if (cuit != null) cmd.Parameters.AddWithValue("@pCUIT", cuit);
            if (clienteEspecial != null) cmd.Parameters.AddWithValue("@pClienteEspecial", clienteEspecial);
            SqlDataAdapter dataAdapter = new(cmd);
            dataAdapter.Fill(dt);

            return dt;

        }

        public int PersistirEdicionCliente(string codigoCliente, bool clienteEspecial, string observaciones,SqlTransaction trans)
        {
            int result = 0;
            SqlConnection cnn;
            SqlCommand cmd = new();
            if (trans == null)
                cnn = new(ConnectionString);
            else
            {
                cnn = trans.Connection;
                cmd.Transaction = trans;
            }
            cmd.Connection = cnn;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "prc_upd_Clientes";
            cmd.Parameters.AddWithValue("@pCodCliente", codigoCliente);
            cmd.Parameters.AddWithValue("@pCLienteEspecial", clienteEspecial);
            cmd.Parameters.AddWithValue("@pObeservaciones", observaciones);
            SqlParameter returnValue = new("@Return", result)
            {
                Direction = ParameterDirection.ReturnValue
            };
            cmd.Parameters.Add(returnValue);
            if (trans == null)
            {
                cnn.Open();
                cmd.ExecuteNonQuery();
                result = (int)returnValue.Value;
                cnn.Close();
            }
            else
            {
                cmd.ExecuteNonQuery();
                result = (int)returnValue.Value;
            }
            return result;
        }
    }
}
