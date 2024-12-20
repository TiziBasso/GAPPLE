using GAPPLE.Client.Entities;
using GAPPLE.Client.Pages;
using GAPPLE.Server.Data;
using GAPPLE.Shared.Model;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;
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
                        Linea = row["Linea"].ToString(),
                        Creacion = DateTime.Parse(row["AltaRegistro"].ToString()!),
                        Zona = row["DescripcionZona"].ToString(),
                        IdEstado = (int)row["IdEstado"],
                        DescripcionEstado = row["DescripcionEstado"].ToString(),
                        IdTango = row["CodigoTango"].ToString(),
                        NumeroFactura = row["NumFactura"].ToString(),
                    };
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
                        daO.PersistirPedidoCabecera("F-" + pedido.CodigoOrden, pedido.Linea!, pedido.CodCliente!, pedido.Detalle!.Count, (int)pedido.IdEstado!,
                                                            pedido.Zona!, pedido.CodListaPrecio, pedido.Factura, false,
                                                            pedido.CodTransporte!, pedido.CondicionVenta!, pedido.Entrega!, pedido.Probadores,
                                                            OCCD: pedido.Obsequios, MtEX: pedido.Exhibidor, pedido.Notas!, pedido.FechaEntrega!.Value, "Prueba", trans);
                        int numLinea = 0;
                        foreach (var item in pedido.Detalle!)
                        {
                            numLinea++;
                            daO.PersistirPedidoDetalle("F-" + pedido.CodigoOrden, numLinea, item.CodProducto!, item.Cantidad, item.Probador, item.TotalDescuento, trans);
                        }
                    }

                    if (pedido.Presupuesto)
                    {
                        daO.PersistirPedidoCabecera("X-" + pedido.CodigoOrden, pedido.Linea!, pedido.CodCliente!, pedido.Detalle!.Count, (int)pedido.IdEstado!,
                                                                pedido.Zona!, pedido.CodListaPrecio, false, pedido.Presupuesto,
                                                                pedido.CodTransporte!, pedido.CondicionVenta!, pedido.Entrega!, pedido.Probadores,
                                                                OCCD: pedido.Obsequios, MtEX: pedido.Exhibidor, pedido.Notas!, pedido.FechaEntrega!.Value, "Prueba", trans);
                        int numLinea = 0;
                        foreach (var item in pedido.Detalle!)
                        {
                            numLinea++;
                            daO.PersistirPedidoDetalle("X-" + pedido.CodigoOrden, numLinea, item.CodProducto!, item.Cantidad, item.Probador, item.TotalDescuento, trans);
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
        public IActionResult CambioEstadoPedido(int idEstado, [FromBody] string ids)
        {
            SqlTransaction? trans = null;
            try
            {
                var lstId = ids.Split(',');
                DA_Ordenes daO = new(Configuration.GetConnectionString("DefaultConnection"));
                using (SqlConnection cnn = new(Configuration.GetConnectionString("DefaultConnection")))
                {
                    cnn.Open();
                    trans = cnn.BeginTransaction();
                    foreach (var id in lstId)
                    {
                        daO.PersistirPedidoEstado(id, idEstado, trans);
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
    }
}
