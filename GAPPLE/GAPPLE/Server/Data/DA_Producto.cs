using System.Data;
using System.Data.SqlClient;

namespace GAPPLE.Server.Data
{
    public class DA_Producto
    {
        private string ConnectionString { get; }

        public DA_Producto(string connectionString) => ConnectionString = connectionString;

        public DataTable ObtenerProductos(string? codigoProducto, string? descripcion, bool? clasificado, SqlTransaction? transaction = null)
        {
            SqlConnection cnn;
            SqlCommand cmd = new();
            if (transaction == null)
                cnn = new(ConnectionString);
            else
            {
                cnn = transaction.Connection!;
                cmd.Transaction = transaction;
            }

            DataTable dt = new();
            cmd.Parameters.Clear();
            cmd.Connection = cnn;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "prc_get_Productos";
            if (codigoProducto!= null) cmd.Parameters.AddWithValue("@pCodigoProducto", codigoProducto);
            if (descripcion != null) cmd.Parameters.AddWithValue("@pDescripcion", descripcion);
            if (clasificado != null) cmd.Parameters.AddWithValue("@pClasificados", clasificado);
            SqlDataAdapter dataAdapter = new(cmd);
            dataAdapter.Fill(dt);

            return dt;
        }

        public DataTable GetLineas()
        {
            SqlConnection cnn;
            SqlCommand cmd = new();
                cnn = new(ConnectionString);
            DataTable dt = new();
            cmd.Parameters.Clear();
            cmd.Connection = cnn;
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "select distinct Linea from Clasificaciones";
            SqlDataAdapter dataAdapter = new(cmd);
            dataAdapter.Fill(dt);
            return dt;
        }

        public DataTable GetProductosParaOfertas(string linea)
        {
            SqlConnection cnn;
            SqlCommand cmd = new();
            cnn = new(ConnectionString);
            DataTable dt = new();
            cmd.Parameters.Clear();
            cmd.Connection = cnn;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "prc_get_ProductosParaOfertas";
            cmd.Parameters.AddWithValue("@pLinea", linea);
            SqlDataAdapter dataAdapter = new(cmd);
            dataAdapter.Fill(dt);
            return dt;
        }
    }
}
