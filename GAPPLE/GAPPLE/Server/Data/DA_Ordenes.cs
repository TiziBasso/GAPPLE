using Radzen.Blazor.Rendering;
using System.Data;
using System.Data.SqlClient;

namespace GAPPLE.Server.Data
{
    public class DA_Ordenes
    {
        private string ConnectionString { get; }

        public DA_Ordenes(string connectionString) => ConnectionString = connectionString;

        public DataTable ObtenerOrdenes(int? idOrden, string? cliente, DateTime? desde, DateTime? hasta, int? idEstado)
        {
            DataTable dt = new DataTable();
            using (SqlConnection cnn = new(ConnectionString))
            {
                SqlCommand cmd = new()
                {
                    Connection = cnn,
                    CommandType = CommandType.StoredProcedure,
                    CommandText = "prc_get_PedidosCabecera"
                };
                //if (idOrden != null) cmd.Parameters.AddWithValue("@pIdOrden", idOrden);
                //if (cliente != null) cmd.Parameters.AddWithValue("@pCliente", cliente);
                //if (desde != null) cmd.Parameters.AddWithValue("@pDesde", desde);
                //if (hasta != null) cmd.Parameters.AddWithValue("@pHasta", hasta);
                //if (idEstado != null) cmd.Parameters.AddWithValue("@pIdEstado", idEstado);
                SqlDataAdapter da = new(cmd);
                da.Fill(dt);
            }
            return dt;
        }

        public void PersistirOferta(string? nombre, string? linea, string? descripcion, decimal descuento, DateTime? desde, DateTime? hasta, string inclusiones)
        {
            SqlConnection cnn = new(ConnectionString);
            SqlCommand cmd = new()
            {
                Connection = cnn,
                CommandType = CommandType.StoredProcedure,
                CommandText = "prc_ins_Ofertas"
            };
            cmd.Parameters.AddWithValue("@pNombre", nombre);
            cmd.Parameters.AddWithValue("@pLinea", linea);
            cmd.Parameters.AddWithValue("@pDescripcion", descripcion);
            cmd.Parameters.AddWithValue("@pDescuento", descuento);
            cmd.Parameters.AddWithValue("@pDesde", desde);
            cmd.Parameters.AddWithValue("@pHasta", hasta);
            cmd.Parameters.AddWithValue("@pActiva", 1);
            cmd.Parameters.AddWithValue("@pinclusiones", inclusiones);
            cmd.Parameters.AddWithValue("@pAltaUsuario", "PRUEBAS");
            cnn.Open();
            cmd.ExecuteNonQuery();
            cnn.Close();
        }

        public void EditarOferta(int idOferta, string? nombre, string? linea, string? descripcion, decimal descuento, DateTime? desde, DateTime? hasta, string inclusiones)
        {
            SqlConnection cnn = new(ConnectionString);
            SqlCommand cmd = new()
            {
                Connection = cnn,
                CommandType = CommandType.StoredProcedure,
                CommandText = "prc_upd_Ofertas"
            };
            cmd.Parameters.AddWithValue("@pIdOferta", idOferta);
            cmd.Parameters.AddWithValue("@pNombre", nombre);
            cmd.Parameters.AddWithValue("@pLinea", linea);
            cmd.Parameters.AddWithValue("@pDescripcion", descripcion);
            cmd.Parameters.AddWithValue("@pDescuento", descuento);
            cmd.Parameters.AddWithValue("@pDesde", desde);
            cmd.Parameters.AddWithValue("@pHasta", hasta);
            cmd.Parameters.AddWithValue("@pActiva", 1);
            cmd.Parameters.AddWithValue("@pinclusiones", inclusiones);
            cmd.Parameters.AddWithValue("@pAltaUsuario", "PRUEBAS");
            cnn.Open();
            cmd.ExecuteNonQuery();
            cnn.Close();
        }
    }
}
