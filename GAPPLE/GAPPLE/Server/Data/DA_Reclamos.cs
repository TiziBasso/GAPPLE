using GAPPLE.Shared.Model;
using Microsoft.Data.SqlClient;
using System.Data;

namespace GAPPLE.Server.Data
{
    public class DA_Reclamos
    {
        private string ConnectionString { get; }
        public DA_Reclamos(string connectionString) => ConnectionString = connectionString;

        // ─── Obtener lista de reclamos (cabecera) ────────────────────────────────
        public DataTable ObtenerReclamos(DateTime fechaDesde, DateTime fechaHasta,
            string razonSocialCliente, int? tipo, int? motivo)
        {
            DataTable dt = new();
            using SqlConnection cnn = new(ConnectionString);
            SqlCommand cmd = cnn.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "prc_get_Reclamos";
            cmd.Parameters.AddWithValue("@pFechaDesde", fechaDesde.Date);
            cmd.Parameters.AddWithValue("@pFechaHasta", fechaHasta.Date);
            if (!string.IsNullOrWhiteSpace(razonSocialCliente))
                cmd.Parameters.AddWithValue("@pRazonSocial", razonSocialCliente);
            if (tipo != null)   cmd.Parameters.AddWithValue("@pTipo",   tipo);
            if (motivo != null) cmd.Parameters.AddWithValue("@pMotivo", motivo);
            SqlDataAdapter da = new(cmd);
            da.Fill(dt);
            return dt;
        }

        // ─── Dataset plano para el dashboard (una fila por linea de detalle) ─────
        public DataTable ObtenerReclamosDashboard(DateTime fechaDesde, DateTime fechaHasta)
        {
            DataTable dt = new();
            using SqlConnection cnn = new(ConnectionString);
            SqlCommand cmd = cnn.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "prc_get_ReclamosDashboard";
            cmd.CommandTimeout = 120;
            cmd.Parameters.AddWithValue("@pFechaDesde", fechaDesde.Date);
            cmd.Parameters.AddWithValue("@pFechaHasta", fechaHasta.Date);
            SqlDataAdapter da = new(cmd);
            da.Fill(dt);
            return dt;
        }

        // ─── Obtener detalle (SKUs) de un reclamo ────────────────────────────────
        public DataTable ObtenerReclamoDetalle(int idReclamo)
        {
            DataTable dt = new();
            using SqlConnection cnn = new(ConnectionString);
            SqlCommand cmd = cnn.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "prc_get_ReclamoDetalle";
            cmd.Parameters.AddWithValue("@pIdReclamo", idReclamo);
            SqlDataAdapter da = new(cmd);
            da.Fill(dt);
            return dt;
        }

        // ─── Insertar cabecera (dentro de una transacción) ────────────────────────
        public int InsertarReclamo(Reclamo reclamo, SqlTransaction transaction)
        {
            DataTable dt = new();
            SqlCommand cmd = new()
            {
                Connection  = transaction.Connection,
                Transaction = transaction,
                CommandType = CommandType.StoredProcedure,
                CommandText = "prc_ins_Reclamo"
            };
            cmd.Parameters.AddWithValue("@pFecha",      reclamo.Fecha.Date);
            cmd.Parameters.AddWithValue("@pCodCliente",  reclamo.CodigoCliente!);
            cmd.Parameters.AddWithValue("@pTipo",       (int)reclamo.Tipo!);
            cmd.Parameters.AddWithValue("@pMotivo",     (int)reclamo.Motivo!);
            if (!string.IsNullOrWhiteSpace(reclamo.NumeroFactura))
                cmd.Parameters.AddWithValue("@pCodPedido", reclamo.NumeroFactura);
            if (!string.IsNullOrWhiteSpace(reclamo.Descripcion))
                cmd.Parameters.AddWithValue("@pDescripcion", reclamo.Descripcion);
            if (!string.IsNullOrWhiteSpace(reclamo.Resolucion))
                cmd.Parameters.AddWithValue("@pResolucion", reclamo.Resolucion);
            if (!string.IsNullOrWhiteSpace(reclamo.NFAC))
                cmd.Parameters.AddWithValue("@pNFAC", reclamo.NFAC);
            cmd.Parameters.AddWithValue("@pAltaUsuario", reclamo.AltaUsuario);
            SqlDataAdapter da = new(cmd);
            da.Fill(dt);
            return int.Parse(dt.Rows[0]["IdReclamo"].ToString()!);
        }

