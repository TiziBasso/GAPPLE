using GAPPLE.Shared.Model;
using Microsoft.Extensions.Hosting;
using Radzen.Blazor.Rendering;
using System.Data;
using System.Data.SqlClient;

namespace GAPPLE.Server.Data
{
    public class DA_Ordenes
    {
        private string ConnectionString { get; }

        public DA_Ordenes(string connectionString) => ConnectionString = connectionString;

        public DataTable ObtenerOrdenes(DateTime desde, DateTime hasta, int? idPedido, string? codOrden, bool? presupuesto, string? razonSocial,
                                        string? linea, string? zona, int? idEstado, string? codTango, int idUsuario)
        {
            DataTable dt = new DataTable();
            using (SqlConnection cnn = new(ConnectionString))
            {
                SqlCommand cmd = new()
                {
                    Connection = cnn,
                    CommandType = CommandType.StoredProcedure,
                    CommandText = "prc_get_PedidosCabecera"
                };
                cmd.Parameters.AddWithValue("@pDesde", desde);
                cmd.Parameters.AddWithValue("@pHasta", hasta);
                cmd.Parameters.AddWithValue("@pIdUsuario", idUsuario);
                if (idPedido != null) cmd.Parameters.AddWithValue("@pIdPedido", idPedido);
                if (presupuesto != null) cmd.Parameters.AddWithValue("@pPresupuesto", presupuesto);
                if (!string.IsNullOrEmpty(razonSocial)) cmd.Parameters.AddWithValue("@pRazonSocial", razonSocial);
                if (!string.IsNullOrEmpty(linea)) cmd.Parameters.AddWithValue("@pLinea", linea);
                if (!string.IsNullOrEmpty(zona)) cmd.Parameters.AddWithValue("@pCodZona", zona);
                if (!string.IsNullOrEmpty(codOrden)) cmd.Parameters.AddWithValue("@pCodOrden", codOrden);
                if (idEstado != null) cmd.Parameters.AddWithValue("@pIdEstado", idEstado);
                if (!string.IsNullOrEmpty(codTango)) cmd.Parameters.AddWithValue("@pCodTango", codTango);
                SqlDataAdapter da = new(cmd);
                da.Fill(dt);
            }
            return dt;
        }

        public DataTable ObtenerOrden(string? codOrden, int? idPedido, SqlTransaction? trans = null)
        {
            DataTable dt = new DataTable();
            SqlConnection cnn;
            SqlCommand cmd;
            if (trans != null)
            {
                cnn = trans.Connection;
                cmd = cnn.CreateCommand();
                cmd.Transaction = trans;
            }
            else
            {
                cnn = new(ConnectionString);
                cmd = cnn.CreateCommand();
            }
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "prc_get_PedidosCabecera";
            if (codOrden != null) cmd.Parameters.AddWithValue("@pCodOrden", codOrden);
            if (idPedido != null) cmd.Parameters.AddWithValue("@pIdPedido", idPedido);
            SqlDataAdapter da = new(cmd);
            da.Fill(dt);
            return dt;
        }

        public DataTable ObtenerEstados(string? entidad = null)
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

        public DataTable ObtenerOrdenDetalle(string codOrden)
        {
            DataTable dt = new DataTable();
            using (SqlConnection cnn = new(ConnectionString))
            {
                SqlCommand cmd = new()
                {
                    Connection = cnn,
                    CommandType = CommandType.StoredProcedure,
                    CommandText = "prc_get_PedidosDetalle"
                };
                cmd.Parameters.AddWithValue("@pCodOrden", codOrden);
                SqlDataAdapter da = new(cmd);
                da.Fill(dt);
            }
            return dt;
        }

        public DataTable ObtenerOrdenesDashboard()
        {
            DataTable dt = new DataTable();
            using (SqlConnection cnn = new(ConnectionString))
            {
                SqlCommand cmd = new()
                {
                    Connection = cnn,
                    CommandType = CommandType.StoredProcedure,
                    CommandText = "prc_get_OrdenesDashboard"
                };
                SqlDataAdapter da = new(cmd);
                da.Fill(dt);
            }
            return dt;
        }

