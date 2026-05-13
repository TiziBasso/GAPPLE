using Microsoft.Data.SqlClient;
using System.Data;

namespace GAPPLE.Server.Data
{
    public class DA_Tools
    {
        private string ConnectionString { get; }

        public DA_Tools(string connectionString) => ConnectionString = connectionString;

        public DataTable ObtenerEstados(string entidad = null)
        {
            DataTable dt = new DataTable();
            using (SqlConnection cnn = new(ConnectionString))
            {
                SqlCommand cmd = new()
                {
                    Connection = cnn,
                    CommandType = CommandType.StoredProcedure,
                    CommandText = "prc_get_Estados"
                };
                if (!string.IsNullOrEmpty(entidad)) cmd.Parameters.AddWithValue("@pEntidad", entidad);
                SqlDataAdapter da = new(cmd);
                da.Fill(dt);
            }
            return dt;
        }
    }
}
