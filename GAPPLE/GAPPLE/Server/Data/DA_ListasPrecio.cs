using System.Data;
using GAPPLE.Shared.Model;
using GAPPLE.Shared.Requests;
using Microsoft.Data.SqlClient;

namespace GAPPLE.Server.Data
{
    /// <summary>
    /// Listas de precios cargadas desde Excel y su asignacion por cliente y vendedor.
    /// </summary>
    public class DA_ListasPrecio
    {
        private string ConnectionString { get; }
        public DA_ListasPrecio(string connectionString) => ConnectionString = connectionString;

        #region Listas
        public DataTable ObtenerListas(int? idListaPrecio = null)
        {
            DataTable dt = new();
            using SqlConnection cnn = new(ConnectionString);
            SqlCommand cmd = cnn.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "prc_get_ListasPrecioImportadas";
            if (idListaPrecio != null) cmd.Parameters.AddWithValue("@pIdListaPrecio", idListaPrecio);
            new SqlDataAdapter(cmd).Fill(dt);
            return dt;
        }

        public DataTable ObtenerItems(int idListaPrecio)
        {
            DataTable dt = new();
            using SqlConnection cnn = new(ConnectionString);
            SqlCommand cmd = cnn.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "prc_get_ListasPrecioImportadasDetalle";
            cmd.Parameters.AddWithValue("@pIdListaPrecio", idListaPrecio);
            new SqlDataAdapter(cmd).Fill(dt);
            return dt;
        }

        /// <summary>Columnas: Nombre, NombreArchivo, Archivo.</summary>
        public DataTable ObtenerArchivo(int idListaPrecio)
        {
            DataTable dt = new();
            using SqlConnection cnn = new(ConnectionString);
            SqlCommand cmd = cnn.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "prc_get_ListaPrecioImportadaArchivo";
            cmd.Parameters.AddWithValue("@pIdListaPrecio", idListaPrecio);
            new SqlDataAdapter(cmd).Fill(dt);
            return dt;
        }

        /// <summary>
        /// Crea la lista (sin IdListaPrecio) o reemplaza todos sus precios y el archivo. Devuelve el id de la lista.
        /// Si ya existe otra lista con el mismo nombre el SP falla con un error 50000.
        /// </summary>
        public int Guardar(ListaPrecioArchivoRequest request, List<ListaPrecioImportadaItem> items)
        {
            DataTable detalle = new();
            detalle.Columns.Add("Orden", typeof(int));
            detalle.Columns.Add("Familia", typeof(string));
            detalle.Columns.Add("SKU", typeof(string));
            detalle.Columns.Add("Descripcion", typeof(string));
            detalle.Columns.Add("Tono", typeof(string));
            detalle.Columns.Add("EAN", typeof(string));
            detalle.Columns.Add("Contenido", typeof(string));
            detalle.Columns.Add("UnidadesPorCaja", typeof(int));
            detalle.Columns.Add("CostoNetoSinIva", typeof(decimal));
            detalle.Columns.Add("CostoNetoConIva", typeof(decimal));
            detalle.Columns.Add("PrecioSugeridoPublico", typeof(decimal));
            foreach (var i in items)
                detalle.Rows.Add(i.Orden, Valor(i.Familia), i.SKU, Valor(i.Descripcion), Valor(i.Tono), Valor(i.EAN), Valor(i.Contenido),
                                 Valor(i.UnidadesPorCaja), Valor(i.CostoNetoSinIva), Valor(i.CostoNetoConIva), Valor(i.PrecioSugeridoPublico));

            using SqlConnection cnn = new(ConnectionString);
            SqlCommand cmd = cnn.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "prc_ins_ListaPrecioImportada";
            cmd.Parameters.AddWithValue("@pIdListaPrecio", Valor(request.IdListaPrecio));
            cmd.Parameters.AddWithValue("@pNombre", request.Nombre.Trim());
            cmd.Parameters.Add("@pVigenciaDesde", SqlDbType.Date).Value = Valor(request.VigenciaDesde?.Date);
            cmd.Parameters.AddWithValue("@pNombreArchivo", Valor(request.NombreArchivo));
            cmd.Parameters.Add("@pArchivo", SqlDbType.VarBinary, -1).Value = Valor(request.Archivo);
            cmd.Parameters.AddWithValue("@pUsuario", Valor(request.Usuario));
            var pDetalle = cmd.Parameters.AddWithValue("@pDetalle", detalle);
            pDetalle.SqlDbType = SqlDbType.Structured;
            pDetalle.TypeName = "dbo.tt_ListaPrecioImportadaDetalle";
            cnn.Open();
            return Convert.ToInt32(cmd.ExecuteScalar());
        }

        /// <summary>Borra la lista con sus precios y sus asignaciones a clientes.</summary>
        public void Eliminar(int idListaPrecio)
        {
            using SqlConnection cnn = new(ConnectionString);
            SqlCommand cmd = cnn.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "prc_del_ListaPrecioImportada";
            cmd.Parameters.AddWithValue("@pIdListaPrecio", idListaPrecio);
            cnn.Open();
            cmd.ExecuteNonQuery();
        }
        #endregion

        #region Asignacion por cliente y vendedor
        /// <summary>
        /// Listas asignadas por cliente y vendedor. La columna VendedorAsignado indica si el usuario sigue asignado al cliente
        /// en VendedoresPorCliente (los administradores no figuran ahi).
        /// </summary>
        public DataTable ObtenerAsignaciones(int? idCliente, int? idUsuario)
        {
            DataTable dt = new();
            using SqlConnection cnn = new(ConnectionString);
            SqlCommand cmd = cnn.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "prc_get_ListasPrecioPorClienteVendedor";
            if (idCliente != null) cmd.Parameters.AddWithValue("@pIdCliente", idCliente);
            if (idUsuario != null) cmd.Parameters.AddWithValue("@pIdUsuario", idUsuario);
            new SqlDataAdapter(cmd).Fill(dt);
            return dt;
        }

        /// <summary>
        /// Reemplaza en una transaccion todas las listas asignadas al cliente por las del request.
        /// </summary>
        public void PersistirAsignaciones(ListasPrecioClienteRequest request)
        {
            using SqlConnection cnn = new(ConnectionString);
            cnn.Open();
            using SqlTransaction tran = cnn.BeginTransaction();
            try
            {
                SqlCommand del = cnn.CreateCommand();
                del.Transaction = tran;
                del.CommandType = CommandType.StoredProcedure;
                del.CommandText = "prc_del_ListasPrecioPorClienteVendedor";
                del.Parameters.AddWithValue("@pIdCliente", request.IdCliente);
                del.ExecuteNonQuery();

                foreach (var vendedor in request.Vendedores)
                {
                    foreach (int idLista in vendedor.IdListas.Distinct())
                    {
                        SqlCommand ins = cnn.CreateCommand();
                        ins.Transaction = tran;
                        ins.CommandType = CommandType.StoredProcedure;
                        ins.CommandText = "prc_ins_ListasPrecioPorClienteVendedor";
                        ins.Parameters.AddWithValue("@pIdCliente", request.IdCliente);
                        ins.Parameters.AddWithValue("@pIdUsuario", vendedor.IdUsuario);
                        ins.Parameters.AddWithValue("@pIdListaPrecio", idLista);
                        ins.Parameters.AddWithValue("@pAltaUsuario", Valor(request.Usuario));
                        ins.ExecuteNonQuery();
                    }
                }

                tran.Commit();
            }
            catch
            {
                tran.Rollback();
                throw;
            }
        }
        #endregion

        private static object Valor(object valor) => valor ?? DBNull.Value;
    }
}