        public DataTable ObtenerTransportes()
        {
            DataTable dt = new DataTable();
            using (SqlConnection cnn = new(ConnectionString))
            {
                SqlCommand cmd = new()
                {
                    Connection = cnn,
                    CommandType = CommandType.StoredProcedure,
                    CommandText = "prc_get_Transportes"
                };
                SqlDataAdapter da = new(cmd);
                da.Fill(dt);
            }
            return dt;
        }

        public DataTable ObtenerCondicionesDeVenta()
        {
            DataTable dt = new DataTable();
            using (SqlConnection cnn = new(ConnectionString))
            {
                SqlCommand cmd = new()
                {
                    Connection = cnn,
                    CommandType = CommandType.StoredProcedure,
                    CommandText = "prc_get_CondicionesDeVenta"
                };
                SqlDataAdapter da = new(cmd);
                da.Fill(dt);
            }
            return dt;
        }

        public DataTable ObtenerListasDePrecio()
        {
            DataTable dt = new DataTable();
            using (SqlConnection cnn = new(ConnectionString))
            {
                SqlCommand cmd = new()
                {
                    Connection = cnn,
                    CommandType = CommandType.StoredProcedure,
                    CommandText = "prc_get_ListaDePrecios"
                };
                SqlDataAdapter da = new(cmd);
                da.Fill(dt);
            }
            return dt;
        }
        public DataTable ObtenerZonas()
        {
            DataTable dt = new DataTable();
            using (SqlConnection cnn = new(ConnectionString))
            {
                SqlCommand cmd = new()
                {
                    Connection = cnn,
                    CommandType = CommandType.StoredProcedure,
                    CommandText = "prc_get_Zonas"
                };
                SqlDataAdapter da = new(cmd);
                da.Fill(dt);
            }
            return dt;
        }

        public int PersistirPedidoCabecera(string codOrden, string linea, string codigoCliente, int cantLineas, int idEstado, string zona, string listaPrecio,
                                            bool factura, bool presupuesto, string codTransporte, string condicionVenta, string entregarEn,
                                            string observaciones, DateTime? fechaEntrega, string altaUsuario, SqlTransaction transaction)
        {
            int id;
            SqlConnection cnn = transaction.Connection;
            SqlCommand cmd = cnn.CreateCommand();
            cmd.Transaction = transaction;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "prc_ins_PedidosCabecera";
            cmd.Parameters.AddWithValue("@pCodigoOrden", codOrden);
            cmd.Parameters.AddWithValue("@pLinea", linea);
            cmd.Parameters.AddWithValue("@pCodigoCliente", codigoCliente);
            cmd.Parameters.AddWithValue("@pCantidadLineas", cantLineas);
            cmd.Parameters.AddWithValue("@pIdEstado", idEstado);
            cmd.Parameters.AddWithValue("@pZona", zona);
            cmd.Parameters.AddWithValue("@pListaDePrecios", listaPrecio);
            cmd.Parameters.AddWithValue("@pFactura", factura);
            cmd.Parameters.AddWithValue("@pPresupuesto", presupuesto);
            cmd.Parameters.AddWithValue("@pCodTransporte", codTransporte);
            cmd.Parameters.AddWithValue("@pCondicionVenta", condicionVenta);
            cmd.Parameters.AddWithValue("@pEntregarEn", entregarEn);
            cmd.Parameters.AddWithValue("@pObservaciones", observaciones);
            cmd.Parameters.AddWithValue("@pFechaEntrega", fechaEntrega);
            cmd.Parameters.AddWithValue("@pAltaUsuario", altaUsuario);
            id = (int)cmd.ExecuteScalar();
            return id;
        }

