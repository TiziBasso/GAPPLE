using System.Data;
using GAPPLE.Server.Data;
using GAPPLE.Server.Helpers;
using GAPPLE.Shared.Model;
using GAPPLE.Shared.Requests;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace GAPPLE.Server.Controllers
{
    /// <summary>
    /// Listas de precios cargadas desde Excel (ej. "Lista 174") y su asignacion por cliente y vendedor.
    /// Un mismo cliente puede tener listas distintas segun el vendedor. Un vendedor solo ve las listas de los clientes que tiene
    /// asignados en VendedoresPorCliente. Los administradores (perfil 1) son vendedores de todos los clientes sin figurar en esa
    /// tabla (igual que en /clientes/vendedores), y ven todas las listas.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class ListasPrecioController : ControllerBase
    {
        private const int PERFIL_ADMINISTRADOR = 1;
        private const int MAX_BYTES_ARCHIVO = 10 * 1024 * 1024;
        private const string TIPO_XLSX = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        private string DefaultConnectionString { get; }

        public ListasPrecioController(IConfiguration configuration)
        {
            DefaultConnectionString = configuration.GetConnectionString("DefaultConnection");
        }

        #region Listas
        [HttpGet]
        public IActionResult GetListas()
        {
            try
            {
                DA_ListasPrecio da = new(DefaultConnectionString);
                List<ListaPrecioImportada> listas = new();
                using DataTable dt = da.ObtenerListas();
                foreach (DataRow row in dt.Rows)
                {
                    listas.Add(new ListaPrecioImportada
                    {
                        IdListaPrecio = Convert.ToInt32(row["IdListaPrecio"]),
                        Nombre = row["Nombre"].ToString(),
                        VigenciaDesde = row["VigenciaDesde"] == DBNull.Value ? null : Convert.ToDateTime(row["VigenciaDesde"]),
                        NombreArchivo = row["NombreArchivo"].ToString(),
                        CantidadArticulos = Convert.ToInt32(row["CantidadArticulos"]),
                        CantidadClientes = Convert.ToInt32(row["CantidadClientes"]),
                        UsuarioActualizacion = row["UsuarioActualizacion"].ToString(),
                        FechaActualizacion = row["FechaActualizacion"] == DBNull.Value ? null : Convert.ToDateTime(row["FechaActualizacion"])
                    });
                }
                return Ok(listas);
            }
            catch (Exception ex) { return StatusCode(500, ex.Message); }
        }

        /// <summary>Lee el Excel y devuelve lo que se guardaria, sin guardar nada.</summary>
        [HttpPost("previsualizar")]
        public IActionResult Previsualizar(ListaPrecioArchivoRequest request)
        {
            try
            {
                var lista = LeerArchivo(request);
                return lista == null ? BadRequest(ModelState) : Ok(lista);
            }
            catch (Exception ex) { return StatusCode(500, ex.Message); }
        }

        /// <summary>
        /// Crea una lista nueva con los precios del Excel, o si viene IdListaPrecio reemplaza los precios y el archivo de esa lista
        /// (sus asignaciones a clientes se mantienen). El Excel se vuelve a leer aca: no se confia en lo que mande el navegador.
        /// </summary>
        [HttpPost]
        public IActionResult PostLista(ListaPrecioArchivoRequest request)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(request?.Nombre))
                    ModelState.AddModelError("Nombre", "Debe indicar el nombre de la lista");
                else if (request.Nombre.Trim().Length > 150)
                    ModelState.AddModelError("Nombre", "El nombre no puede superar los 150 caracteres");

                var lista = LeerArchivo(request);
                if (lista == null || ModelState.ErrorCount > 0)
                    return BadRequest(ModelState);

                DA_ListasPrecio da = new(DefaultConnectionString);
                int id = da.Guardar(request, lista.Items);
                return Ok(id);
            }
            catch (SqlException ex) when (ex.Number == 50000)
            {
                // Validaciones del SP (nombre repetido, lista inexistente)
                ModelState.AddModelError("Lista", ex.Message);
                return BadRequest(ModelState);
            }
            catch (Exception ex) { return StatusCode(500, ex.Message); }
        }

        [HttpDelete("{idListaPrecio:int}")]
        public IActionResult DeleteLista(int idListaPrecio)
        {
            try
            {
                DA_ListasPrecio da = new(DefaultConnectionString);
                da.Eliminar(idListaPrecio);
                return Ok();
            }
            catch (Exception ex) { return StatusCode(500, ex.Message); }
        }

        [HttpGet("{idListaPrecio:int}/items")]
        public IActionResult GetItems(int idListaPrecio, int idUsuario)
        {
            try
            {
                if (!PuedeVerLista(idUsuario, idListaPrecio))
                    return StatusCode(StatusCodes.Status403Forbidden);

                DA_ListasPrecio da = new(DefaultConnectionString);
                List<ListaPrecioImportadaItem> items = new();
                using DataTable dt = da.ObtenerItems(idListaPrecio);
                foreach (DataRow row in dt.Rows)
                {
                    items.Add(new ListaPrecioImportadaItem
                    {
                        Orden = Convert.ToInt32(row["Orden"]),
                        Familia = Texto(row["Familia"]),
                        SKU = Texto(row["SKU"]),
                        Descripcion = Texto(row["Descripcion"]),
                        Tono = Texto(row["Tono"]),
                        EAN = Texto(row["EAN"]),
                        Contenido = Texto(row["Contenido"]),
                        UnidadesPorCaja = row["UnidadesPorCaja"] == DBNull.Value ? null : Convert.ToInt32(row["UnidadesPorCaja"]),
                        CostoNetoSinIva = Decimal(row["CostoNetoSinIva"]),
                        CostoNetoConIva = Decimal(row["CostoNetoConIva"]),
                        PrecioSugeridoPublico = Decimal(row["PrecioSugeridoPublico"])
                    });
                }
                return Ok(items);
            }
            catch (Exception ex) { return StatusCode(500, ex.Message); }
        }

        /// <summary>Descarga el Excel tal cual se subio.</summary>
        [HttpGet("{idListaPrecio:int}/archivo")]
        public IActionResult GetArchivo(int idListaPrecio, int idUsuario)
        {
            try
            {
                if (!PuedeVerLista(idUsuario, idListaPrecio))
                    return StatusCode(StatusCodes.Status403Forbidden);

                DA_ListasPrecio da = new(DefaultConnectionString);
                using DataTable dt = da.ObtenerArchivo(idListaPrecio);
                if (dt.Rows.Count == 0 || dt.Rows[0]["Archivo"] == DBNull.Value)
                    return NotFound();

                var row = dt.Rows[0];
                string nombreArchivo = Texto(row["NombreArchivo"]) ?? $"{row["Nombre"]}.xlsx";
                return File((byte[])row["Archivo"], TIPO_XLSX, nombreArchivo);
            }
            catch (Exception ex) { return StatusCode(500, ex.Message); }
        }
        #endregion

        #region Asignacion por cliente y vendedor
        /// <summary>Todas las listas asignadas a un cliente, de todos sus vendedores (pantalla de carga).</summary>
        [HttpGet("cliente")]
        public IActionResult GetPorCliente(int idCliente)
        {
            try
            {
                DA_ListasPrecio da = new(DefaultConnectionString);
                using DataTable dt = da.ObtenerAsignaciones(idCliente, null);
                return Ok(MapearAsignaciones(dt.Rows.Cast<DataRow>(), conNombreVendedor: true));
            }
            catch (Exception ex) { return StatusCode(500, ex.Message); }
        }

        [HttpPost("cliente")]
        public IActionResult PostPorCliente(ListasPrecioClienteRequest request)
        {
            try
            {
                if (request == null || request.IdCliente <= 0)
                    return BadRequest("Debe indicar el cliente");

                DA_ListasPrecio da = new(DefaultConnectionString);
                da.PersistirAsignaciones(request);
                return Ok();
            }
            catch (Exception ex) { return StatusCode(500, ex.Message); }
        }

        /// <summary>
        /// Listas que puede ver el usuario: las de sus clientes asignados. El administrador ve las de todos los vendedores y,
        /// ademas, las listas que todavia no estan asignadas a ningun cliente (con IdCliente = 0).
        /// </summary>
        [HttpGet("vendedor")]
        public IActionResult GetPorVendedor(int idUsuario)
        {
            try
            {
                var administradores = IdsAdministradores();
                bool admin = administradores.Contains(idUsuario);

                DA_ListasPrecio da = new(DefaultConnectionString);
                using DataTable dt = da.ObtenerAsignaciones(null, admin ? null : idUsuario);
                var asignaciones = MapearAsignaciones(Vigentes(dt, administradores), conNombreVendedor: admin);

                if (admin)
                {
                    var conClientes = asignaciones.Select(x => x.IdListaPrecio).ToHashSet();
                    using DataTable listas = da.ObtenerListas();
                    foreach (DataRow row in listas.Rows)
                    {
                        int idLista = Convert.ToInt32(row["IdListaPrecio"]);
                        if (conClientes.Contains(idLista))
                            continue;

                        asignaciones.Add(new ListaPrecioClienteVendedor
                        {
                            IdListaPrecio = idLista,
                            NombreLista = row["Nombre"].ToString(),
                            VigenciaDesde = row["VigenciaDesde"] == DBNull.Value ? null : Convert.ToDateTime(row["VigenciaDesde"])
                        });
                    }
                }

                return Ok(asignaciones);
            }
            catch (Exception ex) { return StatusCode(500, ex.Message); }
        }
        #endregion

        /// <summary>Valida el archivo y lo lee. Si no sirve devuelve null y deja los motivos en ModelState.</summary>
        private ListaPrecioPrevisualizacion LeerArchivo(ListaPrecioArchivoRequest request)
        {
            if (request?.Archivo == null || request.Archivo.Length == 0)
            {
                ModelState.AddModelError("Archivo", "Debe seleccionar un archivo");
                return null;
            }
            if (request.Archivo.Length > MAX_BYTES_ARCHIVO)
            {
                ModelState.AddModelError("Archivo", "El archivo no puede superar los 10 MB");
                return null;
            }

            List<string> errores = new();
            var lista = ListaPrecioExcelParser.Leer(request.Archivo, request.NombreArchivo, errores);
            foreach (var error in errores)
                ModelState.AddModelError("Archivo", error);
            return lista;
        }

        /// <summary>
        /// El administrador puede ver cualquier lista. Un vendedor, solo las que tiene asignadas para algun cliente
        /// del que siga siendo vendedor.
        /// </summary>
        private bool PuedeVerLista(int idUsuario, int idListaPrecio)
        {
            var administradores = IdsAdministradores();
            if (administradores.Contains(idUsuario))
                return true;

            DA_ListasPrecio da = new(DefaultConnectionString);
            using DataTable dt = da.ObtenerAsignaciones(null, idUsuario);
            return Vigentes(dt, administradores).Any(r => Convert.ToInt32(r["IdListaPrecio"]) == idListaPrecio);
        }

        /// <summary>Usuarios con perfil administrador: son vendedores de todos los clientes.</summary>
        private HashSet<int> IdsAdministradores()
        {
            DA_Seguridad daS = new(DefaultConnectionString);
            using DataTable dt = daS.GetUsuarios(null, null, null, PERFIL_ADMINISTRADOR, null);
            return dt.Rows.Cast<DataRow>()
                .Where(r => Convert.ToInt32(r["IdPerfil"]) == PERFIL_ADMINISTRADOR)
                .Select(r => Convert.ToInt32(r["IdUsuario"]))
                .ToHashSet();
        }

        /// <summary>
        /// Descarta las asignaciones de vendedores que ya no estan asignados al cliente. Las de administradores siempre valen.
        /// </summary>
        private static IEnumerable<DataRow> Vigentes(DataTable dt, HashSet<int> administradores) =>
            dt.Rows.Cast<DataRow>().Where(r => Convert.ToBoolean(r["VendedorAsignado"])
                                               || administradores.Contains(Convert.ToInt32(r["IdUsuario"])));

        private List<ListaPrecioClienteVendedor> MapearAsignaciones(IEnumerable<DataRow> rows, bool conNombreVendedor)
        {
            var filas = rows.ToList();
            var vendedores = new Dictionary<int, string>();
            if (conNombreVendedor && filas.Count > 0)
            {
                DA_Seguridad daS = new(DefaultConnectionString);
                foreach (DataRow row in daS.GetUsuarios(null, null, null, null, null).Rows)
                    vendedores[Convert.ToInt32(row["IdUsuario"])] = row["ApellidoYNombre"].ToString();
            }

            List<ListaPrecioClienteVendedor> lista = new();
            foreach (DataRow row in filas)
            {
                int idUsuario = Convert.ToInt32(row["IdUsuario"]);
                lista.Add(new ListaPrecioClienteVendedor
                {
                    IdCliente = Convert.ToInt32(row["IdCliente"]),
                    CodigoCliente = row["CodigoCliente"].ToString(),
                    RazonSocial = row["RazonSocial"].ToString(),
                    IdUsuario = idUsuario,
                    ApellidoYNombre = vendedores.GetValueOrDefault(idUsuario),
                    IdListaPrecio = Convert.ToInt32(row["IdListaPrecio"]),
                    NombreLista = row["NombreLista"].ToString(),
                    VigenciaDesde = row["VigenciaDesde"] == DBNull.Value ? null : Convert.ToDateTime(row["VigenciaDesde"])
                });
            }
            return lista;
        }

        private static string Texto(object valor) => valor == DBNull.Value ? null : valor.ToString();
        private static decimal? Decimal(object valor) => valor == DBNull.Value ? null : Convert.ToDecimal(valor);
    }
}
