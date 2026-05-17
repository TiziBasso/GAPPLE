using GAPPLE.Client.Pages;
using GAPPLE.Server.Data;
using GAPPLE.Server.Tools;
using GAPPLE.Shared.Model;
using Integra.Web.Server.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Data;

namespace GAPPLE.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OfertasController : ControllerBase
    {
        private IConfiguration Configuration { get; }
        private Usuario Usuario { get; }
        private readonly IHubContext<SignalRController> HubContext;

        public OfertasController(IConfiguration configuration, IHubContext<SignalRController> hub = null)
        {
            Configuration = configuration;
            HubContext = hub;
        }

        [HttpGet]
        public List<Oferta> GetOfertas(string? nombre, string? linea, DateTime? mes, bool? activas)
        {
            DA_Ofertas daO = new(Configuration.GetConnectionString("DefaultConnection"));
            List<Oferta> lstOfertas = new List<Oferta>();
            using (DataTable dt = daO.ObtenerOfertas(nombre, linea, mes, activas))
            {
                foreach (DataRow row in dt.Rows)
                {
                    Oferta o = new()
                    {
                        IdOferta = int.Parse(row["IdOferta"].ToString()!),
                        Nombre = row["Nombre"].ToString()!,
                        Linea = row["Linea"].ToString()!,
                        Descuento = decimal.Parse(row["Descuento"].ToString()!),
                        Desde = DateTime.Parse(row["Desde"].ToString()!),
                        Hasta = DateTime.Parse(row["Hasta"].ToString()!),
                        Activa = bool.Parse(row["Activo"].ToString()!),
                        Descripcion = row["Descripcion"].ToString()!,
                        Inclusiones = row["Inclusiones"].ToString(),
                        AltaRegistro = Convert.ToDateTime(row["AltaRegistro"]),
                        AltaUsuario = Convert.ToString(row["AltaUsuario"]),
                    };

                    if (row["EdicionRegistro"] != DBNull.Value)
                    {
                        o.EdicionRegistro = Convert.ToDateTime(row["EdicionRegistro"]);
                        o.EdicionUsuario = Convert.ToString(row["EdicionUsuario"]);
                    }
                    lstOfertas.Add(o);
                }
            }
            return lstOfertas;
        }

        [HttpGet("especiales")]
        public List<Oferta> GetOfertasEspeciales(string nombre, string linea, DateTime? mes, bool? activas, string codCliente)
        
        {
            DA_Ofertas daO = new(Configuration.GetConnectionString("DefaultConnection"));
            List<Oferta> lstOfertas = new List<Oferta>();
            using (DataTable dt = daO.ObtenerOfertasEspeciales(nombre, linea, mes, activas, codCliente))
            {
                foreach (DataRow row in dt.Rows)
                {
                    Oferta o = new()
                    {
                        IdOferta = int.Parse(row["IdOferta"].ToString()!),
                        Nombre = row["Nombre"].ToString()!,
                        Linea = row["Linea"].ToString()!,
                        Descuento = decimal.Parse(row["Descuento"].ToString()!),
                        Desde = DateTime.Parse(row["Desde"].ToString()!),
                        Hasta = DateTime.Parse(row["Hasta"].ToString()!),
                        Activa = bool.Parse(row["Activo"].ToString()!),
                        Descripcion = row["Descripcion"].ToString()!,
                        Inclusiones = row["Inclusiones"].ToString(),
                        CodCliente = row["CodCliente"].ToString(),
                        RazonSocial = row["RazonSocial"].ToString(),
                        AltaRegistro = Convert.ToDateTime(row["AltaRegistro"]),
                        AltaUsuario = Convert.ToString(row["AltaUsuario"]),
                    };

                    if (row["EdicionRegistro"] != DBNull.Value)
                    {
                        o.EdicionRegistro = Convert.ToDateTime(row["EdicionRegistro"]);
                        o.EdicionUsuario = Convert.ToString(row["EdicionUsuario"]);
                    }
                    lstOfertas.Add(o);
                }
            }
            return lstOfertas;
        }

        [HttpPost]
        public IActionResult PostOfertas(Oferta oferta)
        {
            DA_Ofertas daO = new(Configuration.GetConnectionString("DefaultConnection"));
            daO.PersistirOferta(oferta.Nombre, oferta.Linea, oferta.Descripcion, oferta.Descuento, oferta.Desde, oferta.Hasta, oferta.Inclusiones!, oferta.AltaUsuario);
            return Ok();
        }

        [HttpPut]
        public IActionResult PutOfertas(Oferta oferta)
        {
            DA_Ofertas daO = new(Configuration.GetConnectionString("DefaultConnection"));
            daO.EditarOferta(oferta.IdOferta, oferta.Nombre, oferta.Linea, oferta.Descripcion, oferta.Descuento, oferta.Desde, oferta.Hasta, oferta.Inclusiones!, oferta.EdicionUsuario, null, oferta.Activa);
            return Ok();
        }

        [HttpPost("Especial")]
        public IActionResult PostOfertasEspecial(Oferta oferta)
        {
            DA_Ofertas daO = new(Configuration.GetConnectionString("DefaultConnection"));
            daO.PersistirOferta(oferta.Nombre, oferta.Linea, oferta.Descripcion, oferta.Descuento, oferta.Desde, oferta.Hasta, oferta.Inclusiones!, oferta.AltaUsuario, oferta.CodCliente);
            return Ok();
        }

        [HttpPut("Especial")]
        public IActionResult PutOfertasEspecial(Oferta oferta)
        {
            DA_Ofertas daO = new(Configuration.GetConnectionString("DefaultConnection"));
            daO.EditarOferta(oferta.IdOferta, oferta.Nombre, oferta.Linea, oferta.Descripcion, oferta.Descuento, oferta.Desde, oferta.Hasta, oferta.Inclusiones!, oferta.EdicionUsuario, oferta.CodCliente, oferta.Activa);
            return Ok();
        }

        [HttpPost("procesar")]
        public async Task<IActionResult> ProcesarArchivo(OfertaExcelRequest req)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(req.Linea))
                {
                    ModelState.AddModelError("Linea", "Debe seleccionar una línea antes de procesar el archivo");
                    return BadRequest(ModelState);
                }

                if (req.Hasta < req.Desde)
                {
                    ModelState.AddModelError("Fechas", "La fecha hasta no puede ser anterior a la fecha desde");
                    return BadRequest(ModelState);
                }

                DA_Producto daP = new(Configuration.GetConnectionString("DefaultConnection"));
                Dictionary<string, ProductoParaOfertas> productosLinea = new(StringComparer.OrdinalIgnoreCase);
                using (DataTable dtProd = daP.GetProductosParaOfertas(req.Linea, null))
                {
                    foreach (DataRow row in dtProd.Rows)
                    {
                        var p = new ProductoParaOfertas
                        {
                            Familia = row["Familia"]?.ToString(),
                            CodigoProducto = row["CodigoProducto"].ToString()!,
                            Descripcion = row["Descripcion"]?.ToString(),
                        };
                        productosLinea[p.CodigoProducto] = p;
                    }
                }

                DataTable dt = ManejoDeArchivos.ExcelToDataTable(req.File, false, true);

                if (dt.Rows.Count == 0)
                {
                    ModelState.AddModelError("El archivo", "Se encuentra vacío o no se pudo leer");
                    return BadRequest(ModelState);
                }

                Dictionary<string, List<(string Cod, decimal Desc, string Row)>> porTitulo =
                    new(StringComparer.OrdinalIgnoreCase);

                SignalRController srC = new();
                int i = 0;

                foreach (DataRow row in dt.Rows)
                {
                    string titulo = row["0"]?.ToString()?.Trim();
                    string sku = NormalizarSku(row["1"]?.ToString()?.Trim());
                    string descStr = row["2"]?.ToString()?.Trim();
                    string originalRow = row["originalRow"].ToString();

                    if (string.IsNullOrEmpty(titulo))
                        ModelState.AddModelError($"En la fila {originalRow}", "La columna 'Título de Oferta' está vacía");

                    if (string.IsNullOrEmpty(sku))
                        ModelState.AddModelError($"En la fila {originalRow}", "La columna 'SKU' está vacía");

                    if (string.IsNullOrEmpty(descStr))
                        ModelState.AddModelError($"En la fila {originalRow}", "La columna 'Descuento' está vacía");

                    decimal descuento = 0;
                    bool descOk = !string.IsNullOrEmpty(descStr)
                        && (decimal.TryParse(descStr.Replace(",", "."), System.Globalization.NumberStyles.Any,
                            System.Globalization.CultureInfo.InvariantCulture, out descuento)
                            || decimal.TryParse(descStr, out descuento));

                    if (!string.IsNullOrEmpty(descStr) && !descOk)
                        ModelState.AddModelError($"En la fila {originalRow}", $"El descuento '{descStr}' no es un número válido");
                    else if (descOk && (descuento <= 0 || descuento > 100))
                        ModelState.AddModelError($"En la fila {originalRow}", $"El descuento debe estar entre 0,01 y 100 (valor: {descuento})");

                    if (!string.IsNullOrEmpty(sku) && !productosLinea.ContainsKey(sku))
                        ModelState.AddModelError($"En la fila {originalRow}", $"El SKU '{sku}' no existe en la línea {req.Linea} o se encuentra pasivo");

                    if (!string.IsNullOrEmpty(titulo) && !string.IsNullOrEmpty(sku) && descOk && descuento > 0 && descuento <= 100 && productosLinea.ContainsKey(sku))
                    {
                        if (!porTitulo.TryGetValue(titulo, out var lista))
                        {
                            lista = new();
                            porTitulo[titulo] = lista;
                        }
                        lista.Add((sku, descuento, originalRow));
                    }

                    i++;
                    if (!string.IsNullOrEmpty(req.ConnectionId) && HubContext != null)
                        await srC.CambiarPorcentajeTarea(HubContext.Clients, req.ConnectionId, i * 100 / dt.Rows.Count);
                }

                List<Oferta> ofertas = new();
                foreach (var kv in porTitulo)
                {
                    var descuentos = kv.Value.Select(x => x.Desc).Distinct().ToList();
                    if (descuentos.Count > 1)
                    {
                        var filas = string.Join(", ", kv.Value.Select(x => x.Row));
                        ModelState.AddModelError($"Título '{kv.Key}'",
                            $"Todas las filas con el mismo título deben tener el mismo descuento. Filas afectadas: {filas}");
                        continue;
                    }

                    var skusDuplicados = kv.Value.GroupBy(x => x.Cod).Where(g => g.Count() > 1).Select(g => g.Key).ToList();
                    if (skusDuplicados.Any())
                        ModelState.AddModelError($"Título '{kv.Key}'",
                            $"Los siguientes SKUs están duplicados dentro de la misma oferta: {string.Join(", ", skusDuplicados)}");

                    var skus = kv.Value.Select(x => x.Cod).Distinct().ToList();
                    ofertas.Add(new Oferta
                    {
                        Nombre = kv.Key,
                        Linea = req.Linea,
                        Descuento = descuentos.First(),
                        Desde = req.Desde,
                        Hasta = req.Hasta.AddHours(23).AddMinutes(59).AddSeconds(59),
                        Activa = true,
                        Inclusiones = string.Join('|', skus),
                        CodCliente = string.IsNullOrWhiteSpace(req.CodCliente) ? null : req.CodCliente,
                        Descripcion = $"Carga masiva ({skus.Count} productos)"
                    });
                }

                if (ModelState.ErrorCount > 0)
                    return BadRequest(ModelState);

                if (!ofertas.Any())
                {
                    ModelState.AddModelError("El archivo", "No se encontraron filas válidas para crear ofertas");
                    return BadRequest(ModelState);
                }

                return Ok(ofertas);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("masivo")]
        public IActionResult PostOfertasMasivo(List<Oferta> ofertas)
        {
            try
            {
                if (ofertas == null || !ofertas.Any())
                {
                    ModelState.AddModelError("Ofertas", "No se recibieron ofertas para crear");
                    return BadRequest(ModelState);
                }

                DA_Ofertas daO = new(Configuration.GetConnectionString("DefaultConnection"));
                foreach (var o in ofertas)
                {
                    daO.PersistirOferta(
                        o.Nombre,
                        o.Linea,
                        o.Descripcion,
                        o.Descuento,
                        o.Desde,
                        o.Hasta,
                        o.Inclusiones!,
                        o.AltaUsuario,
                        string.IsNullOrWhiteSpace(o.CodCliente) ? null : o.CodCliente);
                }
                return Ok(new { creadas = ofertas.Count });
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        private static string NormalizarSku(string sku)
        {
            if (string.IsNullOrEmpty(sku)) return sku;

            int sepIdx = sku.IndexOfAny(new[] { '.', ',' });
            if (sepIdx > 0)
            {
                string parteEntera = sku.Substring(0, sepIdx);
                string parteDecimal = sku.Substring(sepIdx + 1);
                if (parteDecimal.Length > 0 && parteDecimal.All(c => c == '0'))
                    return parteEntera;
            }

            return sku;
        }
    }
}
