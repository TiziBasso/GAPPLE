using GAPPLE.Shared.Enums;
using GAPPLE.Shared.Model;
using GAPPLE.Shared.Structs;
using MathNet.Numerics.Optimization;
using System.Data;
using System.Data.SqlClient;

namespace GAPPLE.Server.Data
{
    public class DA_Comprobantes
    {
        private string ConnectionString { get; }
        public DA_Comprobantes(string connectionString) => ConnectionString = connectionString;

        public DataTable ObtenerComprobantesCabecera(DateTime fechaDesde, DateTime fechaHasta, string codigoOrden, string codigoTango, bool? mercaderiaIngresada, ComprobanteCabeceraEstadoEnum? idEstado, string razonSocial, int? idComprobante = null)
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
            if (idEstado != null) cmd.Parameters.AddWithValue("@pIdEstado", (int)idEstado);
            if (razonSocial != null) cmd.Parameters.AddWithValue("@pRazonSocialCliente", razonSocial);
            if (idComprobante != null) cmd.Parameters.AddWithValue("@pIdComprobante", idComprobante);
            SqlDataAdapter da = new(cmd);
            da.Fill(dt);

            return dt;
        }

        public void ActualizarNotaCreditoEstado(int idComprobante, ComprobanteCabeceraEstadoEnum estado, string usuario)
        {
            SqlConnection cnn = new(ConnectionString);
            SqlCommand cmd = new()
            {
                Connection = cnn,
                CommandType = CommandType.StoredProcedure,
                CommandText = "prc_upd_ComprobanteCabecera"
            };
            cmd.Parameters.AddWithValue("@pIdComprobante", idComprobante);
            cmd.Parameters.AddWithValue("@pIdEstado", (int)estado);
            cmd.Parameters.AddWithValue("@pEdicionUsuario", usuario);
            cnn.Open();
            cmd.ExecuteNonQuery();
            cnn.Close();
        }

        public DataTable InsertarNotaCreditoCabecera(ComprobanteCabecera comprobante, SqlTransaction transaction)
        {
            DataTable dt = new();

            SqlConnection cnn = transaction.Connection;
            SqlCommand cmd = cnn.CreateCommand();
            cmd.Transaction = transaction;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "prc_ins_ComprobantesCabecera";
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@pTipoComprobante", comprobante.TipoComprobante);
            cmd.Parameters.AddWithValue("@pIdCliente", comprobante.IdCliente);
            cmd.Parameters.AddWithValue("@pIdMotivo", comprobante.IdMotivo);
            cmd.Parameters.AddWithValue("@pIdDeposito", comprobante.IdDeposito);
            cmd.Parameters.AddWithValue("@pFecha", comprobante.FechaComprobante);
            cmd.Parameters.AddWithValue("@pIdEstado", comprobante.IdEstado);
            cmd.Parameters.AddWithValue("@pCodigoOrden", comprobante.CodigoOrden);
            cmd.Parameters.AddWithValue("@pImporteTotal", comprobante.ImporteTotal);
            cmd.Parameters.AddWithValue("@pMercaderiaIngresada", comprobante.MercaderiaIngresada);
            if (!string.IsNullOrWhiteSpace(comprobante.Observaciones)) cmd.Parameters.AddWithValue("@pObservaciones", comprobante.Observaciones);
            cmd.Parameters.AddWithValue("@pComprobanteReferencia", comprobante.ComprobanteReferencia);
            cmd.Parameters.AddWithValue("@pIdListaDePrecios", comprobante.IdListaPrecio);
            cmd.Parameters.AddWithValue("@pAlternativo", comprobante.Presupuesto);
            cmd.Parameters.AddWithValue("@pAltaUsuario", comprobante.AltaUsuario);
            SqlDataAdapter da = new(cmd);
            da.Fill(dt);

            return dt;
        }

