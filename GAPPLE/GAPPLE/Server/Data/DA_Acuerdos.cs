using GAPPLE.Shared.Model;
using GAPPLE.Shared.Requests;
using System.Data;
using System.Data.SqlClient;

namespace GAPPLE.Server.Data
{
    public class DA_Acuerdos
    {
        private string ConnectionString { get; }

        public DA_Acuerdos(string connectionString) => ConnectionString = connectionString;

        public DataTable ObtenerAcuerdos(AcuerdosRequest request, SqlTransaction? transaction = null)
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
            cmd.CommandText = "prc_get_Acuerdos";
            cmd.Parameters.AddWithValue("@pFechaDesde", request.FechaDesde);
            cmd.Parameters.AddWithValue("@pFechaHasta", request.FechaHasta);
            cmd.Parameters.AddWithValue("@pIdCliente", request.IdCliente ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@pCodCliente", string.IsNullOrEmpty(request.CodCliente) ? DBNull.Value : request.CodCliente);
            cmd.Parameters.AddWithValue("@pRazonSocial", string.IsNullOrEmpty(request.RazonSocial) ? DBNull.Value : request.RazonSocial);
            cmd.Parameters.AddWithValue("@pCUIT", string.IsNullOrEmpty(request.CUIT) ? DBNull.Value : request.CUIT);
            cmd.Parameters.AddWithValue("@pLinea", string.IsNullOrEmpty(request.Linea) ? DBNull.Value : request.Linea);
            cmd.Parameters.AddWithValue("@pActivo", request.Activo.HasValue ? request.Activo.Value : DBNull.Value);
            SqlDataAdapter dataAdapter = new(cmd);
            dataAdapter.Fill(dt);

            return dt;
        }

        public void EditarAcuerdo(Acuerdo acuerdo)
        {
            SqlConnection cnn = new(ConnectionString);
            SqlCommand cmd = new()
            {
                Connection = cnn,
                CommandType = CommandType.StoredProcedure,
                CommandText = "prc_upd_Acuerdo"
            };
            cmd.Parameters.AddWithValue("@pIdAcuerdo", acuerdo.IdAcuerdo);
            cmd.Parameters.AddWithValue("@pLinea", string.IsNullOrWhiteSpace(acuerdo.Linea) ? DBNull.Value : acuerdo.Linea);
            cmd.Parameters.AddWithValue("@pCondicion", string.IsNullOrWhiteSpace(acuerdo.Condicion) ? DBNull.Value : acuerdo.Condicion);
            cmd.Parameters.AddWithValue("@pFechaDesde", acuerdo.FechaDesde.HasValue ? acuerdo.FechaDesde : DBNull.Value);
            cmd.Parameters.AddWithValue("@pFechaHasta", acuerdo.FechaHasta.HasValue ? acuerdo.FechaHasta : DBNull.Value);
            cmd.Parameters.AddWithValue("@pActivo", acuerdo.Activo);
            cmd.Parameters.AddWithValue("@pEdicionUsuario", acuerdo.EdicionUsuario);

            cnn.Open();
            cmd.ExecuteNonQuery();
            cnn.Close();
        }

        public int InsertarAcuerdo(Acuerdo acuerdo)
        {
            int idAcuerdo = 0;
            DataTable dt = new();
            SqlConnection cnn;
            SqlCommand cmd = new();
            cnn = new(ConnectionString);
            cmd.Connection = cnn;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "prc_ins_Acuerdo";
            cmd.Parameters.AddWithValue("@pIdCliente", acuerdo.IdCliente);
            cmd.Parameters.AddWithValue("@pLinea", acuerdo.Linea);
            cmd.Parameters.AddWithValue("@pCondicion", acuerdo.Condicion);
            cmd.Parameters.AddWithValue("@pFechaDesde", acuerdo.FechaDesde);
            cmd.Parameters.AddWithValue("@pFechaHasta", acuerdo.FechaHasta);
            cmd.Parameters.AddWithValue("@pActivo", acuerdo.Activo);
            cmd.Parameters.AddWithValue("@pAltaUsuario", acuerdo.AltaUsuario);
            SqlDataAdapter da = new(cmd);
            da.Fill(dt);
            idAcuerdo = int.Parse(dt.Rows[0]["IdMotivo"].ToString()!);

            return idAcuerdo;
        }
    }
}
