using GAPPLE.Client.Pages;
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

        public DataTable ObtenerOrdenes(DateTime? desde, DateTime? hasta, int? idPedido, string? codOrden, bool? presupuesto, string? razonSocial,
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

        public DataTable ObtenerOrdenesConPendientes(DateTime? desde, DateTime? hasta, int idUsuario)
        {
            DataTable dt = new DataTable();
            using (SqlConnection cnn = new(ConnectionString))
            {
                SqlCommand cmd = new()
                {
                    Connection = cnn,
                    CommandType = CommandType.StoredProcedure,
                    CommandText = "prc_get_ordenesconpendientes"
                };
                cmd.Parameters.AddWithValue("@pDesde", desde);
                cmd.Parameters.AddWithValue("@pHasta", hasta);
                cmd.Parameters.AddWithValue("@pIdUsuario", idUsuario);
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

        public DataTable ObtenerOrdenConPendienteDetalle(string codOrden)
        {
            DataTable dt = new DataTable();
            using (SqlConnection cnn = new(ConnectionString))
            {
                SqlCommand cmd = new()
                {
                    Connection = cnn,
                    CommandType = CommandType.StoredProcedure,
                    CommandText = "prc_get_OrdenesConPendienteDetalle"
                };
                cmd.Parameters.AddWithValue("@pCodigoOrden", codOrden);
                SqlDataAdapter da = new(cmd);
                da.Fill(dt);
            }
            return dt;
        }

        public DataTable ObtenerOrdenesDashboard(int idUsuario)
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
                cmd.Parameters.AddWithValue("@pIdUsuario", idUsuario);
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
                                            string observaciones, DateTime? fechaEntrega, string ofertas, string altaUsuario, string? observacionesZentra, SqlTransaction transaction)
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
            if (!string.IsNullOrEmpty(ofertas)) cmd.Parameters.AddWithValue("@pOfertas", ofertas);
            if (!string.IsNullOrEmpty(observacionesZentra)) cmd.Parameters.AddWithValue("@pObservacionesZentra", observacionesZentra);
            id = (int)cmd.ExecuteScalar();
            return id;
        }

        public void UpdatePedidoCabecera(string codOrden, string linea, string codigoCliente, int? cantLineas, int idEstado, string zona, string listaPrecio,
                                            bool factura, bool presupuesto, string codTransporte, string condicionVenta, string entregarEn,
                                            string observaciones, DateTime? fechaEntrega, string edicionUsuario, string? observacionesZentra, string codOrdenCambiar, SqlTransaction transaction)
        {
            SqlConnection cnn = transaction.Connection;
            SqlCommand cmd = cnn.CreateCommand();
            cmd.Transaction = transaction;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "prc_upd_PedidoCabeceraCompleto";
            cmd.Parameters.AddWithValue("@pCodigoOrden", codOrden);
            cmd.Parameters.AddWithValue("@pLinea", linea);
            cmd.Parameters.AddWithValue("@pCodigoCliente", codigoCliente);
            if (cantLineas != null) cmd.Parameters.AddWithValue("@pCantidadLineas", cantLineas);
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
            cmd.Parameters.AddWithValue("@pEdicionUsuario", edicionUsuario);
            if (codOrdenCambiar != null) cmd.Parameters.AddWithValue("@pCodigoOrdenCambiar", codOrdenCambiar);
            if (!string.IsNullOrEmpty(observacionesZentra)) cmd.Parameters.AddWithValue("@pObservacionesZentra", observacionesZentra);
            cmd.ExecuteNonQuery();
        }

        public void UpdatePedidoCabecera(string codOrden, string linea, int? cantLineas, int idEstado, string listaPrecio, bool factura, bool presupuesto, string edicionUsuario)
        {
            using (SqlConnection cnn = new(ConnectionString))
            {
                SqlCommand cmd = cnn.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "prc_upd_PedidoCabeceraCompleto";
                cmd.Parameters.AddWithValue("@pCodigoOrden", codOrden);
                cmd.Parameters.AddWithValue("@pLinea", linea);
                if (cantLineas != null) cmd.Parameters.AddWithValue("@pCantidadLineas", cantLineas);
                cmd.Parameters.AddWithValue("@pIdEstado", idEstado);
                cmd.Parameters.AddWithValue("@pListaDePrecios", listaPrecio);
                cmd.Parameters.AddWithValue("@pFactura", factura);
                cmd.Parameters.AddWithValue("@pPresupuesto", presupuesto);
                cmd.Parameters.AddWithValue("@pEdicionUsuario", edicionUsuario);
                cnn.Open();
                cmd.ExecuteNonQuery();
                cnn.Close();
            }
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

        public void PersistirPedidoDetalle(string codOrden, int numLinea, string codProducto, int cantidad, int probador, int obsequio, decimal descuento, SqlTransaction transaction)
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
            if(obsequio != 0 ) cmd.Parameters.AddWithValue("@pObsequios", obsequio);
            cmd.ExecuteNonQuery();
        }

        public void PersistirPedidoEstado(string idPedido, int estado, string nombreUsuario, SqlTransaction transaction)
        {
            SqlConnection cnn = transaction.Connection;
            SqlCommand cmd = cnn.CreateCommand();
            cmd.Transaction = transaction;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "prc_upd_PedidosCabecera";
            cmd.Parameters.AddWithValue("@pIdPedido", idPedido);
            cmd.Parameters.AddWithValue("@pIdEstado", estado);
            cmd.Parameters.AddWithValue("@pEdicionUsuario", nombreUsuario);
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

        public void PersistirPedidoAprobacion(int idPedido, bool finanzas, bool ventas, bool contaduria, string usuario, SqlTransaction? trans = null)
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
            cmd.Parameters.AddWithValue("@pEdicionUsuario", usuario);
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

        public void UpdatePedidoDetalle(string codOrden, string codProducto, int cantidadAprobada, int? probadorAprobada, int? obsequioAprobada, SqlTransaction transaction)
        {
            SqlConnection cnn = transaction.Connection;
            SqlCommand cmd = cnn.CreateCommand();
            cmd.Transaction = transaction;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "prc_upd_PedidosDetalle";
            cmd.Parameters.AddWithValue("@pCodigoOrden", codOrden);
            cmd.Parameters.AddWithValue("@pCodProducto", codProducto);
            cmd.Parameters.AddWithValue("@pCantidadAprobada", cantidadAprobada);
            if (probadorAprobada != null) cmd.Parameters.AddWithValue("@pProbadorAprobada", probadorAprobada);
            if (obsequioAprobada != null) cmd.Parameters.AddWithValue("@pObsequioAprobada", obsequioAprobada);
            cmd.ExecuteNonQuery();
        }
        public void PersistirPedidoImpresion(string idPedido, string nombreUsuario)
        {
            using (SqlConnection cnn = new(ConnectionString))
            {
                SqlCommand cmd = cnn.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "prc_upd_PedidosCabecera";
                cmd.Parameters.AddWithValue("@pIdPedido", idPedido);
                cmd.Parameters.AddWithValue("@pImpreso", true);
                cmd.Parameters.AddWithValue("@pEdicionUsuario", nombreUsuario);
                cnn.Open();
                cmd.ExecuteNonQuery();
                cnn.Close();
            }
        }

        public DataTable GetCantidadesProductos(int idUsuario)
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
                cmd.Parameters.AddWithValue("@pIdUsuario", idUsuario);
                SqlDataAdapter da = new(cmd);
                da.Fill(dt);
            }
            return dt;
        }

        public void RevertirOrden(string idPedido, string nombreUsuario)
        {
            using (SqlConnection cnn = new(ConnectionString))
            {
                SqlCommand cmd = cnn.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "prc_upd_RevertirOrden";
                cmd.Parameters.AddWithValue("@pIdPedido", idPedido);
                cmd.Parameters.AddWithValue("@pEdicionUsuario", nombreUsuario);
                cnn.Open();
                cmd.ExecuteNonQuery();
                cnn.Close();
            }
        }

        public DataTable ObtenerIndicadores(int idUsuario)
        {
            DataTable dt = new DataTable();
            using (SqlConnection cnn = new(ConnectionString))
            {
                SqlCommand cmd = new()
                {
                    Connection = cnn,
                    CommandType = CommandType.StoredProcedure,
                    CommandText = "prc_get_EstadisticasMesActual"
                };
                cmd.Parameters.AddWithValue("@pIdUsuario", idUsuario);
                SqlDataAdapter da = new(cmd);
                da.Fill(dt);
            }
            return dt;
        }
    }
}
