using GAPPLE.Client.Entities;
using GAPPLE.Client.Pages;
using GAPPLE.Server.Data;
using GAPPLE.Shared.Model;
using Integra.Web.Shared.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RestSharp;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Globalization;
using System.Net;
using System.Text.Json;

namespace GAPPLE.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdenesController : ControllerBase
    {
        private IConfiguration Configuration { get; }
        private Usuario Usuario { get; }

        public OrdenesController(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        [HttpGet("lista")]
        public List<Orden> GetOrdenes(string? desdeStr, string? hastaStr, int? idPedido, string? codOrden, bool? presupuesto, string? razonSocial,
                                        string? linea, string? zona, int? idEstado, string? codTango, int idUsuario)
        {
            DateTime? desde = null, hasta = null;
            if (desdeStr != null && hastaStr != null)
            {
                desde = DateTime.Parse(WebUtility.UrlDecode(desdeStr));
                hasta = DateTime.Parse(WebUtility.UrlDecode(hastaStr));
            }
            DA_Ordenes daO = new(Configuration.GetConnectionString("DefaultConnection"));
            List<Orden> lstOrdenes = new();
            using (DataTable dt = daO.ObtenerOrdenes(desde, hasta, idPedido, codOrden, presupuesto, razonSocial, linea, zona, idEstado, codTango, idUsuario))
            {
                foreach (DataRow row in dt.Rows)
                {
                    Orden o = new()
                    {
                        Id = (int)row["IdPedido"],
                        CodigoOrden = row["CodigoOrden"].ToString()!,
                        Presupuesto = (bool)row["Presupuesto"],
                        Cliente = row["RazonSocial"].ToString(),
                        Linea = row["Linea"].ToString(),
                        Creacion = (DateTime)row["AltaRegistro"],
                        Zona = row["DescripcionZona"].ToString(),
                        IdEstado = int.Parse(row["IdEstado"].ToString()!),
                        DescripcionEstado = row["DescripcionEstado"].ToString(),
                        NumeroFactura = row["NumFactura"].ToString(),
                        Unidades = (int)row["CantidadLineas"],
                        CodListaPrecio = row["IdListaDePrecio"].ToString()!,
                        Transporte = row["DescripcionTransporte"].ToString(),
                        CodTransporte = row["CodigoTransporte"].ToString(),
                        AprobadoContaduria = bool.Parse(row["AprobadoContaduria"].ToString()),
                        AprobadoVentas = bool.Parse(row["AprobadoVentas"].ToString()),
                        AprobadoFinanzas = bool.Parse(row["AprobadoContaduria"].ToString()),
                    };
                    if (row["NroPedidoTango"] != DBNull.Value) o.NROTANGO = row["NroPedidoTango"].ToString();
                    if (row["Observaciones"] != DBNull.Value) o.Notas = row["Observaciones"].ToString();

                    lstOrdenes.Add(o);
                }
            }
            return lstOrdenes;
        }

        [HttpGet]
        public Orden? GetOrden(string? codOrden, bool conDetalle, int? idPedido, SqlTransaction? trans)
        {
            DA_Ordenes daO = new(Configuration.GetConnectionString("DefaultConnection"));
            Orden? orden = null;
            using (DataTable dt = daO.ObtenerOrden(codOrden, idPedido, trans))
            {
                if (dt.Rows.Count > 0)
                {
                    var row = dt.Rows[0];
                    orden = new()
                    {
                        Id = (int)row["IdPedido"],
                        CodigoOrden = row["CodigoOrden"].ToString()!,
                        Presupuesto = (bool)row["Presupuesto"],
                        Cliente = row["RazonSocial"].ToString(),
                        CodListaPrecio = row["IdListaDePrecio"].ToString()!,
                        CodCliente = row["CodigoCliente"].ToString(),
                        CUITCliente = row["CUIT"].ToString(),
                        DomicilioCliente = row["DomicilioCliente"].ToString(),
                        CondicionVenta = row["CondicionVenta"].ToString(),
                        Entrega = row["EntregarEn"].ToString(),
                        Notas = row["Observaciones"].ToString(),
                        Linea = row["Linea"].ToString(),
                        Creacion = DateTime.Parse(row["AltaRegistro"].ToString()!),
                        Zona = row["DescripcionZona"].ToString(),
                        IdEstado = (int)row["IdEstado"],
                        DescripcionEstado = row["DescripcionEstado"].ToString(),
                        NROTANGO = row["NroPedidoTango"].ToString(),
                        NumeroFactura = row["NumFactura"].ToString()
                    };
                    if (row["Ofertas"] != DBNull.Value)
                    {
                        orden.Ofertas = row["Ofertas"].ToString()
                            .Split(',')
                            .Select(x => int.TryParse(x.Trim(), out int num) ? num : 0) // Valores inválidos serán 0
                            .ToList();
                    }
                    if (row["GVA_CONDVENTA"] != DBNull.Value) orden.ID_GVA01 = int.Parse(row["GVA_CONDVENTA"].ToString());
                    if (row["GVA_LISTAPRECIO"] != DBNull.Value) orden.ID_GVA10 = int.Parse(row["GVA_LISTAPRECIO"].ToString());
                    if (row["GVA_CLIENTE"] != DBNull.Value) orden.ID_GVA14 = int.Parse(row["GVA_CLIENTE"].ToString());
                    if (row["GVA_VENDEDOR"] != DBNull.Value) orden.ID_GVA23 = int.Parse(row["GVA_VENDEDOR"].ToString());
                    if (row["GVA_TRANSPORTE"] != DBNull.Value) orden.ID_GVA24 = int.Parse(row["GVA_TRANSPORTE"].ToString());
                    else orden.ID_GVA24 = 8;
                    if (row["CodTransporte"] != DBNull.Value) orden.CodTransporte = row["CodTransporte"].ToString();
                    if (row["DescripcionTransporte"] != DBNull.Value) orden.Transporte = row["DescripcionTransporte"].ToString();
                }

                using (DataTable dt2 = daO.ObtenerOrden(codOrden.StartsWith("X-") ? "F" + codOrden.Substring(1) : "X" + codOrden.Substring(1), idPedido, trans))
                {
                    if (dt2.Rows.Count > 0)
                    {
                        var row = dt2.Rows[0];
                        if (row["IdEstado"].ToString() == "1")
                            orden.TieneOrdenDoble = dt2.Rows.Count > 0;
                    }
                }
            }

            if (orden != null && conDetalle)
            {
                using (DataTable dt = daO.ObtenerOrdenDetalle(orden.CodigoOrden))
                {
                    if (dt.Rows.Count > 0) //siempre deberia tener pero por las dudas
                    {
                        orden.Detalle = new();
                        foreach (DataRow dr in dt.Rows)
                        {
                            OrdenDetalle detalle = new()
                            {
                                Id = orden.Id,
                                NumeroLinea = (int)dr["NLinea"],
                                Descuento = (decimal)dr["descuento"],
                                IdProducto = (int)dr["IdProducto"],
                                CodProducto = dr["CodProducto"].ToString(),
                                Descripcion = dr["Descripcion"].ToString(),
                                Cantidad = (int)dr["Cantidad"],
                                CantidadAprobada = (int)dr["CantidadAprobada"],
                                CantidadCancelada = (int)dr["CantidadCancelada"]
                            };
                            if (dr["CantidadProbador"] != DBNull.Value) detalle.CantidadProbador = int.Parse(dr["CantidadProbador"].ToString());
                            if (detalle.CantidadProbador > 0) detalle.Probador = true;
                            if (dr["CantidadProbadorAprobada"] != DBNull.Value) detalle.CantidadProbadorAprobada = int.Parse(dr["CantidadProbadorAprobada"].ToString());
                            if (dr["CantidadProbadorCancelada"] != DBNull.Value) detalle.CantidadProbadorCancelada = int.Parse(dr["CantidadProbadorCancelada"].ToString());
                            if (dr["ID_STA"] != DBNull.Value) detalle.ID_STA11 = int.Parse(dr["ID_STA"].ToString());
                            orden.Detalle.Add(detalle);
                        }
                    }
                }
            }

            return orden;
        }

        [HttpGet("transportes")]
        public List<Transporte> GetTransportes()
        {
            DA_Ordenes daO = new(Configuration.GetConnectionString("DefaultConnection"));
            List<Transporte> transportes = new List<Transporte>();
            using (DataTable dt = daO.ObtenerTransportes())
            {
                foreach (DataRow row in dt.Rows)
                {
                    Transporte transporte = new Transporte();

                    transporte.CodigoTango = row["CodigoTango"].ToString()!;
                    transporte.Descripcion = row["Descripcion"].ToString()!;
                    transporte.CUIT = row["CUIT"].ToString()!;
                    transportes.Add(transporte);
                }
                return transportes;
            }
        }

        [HttpGet("estados")]
        public List<Opcion> GetEstados()
        {
            List<Opcion> estados = new() { new((int?)null, "(Todos)") };
            DA_Ordenes daO = new(Configuration.GetConnectionString("DefaultConnection"));

            using (DataTable dt = daO.ObtenerEstados("Pedidos"))
            {
                foreach (DataRow row in dt.Rows)
                {
                    Opcion estado = new Opcion()
                    {
                        Id = int.Parse(row["IdEstado"].ToString()!),
                        Descripcion = row["Descripcion"].ToString()!
                    };

                    estados.Add(estado);
                }
            }

            return estados;
        }

        [HttpGet("ordenDashboard")]
        public List<OrdenDashboard> GetOrdenesDashboard(int idUsuario)
        {
            DA_Ordenes daO = new(Configuration.GetConnectionString("DefaultConnection"));
            List<OrdenDashboard> oDashs = new List<OrdenDashboard>();
            using (DataTable dt = daO.ObtenerOrdenesDashboard(idUsuario))
            {
                foreach (DataRow row in dt.Rows)
                {
                    OrdenDashboard oDash = new OrdenDashboard();
                    oDash.CodigoOrden = row["CodigoOrden"].ToString()!;
                    if (row["AltaRegistro"] != DBNull.Value) oDash.AltaRegistro = DateTime.Parse(row["AltaRegistro"].ToString()!);
                    if (row["FechaAprobacion"] != DBNull.Value) oDash.FechaAprobacion = DateTime.Parse(row["FechaAprobacion"].ToString()!);
                    oDashs.Add(oDash);
                }
                return oDashs;
            }
        }

        [HttpGet("condicionesdeventa")]
        public List<CondicionDeVenta> GetCondicionesDeVenta()
        {
            DA_Ordenes daO = new(Configuration.GetConnectionString("DefaultConnection"));
            List<CondicionDeVenta> Condiciones = new List<CondicionDeVenta>();
            using (DataTable dt = daO.ObtenerCondicionesDeVenta())
            {
                foreach (DataRow row in dt.Rows)
                {
                    CondicionDeVenta condicion = new CondicionDeVenta();
                    condicion.CodigoTango = row["CodigoTango"].ToString()!;
                    condicion.Descripcion = row["Descripcion"].ToString()!;
                    Condiciones.Add(condicion);
                }
                return Condiciones;
            }
        }

        [HttpGet("listasdeprecio")]
        public List<ListaDePrecios> GetListasDePrecio()
        {
            DA_Ordenes daO = new(Configuration.GetConnectionString("DefaultConnection"));
            List<ListaDePrecios> listas = new List<ListaDePrecios>();
            using (DataTable dt = daO.ObtenerListasDePrecio())
            {
                foreach (DataRow row in dt.Rows)
                {
                    ListaDePrecios lista = new ListaDePrecios();
                    lista.CodigoTango = row["CodLista"].ToString()!;
                    lista.Descripcion = row["Descripcion"].ToString()!;
                    listas.Add(lista);
                }
                return listas;
            }
        }

        [HttpGet("zonas")]
        public List<Zonas> GetZonas()
        {
            DA_Ordenes daO = new(Configuration.GetConnectionString("DefaultConnection"));
            List<Zonas> zonas = new List<Zonas>();
            using (DataTable dt = daO.ObtenerZonas())
            {
                foreach (DataRow row in dt.Rows)
                {
                    Zonas zona = new Zonas();
                    zona.CodigoTango = row["CodigoTango"].ToString()!;
                    zona.Descripcion = row["Descripcion"].ToString()!;
                    zonas.Add(zona);
                }
                return zonas;
            }
        }

        [HttpPost]
        public IActionResult PostPedido(Orden pedido)
        {
            SqlTransaction? trans = null;
            try
            {
                DA_Ordenes daO = new(Configuration.GetConnectionString("DefaultConnection"));
                using (SqlConnection cnn = new(Configuration.GetConnectionString("DefaultConnection")))
                {
                    cnn.Open();
                    trans = cnn.BeginTransaction();
                    pedido.CodigoOrden = daO.ObtenerCodigoOrden().ToString().PadLeft(8, '0');

                    if (pedido.Factura)
                    {
                        daO.PersistirPedidoCabecera("F-" + pedido.CodigoOrden, pedido.Linea!, pedido.CodCliente!, pedido.Detalle!.Sum(x => x.Cantidad), (int)pedido.IdEstado!,
                                                            pedido.Zona!, pedido.CodListaPrecio, pedido.Factura, false,
                                                            pedido.CodTransporte! == null ? "01" : pedido.CodTransporte, pedido.CondicionVenta!, pedido.Entrega!,
                                                            pedido.Notas!, pedido.FechaEntrega != null ? pedido.FechaEntrega!.Value : null, string.Join(",", pedido.Ofertas), pedido.Usuario, trans);
                        int numLinea = 0;
                        foreach (var item in pedido.Detalle!)
                        {
                            numLinea++;
                            daO.PersistirPedidoDetalle("F-" + pedido.CodigoOrden, numLinea, item.CodProducto!, item.Cantidad, item.CantidadProbador, item.Descuento, trans);
                            item.CantidadProbador = 0;
                        }
                    }

                    if (pedido.Presupuesto)
                    {
                        daO.PersistirPedidoCabecera("X-" + pedido.CodigoOrden, pedido.Linea!, pedido.CodCliente!, pedido.Detalle!.Sum(x => x.Cantidad), (int)pedido.IdEstado!,
                                                                pedido.Zona!, pedido.CodListaPrecio, false, pedido.Presupuesto,
                                                                pedido.CodTransporte! == null ? "01" : pedido.CodTransporte, pedido.CondicionVenta!, pedido.Entrega!,
                                                                pedido.Notas!, pedido.FechaEntrega != null ? pedido.FechaEntrega!.Value : null, string.Join(",", pedido.Ofertas), pedido.Usuario, trans);
                        int numLinea = 0;
                        foreach (var item in pedido.Detalle!)
                        {
                            numLinea++;
                            daO.PersistirPedidoDetalle("X-" + pedido.CodigoOrden, numLinea, item.CodProducto!, item.Cantidad, item.CantidadProbador, item.Descuento, trans);
                        }
                    }

                    trans.Commit();
                    cnn.Close();
                }

                return Ok(pedido);
            }
            catch (Exception ex)
            {
                if (trans != null && trans.Connection != null)
                    trans.Rollback();
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPut]
        public IActionResult PutPedido(Orden pedido)
        {
            SqlTransaction? trans = null;
            try
            {
                DA_Ordenes daO = new(Configuration.GetConnectionString("DefaultConnection"));
                using (SqlConnection cnn = new(Configuration.GetConnectionString("DefaultConnection")))
                {
                    cnn.Open();
                    trans = cnn.BeginTransaction();

                    if (pedido.Factura)
                    {
                        daO.UpdatePedidoCabecera(pedido.CodigoOrden, pedido.Linea!, pedido.CodCliente!, pedido.Detalle!.Sum(x => x.Cantidad), (int)pedido.IdEstado!,
                                                            pedido.Zona!, pedido.CodListaPrecio, pedido.Factura, false,
                                                            pedido.CodTransporte!, pedido.CondicionVenta!, pedido.Entrega!,
                                                            pedido.Notas!, pedido.FechaEntrega != null ? pedido.FechaEntrega!.Value : null, pedido.Usuario, trans);
                    }
                    else
                    {
                        daO.UpdatePedidoCabecera(pedido.CodigoOrden, pedido.Linea!, pedido.CodCliente!, pedido.Detalle!.Sum(x => x.Cantidad), (int)pedido.IdEstado!,
                                                            pedido.Zona!, pedido.CodListaPrecio, false, pedido.Presupuesto,
                                                            pedido.CodTransporte!, pedido.CondicionVenta!, pedido.Entrega!,
                                                            pedido.Notas!, pedido.FechaEntrega != null ? pedido.FechaEntrega!.Value : null, pedido.Usuario, trans);
                    }

                    daO.EliminarPedidoDetalle(pedido.CodigoOrden, trans);
                    int numLinea = 0;
                    foreach (var item in pedido.Detalle!)
                    {
                        numLinea++;
                        daO.PersistirPedidoDetalle(pedido.CodigoOrden, numLinea, item.CodProducto!, item.Cantidad, item.CantidadProbador, item.Descuento, trans);
                        item.CantidadProbador = 0;
                    }

                    trans.Commit();
                    cnn.Close();
                }

                return Ok(pedido);
            }
            catch (Exception ex)
            {
                if (trans != null && trans.Connection != null)
                    trans.Rollback();
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPut("aprobacion/{idUsuario:int}")]
        public IActionResult PutPedidoAprobacion(int idUsuario, [FromBody] Orden pedido)
        {
            SqlTransaction? trans = null;
            try
            {
                SqlConnection cnn = new(Configuration.GetConnectionString("DefaultConnection"));
                DA_Ordenes daO = new(cnn.ConnectionString);
                var pedidos = GetOrdenes(null, null, null, "%" + pedido.CodigoOrden.Substring(2) + "%", null, null, null, null, null, null, idUsuario).AsEnumerable();
                var idPedidos = pedidos.Where(x => x.IdEstado == 1).Select(x => x.Id);

                cnn.Open();
                trans = cnn.BeginTransaction();
                foreach (var id in idPedidos)
                {
                    daO.PersistirPedidoAprobacion(id, pedido.AprobadoFinanzas, pedido.AprobadoVentas, pedido.AprobadoContaduria, trans);

                    if (pedido.AprobadoContaduria && pedido.AprobadoFinanzas && pedido.AprobadoVentas)
                    {
                        pedido.IdEstado = 3;
                        pedido.DescripcionEstado = "APROBADO";
                        daO.PersistirPedidoEstado(id.ToString(), (int)pedido.IdEstado, trans);
                    }
                }

                trans.Commit();
                cnn.Close();
                return Ok(pedido);
            }
            catch (Exception ex)
            {
                if (trans != null && trans.Connection != null)
                    trans.Rollback();
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPut("tango")]
        public IActionResult PasarATango(Orden pedido)
        {
            SqlTransaction? trans = null;
            try
            {
                SqlConnection cnn = new(Configuration.GetConnectionString("DefaultConnection"));
                DA_Ordenes daO = new(cnn.ConnectionString);
                cnn.Open();
                trans = cnn.BeginTransaction();

                //var orden = GetOrdenExpedicion(pedido.CodigoOrden, 4, trans);
                var response = PostTango(pedido, trans);
                if (!response.IsSuccessStatusCode)
                {
                    if (response.Message != null)
                        return BadRequest(response.Message);
                    else
                        throw new Exception();
                }
                else
                {
                    pedido.IdEstado = 5;
                    pedido.DescripcionEstado = "EN TANGO";
                    daO.PersistirPedidoEstado(pedido.Id.ToString(), (int)pedido.IdEstado, trans);
                    daO.PersistirPedidoTango(pedido.CodigoOrden, response.Message, trans);
                }

                if (ModelState.ErrorCount > 0)
                {
                    trans.Rollback();
                    cnn.Close();
                    return BadRequest(ModelState);
                }
                else
                {
                    trans.Commit();
                    cnn.Close();
                    return Ok(pedido);
                }
            }
            catch (Exception ex)
            {
                if (trans != null && trans.Connection != null)
                    trans.Rollback();
                return StatusCode(500, ex.Message);
            }
        }

        private Response PostTango(Orden orden, SqlTransaction trans)
        {
            var options = new RestClientOptions("http://192.168.10.10:17000/Api")
            {
                ThrowOnAnyError = true,
                MaxTimeout = 300000
            };
            RestClient restClient = new RestClient(options);
      
            restClient.AddDefaultHeader("ApiAuthorization", "35639960-b67a-41f0-bb7b-b38b1355ff0d");
            restClient.AddDefaultHeader("Company", "7");

            RestRequest request = new RestRequest("Create?process=19845", Method.Post);

            PedidoDTO pedido = new PedidoDTO();

            Orden ordenFull = GetOrden(orden.CodigoOrden, true, null, trans)!;

            if (!ordenFull.Detalle.Any())
                return new(false, "La orden debe poseer al menos 1 producto");

            pedido.NRO_ORDEN_COMPRA = ordenFull.Id.ToString();
            pedido.FECHA_ORDEN_COMPRA = orden.Creacion.Value.AddDays(-1);
            pedido.ID_GVA43_TALON_PED = ordenFull.Presupuesto ? 23 : 26;
            pedido.ESTADO = 2;
            pedido.ES_CLIENTE_HABITUAL = true;
            pedido.ID_GVA01 = ordenFull.ID_GVA01;
            pedido.ID_GVA14 = ordenFull.ID_GVA14;
            pedido.ID_GVA24 = ordenFull.ID_GVA24;
            pedido.ID_GVA10 = ordenFull.ID_GVA10;
            pedido.ID_GVA23 = ordenFull.ID_GVA23.HasValue ? ordenFull.ID_GVA23 : 1;
            pedido.ID_STA22 = 23;
            pedido.FECHA_PEDIDO = orden.Creacion.Value;
            pedido.FECHA_ENTREGA = orden.FechaEntrega != null ? orden.FechaEntrega.Value.AddDays(1) : null;
            pedido.ID_MONEDA = "1";
            pedido.COTIZACION = 1;
            pedido.ID_ASIENTO_MODELO_GV = "14";
            pedido.LEYENDA_1 = ordenFull.Entrega!;
            pedido.LEYENDA_2 = ordenFull.Notas!;

            pedido.RENGLON_DTO = new();
            foreach (var detalle in ordenFull.Detalle)
            {
                if (detalle.CantidadAprobada > 0)
                {
                    RenglonDTO renglonDTO = new();
                    renglonDTO.CANTIDAD_PEDIDA = detalle.CantidadAprobada;
                    renglonDTO.ID_STA11 = detalle.ID_STA11;
                    renglonDTO.PORCENTAJE_BONIFICACION = detalle.Descuento;
                    pedido.RENGLON_DTO.Add(renglonDTO);
                }
            }

            request.AddBody(pedido);

            var response = restClient.Execute(request);
            using JsonDocument doc = JsonDocument.Parse(response.Content);
            JsonElement root = doc.RootElement;
            if (response.IsSuccessStatusCode)
            {
                if (root.TryGetProperty("exceptionInfo", out var exceptionInfo) && exceptionInfo.ValueKind != JsonValueKind.Null)
                {
                    var messages = exceptionInfo.GetProperty("messages");
                    if (messages.ValueKind == JsonValueKind.Array && messages.GetArrayLength() > 0)
                    {
                        string? firstErrorMessage = messages[0].GetString();
                        return new(false, firstErrorMessage);
                    }
                }

                if (root.TryGetProperty("savedId", out JsonElement savedIdElement) && savedIdElement.ValueKind == JsonValueKind.Number)
                {
                    int savedId = savedIdElement.GetInt32();
                    return new(true, savedId.ToString());
                }
            }
            else
            {
                return new(false, "Error inesperado");
            }

            return new(true);
        }

        [HttpPut("{idEstado:int}")]
        public IActionResult CambioEstadoPedido(int idEstado, [FromBody] string id)
        {
            SqlTransaction? trans = null;
            try
            {
                DA_Ordenes daO = new(Configuration.GetConnectionString("DefaultConnection"));
                using (SqlConnection cnn = new(Configuration.GetConnectionString("DefaultConnection")))
                {
                    cnn.Open();
                    trans = cnn.BeginTransaction();
                    daO.PersistirPedidoEstado(id, idEstado, trans);
                    trans.Commit();
                    cnn.Close();
                }

                return Ok();
            }
            catch (Exception ex)
            {
                if (trans != null)
                    trans.Rollback();
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("expediciones")]
        public List<OrdenExpedicion> GetOrdenesExpediciones()
        {
            List<OrdenExpedicion> ordenes = new();
            DA_Ordenes daO = new(Configuration.GetConnectionString("DefaultConnection"));

            using (DataTable dt = daO.ObtenerOrdenExpediciones())
            {
                foreach (DataRow row in dt.Rows)
                {
                    var orden = new OrdenExpedicion()
                    {
                        IdPedidos = row["IdPedidos"].ToString(),
                        Orden = row["Orden"].ToString(),
                        Fecha = DateTime.Parse(row["AltaRegistro"].ToString()),
                        Linea = row["Linea"].ToString(),
                        CodCliente = row["CodigoCliente"].ToString(),
                        RazonSocial = row["RazonSocial"].ToString(),
                        Articulos = int.Parse(row["Articulos"].ToString()),
                        Impreso = bool.Parse(row["Impreso"].ToString())
                    };
                    if (row["FechaEntrega"] != DBNull.Value) orden.FechaEntrega = DateTime.Parse(row["FechaEntrega"].ToString());
                    ordenes.Add(orden);
                }
            }

            return ordenes;
        }

        [HttpGet("expedicion")]
        public OrdenExpedicion GetOrdenExpedicion(string idOrden, int? idEstado = null, SqlTransaction? trans = null)
        {
            OrdenExpedicion orden = new()
            {
                Detalle = new()
            };
            DA_Ordenes daO = new(Configuration.GetConnectionString("DefaultConnection"));
            using (DataTable dt = daO.ObtenerOrdenExpediciones(idOrden, idEstado, trans))
            {
                DataRow row = dt.Rows[0];
                orden.IdPedidos = row["IdPedidos"].ToString();
                orden.Orden = idOrden;
                if (row["CodigoTango"] != DBNull.Value) orden.CodTango = row["CodigoTango"].ToString();
                if (row["FechaEntrega"] != DBNull.Value) orden.FechaEntrega = DateTime.Parse(row["FechaEntrega"].ToString());
                orden.Fecha = DateTime.Parse(row["AltaRegistro"].ToString());
                orden.Linea = row["Linea"].ToString();
                orden.LetrasOrden = row["LetrasOrdenes"].ToString();
                orden.CodCliente = row["CodigoCliente"].ToString();
                orden.RazonSocial = row["RazonSocial"].ToString();
                orden.CUIT = row["CUIT"].ToString();
                orden.CondicionIVA = row["CondicionIVA"].ToString();
                orden.Articulos = int.Parse(row["Articulos"].ToString());
                orden.EntregarEn = row["EntregarEn"].ToString();
                if (row["Transporte"] != DBNull.Value) orden.Transporte = row["Transporte"].ToString();
                if (row["Zona"] != DBNull.Value) orden.Zona = row["Zona"].ToString();
                if (row["Observaciones"] != DBNull.Value) orden.Observaciones = row["Observaciones"].ToString();
                orden.Vendedor = row["Vendedor"].ToString();
                orden.Impreso = bool.Parse(row["Impreso"].ToString());
            }

            using (DataTable dt = daO.ObtenerOrdenDetalleExpedicion(idOrden))
            {
                int i = 1;
                foreach (DataRow row in dt.Rows)
                {
                    OrdenExpedicionDetalle linea = new()
                    {
                        IdProducto = row["IdProducto"].ToString(),
                        CodProducto = row["CodProducto"].ToString(),
                        DescripcionProducto = row["Descripcion"].ToString(),
                        NumLinea = i,
                        CantidadF = int.Parse(row["CantidadF"].ToString()),
                        CantidadX = int.Parse(row["CantidadX"].ToString()),
                        //CantidadCanceladaF = int.Parse(row["CantidadCanceladaF"].ToString()),
                        //CantidadCanceladaX = int.Parse(row["CantidadCanceladaX"].ToString()),
                        CantidadAprobadaF = int.Parse(row["CantidadAprobadaF"].ToString()),
                        CantidadAprobadaX = int.Parse(row["CantidadAprobadaX"].ToString()),
                        CantidadProbadorF = int.Parse(row["CantidadProbadorF"].ToString()),
                        CantidadProbadorX = int.Parse(row["CantidadProbadorX"].ToString()),
                        CantidadProbadorAprobadaF = int.Parse(row["CantidadProbadorAprobadaF"].ToString()),
                        CantidadProbadorAprobadaX = int.Parse(row["CantidadProbadorAprobadaX"].ToString()),
                        //CantidadProbadorCanceladaF = int.Parse(row["CantidadProbadorCanceladaF"].ToString()),
                        //CantidadProbadorCanceladaX = int.Parse(row["CantidadProbadorCanceladaF"].ToString())
                    };

                    orden.Detalle.Add(linea);
                    i++;
                }
            }
            return orden;
        }

        [HttpPost("expediciondetalle")]
        public IActionResult PostExpedicionDetalle(OrdenExpedicion orden)
        {
            SqlTransaction? trans = null;
            try
            {
                DA_Ordenes daO = new(Configuration.GetConnectionString("DefaultConnection"));
                using (SqlConnection cnn = new(Configuration.GetConnectionString("DefaultConnection")))
                {
                    cnn.Open();
                    trans = cnn.BeginTransaction();

                    foreach (var linea in orden.Detalle.Where(x => x.HuboCambios))
                    {
                        if (orden.LetrasOrden.Contains("F"))
                            daO.UpdatePedidoDetalle("F-" + orden.Orden, linea.CodProducto, linea.CantidadAprobadaF, linea.CantidadProbadorAprobadaF, trans);

                        if (orden.LetrasOrden.Contains("X"))
                            daO.UpdatePedidoDetalle("X-" + orden.Orden, linea.CodProducto, linea.CantidadAprobadaX, linea.CantidadProbadorAprobadaX, trans);
                    }
                    trans.Commit();
                    cnn.Close();
                }

                return Ok();
            }
            catch (Exception ex)
            {
                if (trans != null)
                    trans.Rollback();
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("despachar")]
        public IActionResult DespacharOrdenes(List<OrdenExpedicion> ordenes)
        {
            SqlTransaction? trans = null;
            try
            {
                List<OrdenExpedicion> ordenesAux = new();
                DA_Ordenes daO = new(Configuration.GetConnectionString("DefaultConnection"));
                foreach (var orden in ordenes)
                {
                    var detalle = daO.ObtenerOrdenDetalleExpedicion(orden.Orden);
                    if (detalle.AsEnumerable().Any(x => int.Parse(x["CantidadAprobadaF"].ToString()) != 0 || int.Parse(x["CantidadAprobadaX"].ToString()) != 0))
                        ordenesAux.Add(orden);
                    else
                    {
                        ModelState.AddModelError("error", $"Todos los productos de la orden {orden.Orden} están pendientes");
                    }
                }

                if (ordenesAux.Any())
                {
                    using (SqlConnection cnn = new SqlConnection(Configuration.GetConnectionString("DefaultConnection")))
                    {
                        cnn.Open();
                        trans = cnn.BeginTransaction();
                        foreach (var orden in ordenesAux)
                        {
                            foreach (var id in orden.IdPedidos.Split(","))
                            {
                                daO.PersistirPedidoEstado(id, 4, trans);
                            }
                        }
                        trans.Commit();
                        cnn.Close();
                    }
                }

                if (ModelState.ErrorCount == 0)
                    return Ok();
                else
                    return BadRequest(ModelState);
            }
            catch (Exception ex)
            {
                if (trans != null)
                    trans.Rollback();
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("expedicionImprimir")]
        public OrdenExpedicion GetOrdenExpedicionImprimir(string idOrden)
        {
            DA_Ordenes daO = new(Configuration.GetConnectionString("DefaultConnection"));
            OrdenExpedicion orden = GetOrdenExpedicion(idOrden);
            var a = JsonConvert.SerializeObject(orden);
            foreach (var idPedido in orden.IdPedidos.Split(","))
                daO.PersistirPedidoImpresion(idPedido);
            return orden;
        }

        [HttpGet("cantidadesproductos")]
        public CantidadesProductosDashboard GetCantidadesDeProductos(int idUsuario)
        {
            DA_Ordenes daO = new(Configuration.GetConnectionString("DefaultConnection"));
            CantidadesProductosDashboard c = new();
            using (DataTable dt = daO.GetCantidadesProductos(idUsuario))
            {
                foreach (DataRow row in dt.Rows)
                {
                    if (row["TotalCantidadAprobada"] != DBNull.Value) c.CantidadAprobada = int.Parse(row["TotalCantidadAprobada"].ToString());
                    else c.CantidadAprobada = 0;
                    if (row["TotalCantidadPendiente"] != DBNull.Value) c.CantidadPendiente = int.Parse(row["TotalCantidadPendiente"].ToString());
                    else c.CantidadPendiente = 0;
                }
            }

            return c;
        }
    }
}
