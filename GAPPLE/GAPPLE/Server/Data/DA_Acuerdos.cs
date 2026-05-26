using GAPPLE.Shared.Model;
using GAPPLE.Shared.Requests;
using System.Data;
using Microsoft.Data.SqlClient;

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
            if (request.IdAcuerdo != null) cmd.Parameters.AddWithValue("@pIdAcuerdo", request.IdAcuerdo);
            if (request.FechaDesde != null) cmd.Parameters.AddWithValue("@pFechaDesde", request.FechaDesde);
            if (request.FechaHasta != null) cmd.Parameters.AddWithValue("@pFechaHasta", request.FechaHasta);
            if (request.IdEstado != null) cmd.Parameters.AddWithValue("@pIdAcuerdo", request.IdAcuerdo);
            if (request.IdCliente != null) cmd.Parameters.AddWithValue("@pIdCliente", request.IdCliente);
            if (!string.IsNullOrEmpty(request.CodCliente)) cmd.Parameters.AddWithValue("@pCodCliente", $"%{request.CodCliente}%");
            if (!string.IsNullOrEmpty(request.RazonSocial)) cmd.Parameters.AddWithValue("@pRazonSocial", $"%{request.RazonSocial}%");
            if (!string.IsNullOrEmpty(request.CUIT)) cmd.Parameters.AddWithValue("@pCUIT", $"%{request.CUIT}%");
            if (!string.IsNullOrEmpty(request.Linea)) cmd.Parameters.AddWithValue("@pLinea", $"%{request.Linea}%");
            if (request.IdEstado != null) cmd.Parameters.AddWithValue("@pIdEstado", request.IdEstado);
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
            if (!string.IsNullOrEmpty(acuerdo.Linea)) cmd.Parameters.AddWithValue("@pLinea", acuerdo.Linea);
            if (!string.IsNullOrEmpty(acuerdo.Condicion)) cmd.Parameters.AddWithValue("@pCondicion", acuerdo.Condicion);
            if (acuerdo.FechaDesde != null) cmd.Parameters.AddWithValue("@pFechaDesde", acuerdo.FechaDesde);
            if (acuerdo.FechaHasta != null) cmd.Parameters.AddWithValue("@pFechaHasta", acuerdo.FechaHasta);
            if (acuerdo.IdEstado != null) cmd.Parameters.AddWithValue("@pIdEstado", acuerdo.IdEstado);
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
            if (!string.IsNullOrEmpty(acuerdo.Linea)) cmd.Parameters.AddWithValue("@pLinea", acuerdo.Linea);
            cmd.Parameters.AddWithValue("@pCondicion", acuerdo.Condicion);
            cmd.Parameters.AddWithValue("@pFechaDesde", acuerdo.FechaDesde);
            cmd.Parameters.AddWithValue("@pFechaHasta", acuerdo.FechaHasta);
            cmd.Parameters.AddWithValue("@pIdEstado", acuerdo.IdEstado);
            cmd.Parameters.AddWithValue("@pAltaUsuario", acuerdo.AltaUsuario);
            SqlDataAdapter da = new(cmd);
            da.Fill(dt);
            idAcuerdo = int.Parse(dt.Rows[0]["IdAcuerdo"].ToString()!);

            return idAcuerdo;
        }

        public void BorrarAcuerdo(int idAcuerdo)
        {
            SqlConnection cnn = new(ConnectionString);
            SqlCommand cmd = cnn.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "prc_del_acuerdo";
            cmd.Parameters.AddWithValue("@pIdAcuerdo", idAcuerdo);
            cnn.Open();
            cmd.ExecuteNonQuery();
            cnn.Close();
        }

        #region AcuerdosMontos
        public DataTable ObtenerAcuerdoMontos(AcuerdoMontosRequest request, SqlTransaction? transaction)
        {
            SqlConnection cnn;
            SqlCommand cmd;

            if (transaction == null)
            {
                cnn = new(ConnectionString);
                cmd = cnn.CreateCommand();
            }
            else
            {
                cnn = transaction.Connection;
                cmd = cnn.CreateCommand();
                cmd.Transaction = transaction;
            }

            DataTable dt = new();
            cmd.Parameters.Clear();
            cmd.Connection = cnn;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "prc_get_AcuerdosMontos";
            if (request.Id != null) cmd.Parameters.AddWithValue("@pId", request.Id);
            if (request.IdAcuerdo != null) cmd.Parameters.AddWithValue("@pIdAcuerdo", request.IdAcuerdo);
            if (request.FechaDesde != null) cmd.Parameters.AddWithValue("@pFechaDesde", request.FechaDesde);
            if (request.FechaHastaFinDia != null) cmd.Parameters.AddWithValue("@pFechaHasta", request.FechaHastaFinDia);
            if (request.IdCliente != null) cmd.Parameters.AddWithValue("@pIdCliente", request.IdCliente);
            if (request.CodClienteLike != null) cmd.Parameters.AddWithValue("@pCodCliente", request.CodClienteLike);
            if (request.RazonSocialLike != null) cmd.Parameters.AddWithValue("@pRazonSocial", request.RazonSocialLike);
            if (request.CUITLike != null) cmd.Parameters.AddWithValue("@pCUIT", request.CUITLike);
            if (request.LineaLike != null) cmd.Parameters.AddWithValue("@pLinea", request.LineaLike);
            SqlDataAdapter dataAdapter = new(cmd);
            dataAdapter.Fill(dt);

            return dt;
        }

        public int InsertarAcuerdoMonto(AcuerdoMonto acuerdoMonto)
        {
            int id = 0;
            DataTable dt = new();
            SqlConnection cnn;
            SqlCommand cmd = new();
            cnn = new(ConnectionString);
            cmd.Connection = cnn;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "prc_ins_AcuerdosMonto";
            cmd.Parameters.AddWithValue("@pIdAcuerdo", acuerdoMonto.IdAcuerdo);
            cmd.Parameters.AddWithValue("@pFecha", acuerdoMonto.Fecha);
            cmd.Parameters.AddWithValue("@pMonto", acuerdoMonto.Monto);
            if (acuerdoMonto.IdComprobante != null) cmd.Parameters.AddWithValue("@pIdComprobante", acuerdoMonto.IdComprobante);
            if (acuerdoMonto.IdPedido != null) cmd.Parameters.AddWithValue("@pIdPedido", acuerdoMonto.IdPedido);
            if (acuerdoMonto.Notas != null) cmd.Parameters.AddWithValue("@pNotas", acuerdoMonto.Notas);
            cmd.Parameters.AddWithValue("@pAltaUsuario", acuerdoMonto.AltaUsuario);
            SqlDataAdapter da = new(cmd);
            da.Fill(dt);
            id = int.Parse(dt.Rows[0]["Id"].ToString()!);

            return id;
        }

        public void EditarAcuerdosMonto(AcuerdoMonto acuerdoMonto)
        {
            SqlConnection cnn = new(ConnectionString);
            SqlCommand cmd = new()
            {
                Connection = cnn,
                CommandType = CommandType.StoredProcedure,
                CommandText = "prc_upd_AcuerdosMonto"
            };
            cmd.Parameters.AddWithValue("@pId", acuerdoMonto.Id);
            cmd.Parameters.AddWithValue("@pFecha", acuerdoMonto.Fecha);
            cmd.Parameters.AddWithValue("@pMonto", acuerdoMonto.Monto);
            if (acuerdoMonto.IdComprobante != null) cmd.Parameters.AddWithValue("@pIdComprobante", acuerdoMonto.IdComprobante);
            if (acuerdoMonto.IdPedido != null) cmd.Parameters.AddWithValue("@pIdPedido", acuerdoMonto.IdPedido);
            if (!string.IsNullOrEmpty(acuerdoMonto.Notas)) cmd.Parameters.AddWithValue("@pNotas", acuerdoMonto.Notas);
            cmd.Parameters.AddWithValue("@pEdicionUsuario", acuerdoMonto.EdicionUsuario);

            cnn.Open();
            cmd.ExecuteNonQuery();
            cnn.Close();
        }

        public void EliminarAcuerdoMonto(int idAcuerdoMonto)
        {
            SqlConnection cnn = new(ConnectionString);
            SqlCommand cmd = cnn.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "prc_del_AcuerdosMonto";
            cmd.Parameters.AddWithValue("@pId", idAcuerdoMonto);

            cnn.Open();
            cmd.ExecuteNonQuery();
            cnn.Close();
        }
        #endregion
    }
}