        // ─── Insertar una línea de detalle (dentro de una transacción) ────────────
        public void InsertarReclamoDetalle(int idReclamo, ReclamoDetalle detalle, SqlTransaction transaction)
        {
            SqlCommand cmd = new()
            {
                Connection  = transaction.Connection,
                Transaction = transaction,
                CommandType = CommandType.StoredProcedure,
                CommandText = "prc_ins_ReclamoDetalle"
            };
            cmd.Parameters.AddWithValue("@pIdReclamo", idReclamo);
            cmd.Parameters.AddWithValue("@pSKU",       detalle.SKU);
            cmd.Parameters.AddWithValue("@pCantidad",  detalle.Cantidad);
            if (!string.IsNullOrWhiteSpace(detalle.DescripcionProducto))
                cmd.Parameters.AddWithValue("@pDescripcionProducto", detalle.DescripcionProducto);
            if (!string.IsNullOrWhiteSpace(detalle.Lote))
                cmd.Parameters.AddWithValue("@pLote", detalle.Lote);
            if (detalle.Vencimiento != null)
                cmd.Parameters.AddWithValue("@pVencimiento", detalle.Vencimiento.Value.Date);
            cmd.ExecuteNonQuery();
        }

        // ─── Actualizar cabecera (dentro de una transacción) ──────────────────────
        public void ActualizarReclamo(Reclamo reclamo, SqlTransaction transaction)
        {
            SqlCommand cmd = new()
            {
                Connection  = transaction.Connection,
                Transaction = transaction,
                CommandType = CommandType.StoredProcedure,
                CommandText = "prc_upd_Reclamo"
            };
            cmd.Parameters.AddWithValue("@pIdReclamo",     reclamo.IdReclamo);
            cmd.Parameters.AddWithValue("@pCodCliente",     reclamo.CodigoCliente!);
            cmd.Parameters.AddWithValue("@pTipo",          (int)reclamo.Tipo!);
            cmd.Parameters.AddWithValue("@pMotivo",        (int)reclamo.Motivo!);
            cmd.Parameters.AddWithValue("@pEdicionUsuario", reclamo.EdicionUsuario);
            if (!string.IsNullOrWhiteSpace(reclamo.NumeroFactura))
                cmd.Parameters.AddWithValue("@pCodPedido", reclamo.NumeroFactura);
            if (!string.IsNullOrWhiteSpace(reclamo.Descripcion))
                cmd.Parameters.AddWithValue("@pDescripcion", reclamo.Descripcion);
            if (!string.IsNullOrWhiteSpace(reclamo.Resolucion))
                cmd.Parameters.AddWithValue("@pResolucion", reclamo.Resolucion);
            cmd.ExecuteNonQuery();
        }

        // ─── Eliminar detalle de un reclamo (dentro de una transacción) ───────────
        public void EliminarReclamoDetalle(int idReclamo, SqlTransaction transaction)
        {
            SqlCommand cmd = new()
            {
                Connection  = transaction.Connection,
                Transaction = transaction,
                CommandType = CommandType.StoredProcedure,
                CommandText = "prc_del_ReclamoDetalle"
            };
            cmd.Parameters.AddWithValue("@pIdReclamo", idReclamo);
            cmd.ExecuteNonQuery();
        }

        // ─── Eliminar cabecera (dentro de una transacción) ────────────────────────
        public void EliminarReclamo(int idReclamo, SqlTransaction transaction)
        {
            SqlCommand cmd = new()
            {
                Connection  = transaction.Connection,
                Transaction = transaction,
                CommandType = CommandType.StoredProcedure,
                CommandText = "prc_del_Reclamo"
            };
            cmd.Parameters.AddWithValue("@pIdReclamo", idReclamo);
            cmd.ExecuteNonQuery();
        }
    }
}