        public void UpdatePedidoCabecera(string codOrden, string linea, string codigoCliente, int cantLineas, int idEstado, string zona, string listaPrecio,
                                            bool factura, bool presupuesto, string codTransporte, string condicionVenta, string entregarEn,
                                            string observaciones, DateTime? fechaEntrega, string altaUsuario, SqlTransaction transaction)
        {
            SqlConnection cnn = transaction.Connection;
            SqlCommand cmd = cnn.CreateCommand();
            cmd.Transaction = transaction;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "prc_upd_PedidoCabeceraCompleto";
            cmd.Parameters.AddWithValue("@pCodigoOrden", codOrden);
            cmd.Parameters.AddWithValue("@pLinea", linea);
            cmd.Parameters.AddWithValue("@pCodigoCliente", codigoCliente);
            cmd.Parameters.AddWithValue("@pCantidadLineas", cantLineas);
            cmd.Parameters.AddWithValue("@pIdEstado", idEstado);
            cmd.Parameters.AddWithValue("@pZona", zona);
            cmd.Parameters.AddWithValue("@pListaDePrecios", listaPrecio);
            cmd.Parameters.AddWithValue("@pFactura", factura);
            cmd.Parameters.AddWithValue("@pPresupuesto", presupuesto);
            cmd.Parameters.AddWithValue("@pCodTransporte", codTransporte);
            cmd.Parameters.AddWithValue("@pCondicionVenta", condicionVenta);
            cmd.Parameters.AddWithValue("@pEntregarEn", entregarEn);
            cmd.Parameters.AddWithValue("@pObservaciones", observaciones);
            cmd.Parameters.AddWithValue("@pFechaEntrega", fechaEntrega);
            cmd.Parameters.AddWithValue("@pAltaUsuario", altaUsuario);
            cmd.ExecuteNonQuery();
        }

        public void EliminarPedidoCabecera(string codOrden, SqlTransaction transaction)
        {
            SqlConnection cnn = transaction.Connection;
            SqlCommand cmd = cnn.CreateCommand();
            cmd.Transaction = transaction;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "prc_del_PedidoCabeceraCompleto";
            cmd.Parameters.AddWithValue("@pCodigoOrden", codOrden);
            cmd.ExecuteNonQuery();
        }

        public void EliminarPedidoDetalle(string codOrden, SqlTransaction transaction)
        {
            SqlConnection cnn = transaction.Connection;
            SqlCommand cmd = cnn.CreateCommand();
            cmd.Transaction = transaction;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "prc_del_PedidosDetalleCompleto";
            cmd.Parameters.AddWithValue("@pCodigoOrden", codOrden);
            cmd.ExecuteNonQuery();
        }

        public void PersistirPedidoDetalle(string codOrden, int numLinea, string codProducto, int cantidad, int probador, decimal descuento, SqlTransaction transaction)
        {
            SqlConnection cnn = transaction.Connection;
            SqlCommand cmd = cnn.CreateCommand();
            cmd.Transaction = transaction;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "prc_ins_PedidosDetalle";
            cmd.Parameters.AddWithValue("@pCodigoOrden", codOrden);
            cmd.Parameters.AddWithValue("@pNLinea", numLinea);
            cmd.Parameters.AddWithValue("@pCodProducto", codProducto);
            cmd.Parameters.AddWithValue("@pCantidad", cantidad);
            cmd.Parameters.AddWithValue("@pDescuento", descuento);
            if (probador != 0) cmd.Parameters.AddWithValue("@pProbador", probador);
            cmd.ExecuteNonQuery();
        }

        public void PersistirPedidoEstado(string idPedido, int estado, SqlTransaction transaction)
        {
            SqlConnection cnn = transaction.Connection;
            SqlCommand cmd = cnn.CreateCommand();
            cmd.Transaction = transaction;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "prc_upd_PedidosCabecera";
            cmd.Parameters.AddWithValue("@pIdPedido", idPedido);
            cmd.Parameters.AddWithValue("@pIdEstado", estado);
            cmd.ExecuteNonQuery();
        }

        public void PersistirPedidoTango(string CodigoOrden, string CodigoTango, SqlTransaction transaction)
        {
            SqlConnection cnn = transaction.Connection;
            SqlCommand cmd = cnn.CreateCommand();
            cmd.Transaction = transaction;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "prc_upd_PedidoCabeceraNroTango";
            cmd.Parameters.AddWithValue("@pCodigoOrden", CodigoOrden);
            cmd.Parameters.AddWithValue("@pCodigoTango", CodigoTango);
            cmd.ExecuteNonQuery();
        }

