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

        // ─── Insertar cabecera ────────────────────────────────────────────────────
        public int InsertarReclamo(Reclamo reclamo)
        {
            DataTable dt = new();
            using SqlConnection cnn = new(ConnectionString);
            SqlCommand cmd = new()
            {
                Connection   = cnn,
                CommandType  = CommandType.StoredProcedure,
                CommandText  = "prc_ins_Reclamo"
            };
            cmd.Parameters.AddWithValue("@pFecha",      reclamo.Fecha.Date);
            cmd.Parameters.AddWithValue("@pIdCliente",  reclamo.IdCliente!);
            cmd.Parameters.AddWithValue("@pTipo",       (int)reclamo.Tipo!);
            cmd.Parameters.AddWithValue("@pMotivo",     (int)reclamo.Motivo!);
            if (!string.IsNullOrWhiteSpace(reclamo.CodPedido))
                cmd.Parameters.AddWithValue("@pCodPedido", reclamo.CodPedido);
            if (!string.IsNullOrWhiteSpace(reclamo.Descripcion))
                cmd.Parameters.AddWithValue("@pDescripcion", reclamo.Descripcion);
            if (!string.IsNullOrWhiteSpace(reclamo.Resolucion))
                cmd.Parameters.AddWithValue("@pResolucion", reclamo.Resolucion);
            cmd.Parameters.AddWithValue("@pAltaUsuario", reclamo.AltaUsuario);
            SqlDataAdapter da = new(cmd);
            da.Fill(dt);
            return int.Parse(dt.Rows[0]["IdReclamo"].ToString()!);
        }

        // ─── Insertar una línea de detalle ────────────────────────────────────────
        public void InsertarReclamoDetalle(int idReclamo, ReclamoDetalle detalle)
        {
            using SqlConnection cnn = new(ConnectionString);
            SqlCommand cmd = new()
            {
                Connection  = cnn,
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
            cnn.Open();
            cmd.ExecuteNonQuery();
        }

        // ─── Actualizar cabecera ──────────────────────────────────────────────────
        public void ActualizarReclamo(Reclamo reclamo)
        {
            using SqlConnection cnn = new(ConnectionString);
            SqlCommand cmd = new()
            {
                Connection  = cnn,
                CommandType = CommandType.StoredProcedure,
                CommandText = "prc_upd_Reclamo"
            };
            cmd.Parameters.AddWithValue("@pIdReclamo",     reclamo.IdReclamo);
            cmd.Parameters.AddWithValue("@pIdCliente",     reclamo.IdCliente!);
            cmd.Parameters.AddWithValue("@pTipo",          (int)reclamo.Tipo!);
            cmd.Parameters.AddWithValue("@pMotivo",        (int)reclamo.Motivo!);
            cmd.Parameters.AddWithValue("@pEdicionUsuario", reclamo.EdicionUsuario);
            if (!string.IsNullOrWhiteSpace(reclamo.CodPedido))
                cmd.Parameters.AddWithValue("@pCodPedido", reclamo.CodPedido);
            if (!string.IsNullOrWhiteSpace(reclamo.Descripcion))
                cmd.Parameters.AddWithValue("@pDescripcion", reclamo.Descripcion);
            if (!string.IsNullOrWhiteSpace(reclamo.Resolucion))
                cmd.Parameters.AddWithValue("@pResolucion", reclamo.Resolucion);
            cnn.Open();
            cmd.ExecuteNonQuery();
        }

        // ─── Eliminar detalle de un reclamo (para reimportar al editar) ───────────
        public void EliminarReclamoDetalle(int idReclamo)
        {
            using SqlConnection cnn = new(ConnectionString);
            SqlCommand cmd = new()
            {
                Connection  = cnn,
                CommandType = CommandType.StoredProcedure,
                CommandText = "prc_del_ReclamoDetalle"
            };
            cmd.Parameters.AddWithValue("@pIdReclamo", idReclamo);
            cnn.Open();
            cmd.ExecuteNonQuery();
        }

        // ─── Eliminar cabecera ────────────────────────────────────────────────────
        public void EliminarReclamo(int idReclamo)
        {
            using SqlConnection cnn = new(ConnectionString);
            SqlCommand cmd = new()
            {
                Connection  = cnn,
                CommandType = CommandType.StoredProcedure,
                CommandText = "prc_del_Reclamo"
            };
            cmd.Parameters.AddWithValue("@pIdReclamo", idReclamo);
            cnn.Open();
            cmd.ExecuteNonQuery();
        }
    }
}
