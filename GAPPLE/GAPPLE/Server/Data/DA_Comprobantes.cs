using System.Data.SqlClient;
using System.Data;
using GAPPLE.Shared.Model;
using GAPPLE.Shared.Enums;

namespace GAPPLE.Server.Data
{
    public class DA_Comprobantes
    {
        private string ConnectionString { get; }
        public DA_Comprobantes(string connectionString) => ConnectionString = connectionString;

        public DataTable ObtenerComprobantesCabecera(DateTime fechaDesde, DateTime fechaHasta, string codigoOrden, string codigoTango, bool? mercaderiaIngresada, int? idEstado, string razonSocial)
        {
            DataTable dt = new();
            SqlConnection cnn = new(ConnectionString);
            SqlCommand cmd = cnn.CreateCommand();

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "prc_get_ComprobantesCabecera";
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@pFechaDesde", fechaDesde);
            cmd.Parameters.AddWithValue("@pFechaHasta", fechaHasta);
            if (!string.IsNullOrWhiteSpace(codigoOrden)) cmd.Parameters.AddWithValue("@pCodigoOrden", codigoOrden);
            if (!string.IsNullOrWhiteSpace(codigoTango)) cmd.Parameters.AddWithValue("@pCodigoTango", codigoTango);
            if (mercaderiaIngresada != null) cmd.Parameters.AddWithValue("@pMercaderiaIngresada", mercaderiaIngresada);
            if (idEstado != null) cmd.Parameters.AddWithValue("@pIdEstado", idEstado);
            if (razonSocial != null) cmd.Parameters.AddWithValue("@pRazonSocialCliente", razonSocial);
            SqlDataAdapter da = new(cmd);
            da.Fill(dt);

            return dt;
        }

        public void CancelarNotaCredito(int idComprobante, string usuario)
        {
            SqlConnection cnn = new(ConnectionString);
            SqlCommand cmd = new()
            {
                Connection = cnn,
                CommandType = CommandType.StoredProcedure,
                CommandText = "prc_upd_ComprobanteCabecera"
            };
            cmd.Parameters.AddWithValue("@pIdComprobante", idComprobante);
            cmd.Parameters.AddWithValue("@pIdEstado", (int)ComprobanteCabeceraEstadoEnum.Cancelado);
            cmd.Parameters.AddWithValue("@pEdicionUsuario", usuario);
            cnn.Open();
            cmd.ExecuteNonQuery();
            cnn.Close();
        }
    }
}