        public void PersistirPedidoAprobacion(int idPedido, bool finanzas, bool ventas, bool contaduria, SqlTransaction? trans = null)
        {
            SqlConnection cnn;
            SqlCommand cmd;
            if (trans == null)
            {
                cnn = new(ConnectionString);
                cmd = cnn.CreateCommand();
            }
            else
            {
                cnn = trans.Connection;
                cmd = cnn.CreateCommand();
                cmd.Transaction = trans;
            }
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "prc_upd_PedidosCabecera";
            cmd.Parameters.AddWithValue("@pIdPedido", idPedido);
            cmd.Parameters.AddWithValue("@pAprobadoFinanzas", finanzas);
            cmd.Parameters.AddWithValue("@pAprobadoVentas", ventas);
            cmd.Parameters.AddWithValue("@pAprobadoContaduria", contaduria);
            if (trans == null)
            {
                cnn.Open();
                cmd.ExecuteNonQuery();
                cnn.Close();
            }
            else
            {
                cmd.ExecuteNonQuery();
            }
        }

        public int ObtenerCodigoOrden()
        {
            int cod;
            using (SqlConnection cnn = new(ConnectionString))
            {
                SqlCommand cmd = cnn.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "prc_get_ProximoCodigoOrden";
                cnn.Open();
                cod = (int)cmd.ExecuteScalar();
                cnn.Close();
            }
            return cod;
        }

        public DataTable ObtenerOrdenExpediciones(string? idOrden = null, int? idEstado = null, SqlTransaction? trans = null)
        {
            DataTable dt = new DataTable();
            SqlConnection cnn;
            SqlCommand cmd;
            if (trans != null)
            {
                cnn = trans.Connection;
                cmd = cnn.CreateCommand();
                cmd.Transaction = trans;
            }
            else
            {
                cnn = new(ConnectionString);
                cmd = cnn.CreateCommand();
            }
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "prc_get_PedidosCabeceraExpedicion";
            if (idOrden != null) cmd.Parameters.AddWithValue("@pCodOrden", idOrden);
            if (idEstado != null) cmd.Parameters.AddWithValue("@pIdEstado", idEstado);
            SqlDataAdapter da = new(cmd);
            da.Fill(dt);

            return dt;
        }

        public DataTable ObtenerOrdenDetalleExpedicion(string idOrden)
        {
            DataTable dt = new DataTable();
            using (SqlConnection cnn = new(ConnectionString))
            {
                SqlCommand cmd = new()
                {
                    Connection = cnn,
                    CommandType = CommandType.StoredProcedure,
                    CommandText = "prc_get_PedidosDetalleExpedicion"
                };
                cmd.Parameters.AddWithValue("@pCodOrden", idOrden);
                SqlDataAdapter da = new(cmd);
                da.Fill(dt);
            }
            return dt;
        }

        public void UpdatePedidoDetalle(string codOrden, int nLinea, int cantidadAprobada, int? probadorAprobada, SqlTransaction transaction)
        {
            SqlConnection cnn = transaction.Connection;
            SqlCommand cmd = cnn.CreateCommand();
            cmd.Transaction = transaction;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "prc_upd_PedidosDetalle";
            cmd.Parameters.AddWithValue("@pCodigoOrden", codOrden);
            cmd.Parameters.AddWithValue("@pNLinea", nLinea);
            cmd.Parameters.AddWithValue("@pCantidadAprobada", cantidadAprobada);
            if (probadorAprobada != null) cmd.Parameters.AddWithValue("@pProbadorAprobada", probadorAprobada);
            cmd.ExecuteNonQuery();
        }
        public void PersistirPedidoImpresion(string idPedido)
        {
            using (SqlConnection cnn = new(ConnectionString))
            {
                SqlCommand cmd = cnn.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "prc_upd_PedidosCabecera";
                cmd.Parameters.AddWithValue("@pIdPedido", idPedido);
                cmd.Parameters.AddWithValue("@pImpreso", true);
                cnn.Open();
                cmd.ExecuteNonQuery();
                cnn.Close();
            }
        }

        public DataTable GetCantidadesProductos()
        {
            DataTable dt = new DataTable();
            using (SqlConnection cnn = new(ConnectionString))
            {
                SqlCommand cmd = new()
                {
                    Connection = cnn,
                    CommandType = CommandType.StoredProcedure,
                    CommandText = "prc_get_CantidadesDeProductos"
                };
                SqlDataAdapter da = new(cmd);
                da.Fill(dt);
            }
            return dt;
        }
    }
}
