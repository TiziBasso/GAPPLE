using System.Data;
using Microsoft.Data.SqlClient;

namespace GAPPLE.Server.Data
{
    public class DA_Stock
    {
        private string ConnectionString { get; }

        public DA_Stock(string connectionString) => ConnectionString = connectionString;

        /// <summary>
        /// Stock consolidado por producto (depositos 01 y 6 ya pivoteados por el SP).
        /// Todos los parametros son opcionales.
        /// </summary>
        public DataTable ObtenerStock(string codigo, string descripcion, string marca)
        {
            DataTable dt = new();
            using SqlConnection cnn = new(ConnectionString);
            SqlCommand cmd = cnn.CreateCommand();

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "prc_get_Stock";
            cmd.CommandTimeout = 120;
            cmd.Parameters.Clear();
            if (!string.IsNullOrWhiteSpace(codigo)) cmd.Parameters.AddWithValue("@pCodigo", codigo);
            if (!string.IsNullOrWhiteSpace(descripcion)) cmd.Parameters.AddWithValue("@pDescripcion", descripcion);
            if (!string.IsNullOrWhiteSpace(marca)) cmd.Parameters.AddWithValue("@pMarca", marca);

            SqlDataAdapter da = new(cmd);
            da.Fill(dt);

            return dt;
        }
    }
}
