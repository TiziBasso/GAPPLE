using Radzen.Blazor.Rendering;
using System.Data;
using Microsoft.Data.SqlClient;
using GAPPLE.Shared.Model;

namespace GAPPLE.Server.Data
{
    public class DA_Ofertas
    {
        private string ConnectionString { get; }

        public DA_Ofertas(string connectionString) => ConnectionString = connectionString;

        public DataTable ObtenerOfertas(string? nombre, string? linea,DateTime? mes, bool? activas)
        {
            DataTable dt = new DataTable();
            using (SqlConnection cnn = new(ConnectionString))
            {
                SqlCommand cmd = new()
                {
                    Connection = cnn,
                    CommandType = CommandType.StoredProcedure,
                    CommandText = "prc_get_Ofertas"
                };
                if (nombre != null) cmd.Parameters.AddWithValue("@pNombre", nombre);
                if (linea != null) cmd.Parameters.AddWithValue("@pLinea", linea);
                if (mes != null) cmd.Parameters.AddWithValue("@pMes", mes);
                if (activas != null) cmd.Parameters.AddWithValue("@pActivas", activas);
                SqlDataAdapter da = new(cmd);
                da.Fill(dt);
            }
            return dt;
        }

        public DataTable ObtenerOfertasEspeciales(string? nombre, string? linea, DateTime? mes, bool? activas, string idCLiente)
        {
            DataTable dt = new DataTable();
            using (SqlConnection cnn = new(ConnectionString))
            {
                SqlCommand cmd = new()
                {
                    Connection = cnn,
                    CommandType = CommandType.StoredProcedure,
                    CommandText = "prc_get_OfertasEspeciales"
                };
                if (nombre != null) cmd.Parameters.AddWithValue("@pNombre", nombre);
                if (linea != null) cmd.Parameters.AddWithValue("@pLinea", linea);
                if (mes != null) cmd.Parameters.AddWithValue("@pMes", mes);
                if (activas != null) cmd.Parameters.AddWithValue("@pActivas", activas);
                if (idCLiente != null) cmd.Parameters.AddWithValue("@pCodCliente", idCLiente);
                SqlDataAdapter da = new(cmd);
                da.Fill(dt);
            }
            return dt;
        }

        public void PersistirOferta(string? nombre, string? linea, string? descripcion, decimal descuento, DateTime? desde, DateTime? hasta, string inclusiones, string altaUsuario, string codCliente = null)
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
            if (codCliente != null) cmd.Parameters.AddWithValue("@pCodCliente", codCliente);
            cmd.Parameters.AddWithValue("@pAltaUsuario", altaUsuario);
            cnn.Open();
            cmd.ExecuteNonQuery();
            cnn.Close();
        }

        public void EditarOferta(int idOferta,string? nombre, string? linea, string? descripcion, decimal descuento, DateTime? desde, DateTime? hasta, string inclusiones, string edicionUsuario, string codCliente = null, bool activa = true)
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
            cmd.Parameters.AddWithValue("@pActiva", activa);
            cmd.Parameters.AddWithValue("@pinclusiones", inclusiones);
            cmd.Parameters.AddWithValue("@pEdicionUsuario", edicionUsuario);
            if(codCliente != null) cmd.Parameters.AddWithValue("@pCodCliente", codCliente);
            cnn.Open();
            cmd.ExecuteNonQuery();
            cnn.Close();
        }
    }
}
