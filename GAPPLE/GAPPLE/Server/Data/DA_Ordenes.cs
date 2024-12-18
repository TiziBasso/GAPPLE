using Radzen.Blazor.Rendering;
using System.Data;
using System.Data.SqlClient;
using System.Runtime.InteropServices;

namespace GAPPLE.Server.Data
{
    public class DA_Ordenes
    {
        private string ConnectionString { get; }

        public DA_Ordenes(string connectionString) => ConnectionString = connectionString;

        public DataTable ObtenerOrdenes(DateTime desde, DateTime hasta, int? idPedido, bool? presupuesto, string? razonSocial,
                                        string? linea, string? zona, int? idEstado, string? codTango)
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
                cmd.Parameters.AddWithValue("@pDesde", desde);
                cmd.Parameters.AddWithValue("@pHasta", hasta);
                if (idPedido != null) cmd.Parameters.AddWithValue("@pIdPedido", idPedido);
                if (presupuesto != null) cmd.Parameters.AddWithValue("@pPresupuesto", presupuesto);
                if (!string.IsNullOrEmpty(razonSocial)) cmd.Parameters.AddWithValue("@pRazonSocial", razonSocial);
                if (!string.IsNullOrEmpty(linea)) cmd.Parameters.AddWithValue("@pLinea", linea);
                if (!string.IsNullOrEmpty(zona)) cmd.Parameters.AddWithValue("@pCodZona", zona);
                if (idEstado != null) cmd.Parameters.AddWithValue("@pIdEstado", idEstado);
                if (!string.IsNullOrEmpty(codTango)) cmd.Parameters.AddWithValue("@pCodTango", codTango);
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

        public DataTable ObtenerTransportes()
        {
            DataTable dt = new DataTable();
            using (SqlConnection cnn = new(ConnectionString))
            {
                SqlCommand cmd = new()
                {
                    Connection = cnn,
                    CommandType = CommandType.StoredProcedure,
                    CommandText = "prc_get_Transportes"
                };
                SqlDataAdapter da = new(cmd);
                da.Fill(dt);
            }
            return dt;
        }

        public DataTable ObtenerCondicionesDeVenta()
        {
            DataTable dt = new DataTable();
            using (SqlConnection cnn = new(ConnectionString))
            {
                SqlCommand cmd = new()
                {
                    Connection = cnn,
                    CommandType = CommandType.StoredProcedure,
                    CommandText = "prc_get_CondicionesDeVenta"
                };
                SqlDataAdapter da = new(cmd);
                da.Fill(dt);
            }
            return dt;
        }

        public DataTable ObtenerListasDePrecio()
        {
            DataTable dt = new DataTable();
            using (SqlConnection cnn = new(ConnectionString))
            {
                SqlCommand cmd = new()
                {
                    Connection = cnn,
                    CommandType = CommandType.StoredProcedure,
                    CommandText = "prc_get_ListaDePrecios"
                };
                SqlDataAdapter da = new(cmd);
                da.Fill(dt);
            }
            return dt;
        }
        public DataTable ObtenerZonas()
        {
            DataTable dt = new DataTable();
            using (SqlConnection cnn = new(ConnectionString))
            {
                SqlCommand cmd = new()
                {
                    Connection = cnn,
                    CommandType = CommandType.StoredProcedure,
                    CommandText = "prc_get_Zonas"
                };
                SqlDataAdapter da = new(cmd);
                da.Fill(dt);
            }
            return dt;
        }
    }
}
