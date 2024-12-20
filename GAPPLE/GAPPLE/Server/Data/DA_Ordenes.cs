using GAPPLE.Shared.Model;
using Microsoft.Extensions.Hosting;
using Radzen.Blazor.Rendering;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Runtime.InteropServices;
using System.Transactions;

namespace GAPPLE.Server.Data
{
    public class DA_Ordenes
    {
        private string ConnectionString { get; }

        public DA_Ordenes(string connectionString) => ConnectionString = connectionString;

        public DataTable ObtenerOrdenes(DateTime desde, DateTime hasta, int? idPedido, string? codOrden, bool? presupuesto, string? razonSocial,
                                        string? linea, string? zona, int? idEstado, string? codTango)
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

        public DataTable ObtenerOrden(int idPedido)
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
                cmd.Parameters.AddWithValue("@pIdPedido", idPedido);
                SqlDataAdapter da = new(cmd);
                da.Fill(dt);
            }
            return dt;
        }

        public DataTable ObtenerOrdenDetalle(int idPedido)
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
                cmd.Parameters.AddWithValue("@pIdPedido", idPedido);
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
                                            bool probadores, bool OCCD, bool MtEX, string observaciones, DateTime fechaEntrega, string altaUsuario, SqlTransaction transaction)
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
            cmd.Parameters.AddWithValue("@pProbadores", probadores);
            cmd.Parameters.AddWithValue("@pOCCD", OCCD);
            cmd.Parameters.AddWithValue("@pMtEX", MtEX);
            cmd.Parameters.AddWithValue("@pObservaciones", observaciones);
            cmd.Parameters.AddWithValue("@pFechaEntrega", fechaEntrega);
            cmd.Parameters.AddWithValue("@pAltaUsuario", altaUsuario);
            id = (int)cmd.ExecuteScalar();
            return id;
        }

        public void PersistirPedidoDetalle(string codOrden, int numLinea, string codProducto, int cantidad, bool probador, decimal descuento, SqlTransaction transaction)
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
            cmd.Parameters.AddWithValue("@pProbador", probador);
            cmd.Parameters.AddWithValue("@pDescuento", descuento);
            cmd.ExecuteNonQuery();
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
    }
}
