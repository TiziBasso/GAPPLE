using GAPPLE.Server.Data;
using GAPPLE.Shared.Model;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Globalization;

namespace GAPPLE.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StockController : ControllerBase
    {
        private string DefaultConnection { get; }

        public StockController(IConfiguration configuration)
        {
            DefaultConnection = configuration.GetConnectionString("DefaultConnection");
        }

        // GET api/stock?codigo=...&descripcion=...&marca=...
        // Devuelve una fila por producto con los saldos de los depositos 01 y 6 ya
        // pivoteados. La pantalla trae todo el dataset y filtra en memoria.
        [HttpGet]
        public IActionResult GetStock(string? codigo = null, string? descripcion = null, string? marca = null)
        {
            try
            {
                DA_Stock da = new(DefaultConnection);
                List<StockConsolidado> lista = [];

                using DataTable dt = da.ObtenerStock(codigo, descripcion, marca);

                foreach (DataRow row in dt.Rows)
                {
                    string marcaFila = LeerTexto(row, "Marca");

                    lista.Add(new StockConsolidado
                    {
                        Codigo = LeerTexto(row, "Codigo"),
                        Descripcion = LeerTexto(row, "Descripcion"),
                        Marca = string.IsNullOrWhiteSpace(marcaFila) ? StockConsolidado.SinMarca : marcaFila,
                        Dep01 = LeerDecimal(row, "Dep01"),
                        Dep06 = LeerDecimal(row, "Dep06"),
                        Pendientes = LeerDecimal(row, "Pendientes")
                    });
                }

                return Ok(lista);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // ─── Lectura defensiva: si el script del SP todavia no se aplico, la app no rompe ───
        private static string LeerTexto(DataRow row, string columna)
            => row.Table.Columns.Contains(columna) && row[columna] != DBNull.Value
                   ? row[columna].ToString().Trim()
                   : null;

        private static decimal LeerDecimal(DataRow row, string columna)
        {
            if (!row.Table.Columns.Contains(columna) || row[columna] == DBNull.Value)
                return 0;

            return decimal.TryParse(Convert.ToString(row[columna], CultureInfo.InvariantCulture),
                                    NumberStyles.Any, CultureInfo.InvariantCulture, out decimal valor)
                       ? valor
                       : 0;
        }
    }
}