        public void InsertarNotaCreditoDetalle(ComprobanteDetalle detalle, SqlTransaction transaction)
        {
            SqlConnection cnn = transaction.Connection;
            SqlCommand cmd = cnn.CreateCommand();
            cmd.Transaction = transaction;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "prc_ins_ComprobantesDetalle";
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@pIdComprobante", detalle.IdComprobante);
            cmd.Parameters.AddWithValue("@pLinea", detalle.NumeroLinea);
            cmd.Parameters.AddWithValue("@pCodProducto", detalle.CodProducto);
            cmd.Parameters.AddWithValue("@pCantidad", detalle.Cantidad);
            cmd.Parameters.AddWithValue("@pPrecio", detalle.Precio);
            cmd.Parameters.AddWithValue("@pDescuento", detalle.Descuento);
            if (!string.IsNullOrWhiteSpace(detalle.Detalle)) cmd.Parameters.AddWithValue("@pDetalle", detalle.Detalle);
            cmd.ExecuteNonQuery();
        }

        public void EliminarNotaCreditoDetalle(int idComprobante, SqlTransaction transaction)
        {
            SqlCommand cmd = transaction.Connection.CreateCommand();
            cmd.Transaction = transaction;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "prc_del_ComprobantesDetalle";
            cmd.Parameters.AddWithValue("@pIdComprobante", idComprobante);
            cmd.ExecuteNonQuery();
        }

        public void ActualizarNotaCreditoCabecera(ComprobanteCabecera comprobante, SqlTransaction transaction)
        {
            SqlCommand cmd = transaction.Connection.CreateCommand();
            cmd.Transaction = transaction;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "prc_upd_ComprobanteCabecera2";
            cmd.Parameters.AddWithValue("@pIdComprobante", comprobante.IdComprobante);
            cmd.Parameters.AddWithValue("@pIdMotivo", comprobante.IdMotivo);
            cmd.Parameters.AddWithValue("@pIdDeposito", comprobante.IdDeposito);
            cmd.Parameters.AddWithValue("@pFecha", comprobante.FechaComprobante);
            cmd.Parameters.AddWithValue("@pImporteTotal", comprobante.ImporteTotal);
            cmd.Parameters.AddWithValue("@pMercaderiaIngresada", comprobante.MercaderiaIngresada);
            if (!string.IsNullOrWhiteSpace(comprobante.Observaciones))
                cmd.Parameters.AddWithValue("@pObservaciones", comprobante.Observaciones);
            else
                cmd.Parameters.AddWithValue("@pObservaciones", DBNull.Value);
            cmd.Parameters.AddWithValue("@pEdicionUsuario", comprobante.EdicionUsuario);
            cmd.ExecuteNonQuery();
        }


        public DataTable ObtenerComprobantesDetalle(int idComprobante)
        {
            DataTable dt = new();
            SqlConnection cnn = new(ConnectionString);
            SqlCommand cmd = cnn.CreateCommand();

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "prc_get_ComprobanteDetalle";
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@pIdComprobante", idComprobante);
            SqlDataAdapter da = new(cmd);
            da.Fill(dt);

            return dt;
        }

        public DataTable ObtenerArchivos(int idComprobante)
        {
            DataTable dt = new();
            SqlConnection cnn = new(ConnectionString);
            SqlCommand cmd = cnn.CreateCommand();

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "prc_get_archivo";
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@pIdComprobante", idComprobante);
            SqlDataAdapter da = new(cmd);
            da.Fill(dt);

            return dt;
        }

        public void InsertarArchivo(NotaCreditoArchivo archivo, SqlTransaction transaction)
        {
            SqlConnection cnn = transaction.Connection;
            SqlCommand cmd = cnn.CreateCommand();
            cmd.Transaction = transaction;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "prc_ins_archivo";
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@pIdComprobante", archivo.IdComprobante);
            cmd.Parameters.AddWithValue("@pNombreArchivo", archivo.NombreArchivo);
            cmd.Parameters.AddWithValue("@pRuta", archivo.Path);
            cmd.Parameters.AddWithValue("@pTipoMime", archivo.TipoArchivo);
            cmd.ExecuteNonQuery();
        }

        public void DeleteArchivo(int idArchivo, int idcomprobante, SqlTransaction transaction)
        {
            SqlConnection cnn = transaction.Connection;
            SqlCommand cmd = cnn.CreateCommand();
            cmd.Transaction = transaction;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "prc_del_archivo";
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@pIdArchivo", idArchivo);
            cmd.Parameters.AddWithValue("@pIdComprobante", idcomprobante);
            cmd.ExecuteNonQuery();
        }
    }
}
