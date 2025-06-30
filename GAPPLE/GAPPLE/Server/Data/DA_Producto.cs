using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

namespace GAPPLE.Server.Data
{
    public class DA_Producto
    {
        private string ConnectionString { get; }

        public DA_Producto(string connectionString) => ConnectionString = connectionString;

        public DataTable ObtenerProductos(string? codigoProducto, string? descripcion, bool? clasificado, bool? pasivo, string? linea, SqlTransaction? transaction = null)
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
            if (codigoProducto != null) cmd.Parameters.AddWithValue("@pCodigoProducto", codigoProducto);
            if (descripcion != null) cmd.Parameters.AddWithValue("@pDescripcion", descripcion);
            if (clasificado != null) cmd.Parameters.AddWithValue("@pClasificados", clasificado);
            if (pasivo != null) cmd.Parameters.AddWithValue("@pPasivo", pasivo);
            if (linea != null) cmd.Parameters.AddWithValue("@pLinea", linea);
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
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "prc_get_Lineas";
            SqlDataAdapter dataAdapter = new(cmd);
            dataAdapter.Fill(dt);
            return dt;
        }

        public DataTable GetProductosParaOfertas(string linea, string codListaPrecios)
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
            if (codListaPrecios != null) cmd.Parameters.AddWithValue("@pCodListaPrecio", codListaPrecios);
            SqlDataAdapter dataAdapter = new(cmd);
            dataAdapter.Fill(dt);
            return dt;
        }

        public void EditarProducto(string idProducto, bool pasivo, int orden)
        {
            SqlConnection cnn = new(ConnectionString);
            SqlCommand cmd = new()
            {
                Connection = cnn,
                CommandType = CommandType.StoredProcedure,
                CommandText = "prc_upd_Producto"
            };
            cmd.Parameters.AddWithValue("@pIdProducto", idProducto);
            cmd.Parameters.AddWithValue("@pPasivo", pasivo);
            cmd.Parameters.AddWithValue("@pOrden", orden);
            cnn.Open();
            cmd.ExecuteNonQuery();
            cnn.Close();
        }

        public DataTable GetProductosComplementos()
        {
            DataTable dt = new();
            SqlConnection cnn = new(ConnectionString);
            SqlCommand cmd = cnn.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "prc_get_ProductoComplemento";
            SqlDataAdapter da = new(cmd);
            da.Fill(dt);
            return dt;
        }

        public void DeleteProductosComplementos(string codPrincipal, string codRelacionado, SqlTransaction trans)
        {
            SqlConnection cnn = trans.Connection;
            SqlCommand cmd = cnn.CreateCommand();
            cmd.Transaction = trans;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "prc_del_ProductoComplemento";
            cmd.Parameters.AddWithValue("@pCodigoPrincipal", codPrincipal);
            cmd.Parameters.AddWithValue("@pCodigoRelacionado", codRelacionado);
            cmd.ExecuteNonQuery();
        }

        public void InsertProductosComplementos(string codPrincipal, string codRelacionado, SqlTransaction trans)
        {
            SqlConnection cnn = trans.Connection;
            SqlCommand cmd = cnn.CreateCommand();
            cmd.Transaction = trans;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "prc_ins_ProductoComplemento";
            cmd.Parameters.AddWithValue("@pCodigoPrincipal", codPrincipal);
            cmd.Parameters.AddWithValue("@pCodigoRelacionado", codRelacionado);
            cmd.ExecuteNonQuery();
        }

        public DataTable ObtenerPrecio(string codLista, string linea, string codProducto)
        {
            DataTable dt = new();
            using (SqlConnection cnn = new(ConnectionString))
            {
                using (SqlCommand cmd = cnn.CreateCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "prc_get_Precios";
                    cmd.Parameters.AddWithValue("@pCodLista", codLista);
                    if (!string.IsNullOrEmpty(linea)) cmd.Parameters.AddWithValue("@pLinea", linea);
                    if (!string.IsNullOrEmpty(codProducto)) cmd.Parameters.AddWithValue("@pCodProducto", codProducto);
                    SqlDataAdapter da = new(cmd);
                    da.Fill(dt);
                    da.Dispose();
                }
            }

            return dt;
        }
    }
}
