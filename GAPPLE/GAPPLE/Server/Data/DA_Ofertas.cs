using System.Data;
using System.Data.SqlClient;

namespace GAPPLE.Server.Data
{
    public class DA_Ofertas
    {
        private string ConnectionString { get; }

        public DA_Ofertas(string connectionString) => ConnectionString = connectionString;

        public DataTable ObtenerOfertas(string? nombre, string? linea,DateTime? desde, DateTime? hasta)
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
                if (desde != null) cmd.Parameters.AddWithValue("@pDesde", desde);
                if (hasta != null) cmd.Parameters.AddWithValue("@pHasta", hasta);
                SqlDataAdapter da = new(cmd);
                da.Fill(dt);
            }
            return dt;
        }
    }
}
