using GAPPLE.Client.Entities;
using GAPPLE.Client.Pages;
using GAPPLE.Server.Data;
using GAPPLE.Shared.Model;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Globalization;
using System.Net;

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
        public List<Orden> GetOrdenes(string desdeStr, string hastaStr, int? idPedido, string? codOrden, bool? presupuesto, string? razonSocial,
                                        string? linea, string? zona, int? idEstado, string? codTango)
        {
            DateTime desde, hasta;
            desde = DateTime.Parse(WebUtility.UrlDecode(desdeStr));
            hasta = DateTime.Parse(WebUtility.UrlDecode(hastaStr));
            DA_Ordenes daO = new(Configuration.GetConnectionString("DefaultConnection"));
            List<Orden> lstOrdenes = new();
            using (DataTable dt = daO.ObtenerOrdenes(desde, hasta, idPedido, codOrden, presupuesto, razonSocial, linea, zona, idEstado, codTango))
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
                        Unidades = (int)row["CantidadLineas"]
                    };

                    lstOrdenes.Add(o);
                }
            }
            return lstOrdenes;
        }

        [HttpGet]
        public Orden? GetOrden(string codOrden, bool conDetalle)
        {
            DA_Ordenes daO = new(Configuration.GetConnectionString("DefaultConnection"));
            Orden? orden = null;
            using (DataTable dt = daO.ObtenerOrden(codOrden))
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
                        IdTango = row["CodigoTango"].ToString(),
                        NumeroFactura = row["NumFactura"].ToString(),
                    };
                    if (row["CodTransporte"] != DBNull.Value) orden.CodTransporte = row["CodTransporte"].ToString();
                    if (row["DescripcionTransporte"] != DBNull.Value) orden.Transporte = row["DescripcionTransporte"].ToString();
                }
            }

            if (orden != null && conDetalle)
            {
                using (DataTable dt = daO.ObtenerOrdenDetalle(codOrden))
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
                                IdProducto = (int)dr["IdProducto"],
                                CodProducto = dr["CodProducto"].ToString(),
                                Descripcion = dr["Descripcion"].ToString(),
                                Cantidad = (int)dr["Cantidad"],
                                CantidadAprobada = (int)dr["CantidadAprobada"],
                                CantidadCancelada = (int)dr["CantidadCancelada"]
                            };

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
        public List<OrdenDashboard> GetOrdenesDashboard()
        {
            DA_Ordenes daO = new(Configuration.GetConnectionString("DefaultConnection"));
            List<OrdenDashboard> oDashs = new List<OrdenDashboard>();
            using (DataTable dt = daO.ObtenerOrdenesDashboard())
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
                                                            pedido.CodTransporte!, pedido.CondicionVenta!, pedido.Entrega!,
                                                            pedido.Notas!, pedido.FechaEntrega!.Value, "Prueba", trans);
                        int numLinea = 0;
                        foreach (var item in pedido.Detalle!)
                        {
                            numLinea++;
                            daO.PersistirPedidoDetalle("F-" + pedido.CodigoOrden, numLinea, item.CodProducto!, item.Cantidad, item.CantidadProbador, item.Descuento, trans);
                        }
                    }

                    if (pedido.Presupuesto)
                    {
                        daO.PersistirPedidoCabecera("X-" + pedido.CodigoOrden, pedido.Linea!, pedido.CodCliente!, pedido.Detalle!.Sum(x => x.Cantidad), 1,
                                                                pedido.Zona!, pedido.CodListaPrecio, false, pedido.Presupuesto,
                                                                pedido.CodTransporte!, pedido.CondicionVenta!, pedido.Entrega!,
                                                                pedido.Notas!, pedido.FechaEntrega!.Value, "Prueba", trans);
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
                        FechaEntrega = DateTime.Parse(row["FechaEntrega"].ToString()),
                        Fecha = DateTime.Parse(row["AltaRegistro"].ToString()),
                        Linea = row["Linea"].ToString(),
                        CodCliente = row["CodigoCliente"].ToString(),
                        RazonSocial = row["RazonSocial"].ToString(),
                        Articulos = int.Parse(row["Articulos"].ToString()),
                        Impreso = bool.Parse(row["Impreso"].ToString())
                    };

                    ordenes.Add(orden);
                }
            }

            return ordenes;
        }

        [HttpGet("expedicion")]
        public OrdenExpedicion GetOrdenExpedicion(string idOrden)
        {
            OrdenExpedicion orden = new()
            {
                Detalle = new()
            };
            DA_Ordenes daO = new(Configuration.GetConnectionString("DefaultConnection"));
            using (DataTable dt = daO.ObtenerOrdenExpediciones(idOrden))
            {
                DataRow row = dt.Rows[0];
                orden.IdPedidos = row["IdPedidos"].ToString();
                orden.Orden = idOrden;
                if (row["CodigoTango"] != DBNull.Value) orden.CodTango = row["CodigoTango"].ToString();
                orden.FechaEntrega = DateTime.Parse(row["FechaEntrega"].ToString());
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
                foreach (DataRow row in dt.Rows)
                {
                    OrdenExpedicionDetalle linea = new()
                    {
                        IdProducto = row["IdProducto"].ToString(),
                        CodProducto = row["CodProducto"].ToString(),
                        DescripcionProducto = row["Descripcion"].ToString(),
                        NumLinea = int.Parse(row["NLinea"].ToString()),
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
                            daO.UpdatePedidoDetalle("F-" + orden.Orden, linea.NumLinea, linea.CantidadAprobadaF, linea.CantidadProbadorAprobadaF, trans);

                        if (orden.LetrasOrden.Contains("X"))
                            daO.UpdatePedidoDetalle("X-" + orden.Orden, linea.NumLinea, linea.CantidadAprobadaX, linea.CantidadProbadorAprobadaX, trans);
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
                DA_Ordenes daO = new(Configuration.GetConnectionString("DefaultConnection"));
                using (SqlConnection cnn = new SqlConnection(Configuration.GetConnectionString("DefaultConnection")))
                {
                    cnn.Open();
                    trans = cnn.BeginTransaction();
                    foreach (var orden in ordenes)
                    {
                        var idPedidos = orden.IdPedidos.Split(",");
                        foreach (var id in idPedidos)
                            daO.PersistirPedidoEstado(id, 4, trans);
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

        [HttpGet("expedicionImprimir")]
        public OrdenExpedicion GetOrdenExpedicionImprimir(string idOrden)
        {
            DA_Ordenes daO = new(Configuration.GetConnectionString("DefaultConnection"));
            OrdenExpedicion orden = GetOrdenExpedicion(idOrden);
            foreach (var idPedido in orden.IdPedidos.Split(","))
                daO.PersistirPedidoImpresion(idPedido);
            return orden;
        }
    }
}
