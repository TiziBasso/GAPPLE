using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using GAPPLE.Shared.Model;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace GAPPLE.Server.Helpers
{
    /// <summary>
    /// Lee el Excel de lista de precios que se manda a los vendedores (ej. "Lista 174 (Septiembre 2026).xlsx"):
    /// <list type="bullet">
    /// <item>Arriba, un titulo "Lista 174 vigente a partir de 19/09/2026": de ahi salen el nombre y la vigencia.</item>
    /// <item>Una fila de encabezados: SKU | DESCRIPCION | (tono, sin titulo) | EAN | CONTENIDO | Unid x caja |
    /// Costo Neto S/IVA | Costo Neto C/IVA | Pr. Sugerido Público. Las columnas se ubican por el encabezado, no por la posicion.</item>
    /// <item>Filas de familia (una sola celda con texto, ej. "SHAMPOO Y ACONDICIONADORES") seguidas de los articulos de la familia.</item>
    /// </list>
    /// Los costos suelen ser formulas: se toma el valor calculado que guarda el Excel, redondeado a 2 decimales.
    /// </summary>
    internal static class ListaPrecioExcelParser
    {
        private const int MaxFilasBusquedaEncabezado = 40;
        private const int MaxAdvertencias = 30;
        private static readonly CultureInfo CulturaAR = new("es-AR");
        private static readonly string[] FormatosFecha = { "d/M/yyyy", "d/M/yy", "d-M-yyyy", "d-M-yy", "d.M.yyyy", "d.M.yy" };
        private static readonly Regex RegexTitulo = new(
            @"^(?<nombre>.*?)[\s\-–:,]*vigente\s+(?:a\s+partir\s+)?(?:del?\s+)?(?<fecha>\d{1,2}[/\-.]\d{1,2}[/\-.]\d{2,4})",
            RegexOptions.IgnoreCase | RegexOptions.Compiled);

        private class Columnas
        {
            public int Sku = -1, Descripcion = -1, Tono = -1, Ean = -1, Contenido = -1, UnidadesPorCaja = -1,
                       CostoSinIva = -1, CostoConIva = -1, Precio = -1;

            public bool TienePrecios => CostoSinIva >= 0 || CostoConIva >= 0 || Precio >= 0;

            public bool EstaUsada(int col) =>
                new[] { Sku, Descripcion, Tono, Ean, Contenido, UnidadesPorCaja, CostoSinIva, CostoConIva, Precio }.Contains(col);
        }

        /// <summary>
        /// Devuelve la lista leida, o null si el archivo no se puede usar (con el motivo en <paramref name="errores"/>).
        /// Los problemas por fila que no impiden guardar van en <see cref="ListaPrecioPrevisualizacion.Advertencias"/>.
        /// </summary>
        public static ListaPrecioPrevisualizacion Leer(byte[] archivo, string nombreArchivo, List<string> errores)
        {
            IWorkbook libro;
            try
            {
                using var ms = new MemoryStream(archivo);
                libro = new XSSFWorkbook(ms);
            }
            catch
            {
                errores.Add("No se pudo leer el archivo. Verifique que sea un Excel .xlsx");
                return null;
            }

            for (int s = 0; s < libro.NumberOfSheets; s++)
            {
                ISheet hoja = libro.GetSheetAt(s);
                var columnas = BuscarEncabezado(hoja, out int filaEncabezado);
                if (columnas == null)
                    continue;

                if (!columnas.TienePrecios)
                {
                    errores.Add("No se encontraron columnas de precio (Costo Neto S/IVA, Costo Neto C/IVA o Pr. Sugerido Público)");
                    return null;
                }

                var resultado = LeerHoja(hoja, filaEncabezado, columnas, nombreArchivo);
                if (resultado.Items.Count == 0)
                {
                    errores.Add("El archivo no tiene artículos con precio");
                    return null;
                }
                return resultado;
            }

            errores.Add("No se encontró la fila de encabezados: debe tener al menos las columnas SKU y DESCRIPCION");
            return null;
        }

        private static Columnas BuscarEncabezado(ISheet hoja, out int filaEncabezado)
        {
            filaEncabezado = -1;
            int ultima = Math.Min(hoja.LastRowNum, hoja.FirstRowNum + MaxFilasBusquedaEncabezado);
            for (int r = hoja.FirstRowNum; r <= ultima; r++)
            {
                IRow fila = hoja.GetRow(r);
                if (fila == null || fila.FirstCellNum < 0)
                    continue;

                var c = new Columnas();
                for (int i = fila.FirstCellNum; i < fila.LastCellNum; i++)
                {
                    string h = Normalizar(Texto(fila.GetCell(i)));
                    if (string.IsNullOrEmpty(h))
                        continue;

                    if (c.Sku < 0 && h is "SKU" or "CODIGO" or "CODIGOPRODUCTO" or "CODARTICULO" or "ARTICULO")
                        c.Sku = i;
                    else if (c.Descripcion < 0 && h.StartsWith("DESCRIPCION"))
                        c.Descripcion = i;
                    else if (c.Tono < 0 && h == "TONO")
                        c.Tono = i;
                    else if (c.Ean < 0 && (h == "EAN" || h.Contains("BARRA")))
                        c.Ean = i;
                    else if (c.Contenido < 0 && (h.StartsWith("CONTENIDO") || h.StartsWith("PRESENTACION")))
                        c.Contenido = i;
                    else if (c.UnidadesPorCaja < 0 && h.StartsWith("UNID") && h.Contains("CAJA"))
                        c.UnidadesPorCaja = i;
                    else if (c.Precio < 0 && (h.Contains("SUGERIDO") || h.Contains("PUBLICO")))
                        c.Precio = i;
                    else if (h.StartsWith("COSTO"))
                    {
                        // "Costo Neto C/IVA" -> COSTONETOCIVA, "Costo Neto S/IVA" -> COSTONETOSIVA
                        if (c.CostoConIva < 0 && (h.EndsWith("CIVA") || h.Contains("CONIVA")))
                            c.CostoConIva = i;
                        else if (c.CostoSinIva < 0 && (h.EndsWith("SIVA") || h.Contains("SINIVA")))
                            c.CostoSinIva = i;
                    }
                }

                if (c.Sku < 0 || c.Descripcion < 0)
                    continue;

                // La columna sin titulo pegada a DESCRIPCION trae el numero de tono en las tinturas (1A, 6.1, ...)
                int siguiente = c.Descripcion + 1;
                if (c.Tono < 0 && !c.EstaUsada(siguiente) && Texto(fila.GetCell(siguiente)) == null)
                    c.Tono = siguiente;

                filaEncabezado = r;
                return c;
            }
            return null;
        }

        private static ListaPrecioPrevisualizacion LeerHoja(ISheet hoja, int filaEncabezado, Columnas c, string nombreArchivo)
        {
            var resultado = new ListaPrecioPrevisualizacion();
            LeerTitulo(hoja, filaEncabezado, resultado);
            if (string.IsNullOrWhiteSpace(resultado.Nombre))
                resultado.Nombre = Recortar(Regex.Replace(Path.GetFileNameWithoutExtension(nombreArchivo ?? string.Empty), @"\s+", " "), 150);

            int advertenciasDeMas = 0;
            void Advertir(string mensaje)
            {
                if (resultado.Advertencias.Count < MaxAdvertencias)
                    resultado.Advertencias.Add(mensaje);
                else
                    advertenciasDeMas++;
            }

            string familia = null;
            var skusVistos = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);

            for (int r = filaEncabezado + 1; r <= hoja.LastRowNum; r++)
            {
                IRow fila = hoja.GetRow(r);
                if (fila == null)
                    continue;

                var textos = fila.Select(Texto).Where(x => x != null).ToList();
                if (textos.Count == 0)
                    continue;

                int nroFila = r + 1;
                string sku = Texto(Celda(fila, c.Sku));
                decimal? sinIva = Numero(Celda(fila, c.CostoSinIva));
                decimal? conIva = Numero(Celda(fila, c.CostoConIva));
                decimal? precio = Numero(Celda(fila, c.Precio));

                if (sinIva == null && conIva == null && precio == null)
                {
                    if (textos.Count == 1)
                    {
                        familia = Recortar(textos[0], 150);
                        resultado.CantidadFamilias++;
                    }
                    else
                        Advertir($"Fila {nroFila}{(sku != null ? $" (SKU {sku})" : string.Empty)}: no tiene precios, se omitió");
                    continue;
                }

                if (sku == null)
                {
                    Advertir($"Fila {nroFila}: tiene precios pero no tiene SKU, se omitió");
                    continue;
                }

                if (skusVistos.TryGetValue(sku, out int filaAnterior))
                    Advertir($"Fila {nroFila}: el SKU {sku} ya figura en la fila {filaAnterior}");
                else
                    skusVistos[sku] = nroFila;

                string unidades = Texto(Celda(fila, c.UnidadesPorCaja));
                resultado.Items.Add(new ListaPrecioImportadaItem
                {
                    Orden = resultado.Items.Count + 1,
                    Familia = familia,
                    SKU = Recortar(sku, 50),
                    Descripcion = Recortar(Texto(Celda(fila, c.Descripcion)), 250),
                    Tono = Recortar(Texto(Celda(fila, c.Tono)), 50),
                    EAN = Recortar(Texto(Celda(fila, c.Ean)), 50),
                    Contenido = Recortar(Texto(Celda(fila, c.Contenido)), 100),
                    UnidadesPorCaja = int.TryParse(unidades, NumberStyles.Integer, CultureInfo.InvariantCulture, out int u) ? u : null,
                    CostoNetoSinIva = sinIva,
                    CostoNetoConIva = conIva,
                    PrecioSugeridoPublico = precio
                });
            }

            if (advertenciasDeMas > 0)
                resultado.Advertencias.Add($"... y {advertenciasDeMas} advertencias más");

            return resultado;
        }

        /// <summary>Busca arriba de los encabezados un titulo tipo "Lista 174 vigente a partir de 19/09/2026".</summary>
        private static void LeerTitulo(ISheet hoja, int filaEncabezado, ListaPrecioPrevisualizacion resultado)
        {
            var textos = new List<string>();
            for (int r = hoja.FirstRowNum; r < filaEncabezado; r++)
            {
                IRow fila = hoja.GetRow(r);
                if (fila != null)
                    textos.AddRange(fila.Select(Texto).Where(x => x != null));
            }

            string titulo = textos.FirstOrDefault(x => x.Contains("lista", StringComparison.OrdinalIgnoreCase)) ?? textos.FirstOrDefault();
            if (titulo == null)
                return;

            var match = RegexTitulo.Match(titulo);
            if (match.Success && !string.IsNullOrWhiteSpace(match.Groups["nombre"].Value))
            {
                resultado.Nombre = Recortar(match.Groups["nombre"].Value, 150);
                if (DateTime.TryParseExact(match.Groups["fecha"].Value.Trim(), FormatosFecha, CulturaAR, DateTimeStyles.None, out var fecha))
                    resultado.VigenciaDesde = fecha;
            }
            else
                resultado.Nombre = Recortar(titulo, 150);
        }

        private static ICell Celda(IRow fila, int col) => col < 0 ? null : fila.GetCell(col);

        private static string Texto(ICell celda)
        {
            if (celda == null)
                return null;

            var tipo = celda.CellType == CellType.Formula ? celda.CachedFormulaResultType : celda.CellType;
            string s = tipo switch
            {
                CellType.String => celda.StringCellValue,
                // Los SKU / EAN numericos se guardan como numero: se muestran sin decimales ni notacion cientifica
                CellType.Numeric => celda.NumericCellValue == Math.Floor(celda.NumericCellValue) && Math.Abs(celda.NumericCellValue) < 1e15
                    ? ((long)celda.NumericCellValue).ToString(CultureInfo.InvariantCulture)
                    : celda.NumericCellValue.ToString("0.############", CultureInfo.InvariantCulture),
                CellType.Boolean => celda.BooleanCellValue ? "SI" : "NO",
                _ => null
            };

            s = s?.Trim();
            return string.IsNullOrEmpty(s) ? null : s;
        }

        private static decimal? Numero(ICell celda)
        {
            if (celda == null)
                return null;

            var tipo = celda.CellType == CellType.Formula ? celda.CachedFormulaResultType : celda.CellType;
            try
            {
                if (tipo == CellType.Numeric)
                    return Math.Round((decimal)celda.NumericCellValue, 2, MidpointRounding.AwayFromZero);

                if (tipo == CellType.String)
                {
                    string s = celda.StringCellValue?.Replace("$", string.Empty).Trim();
                    if (decimal.TryParse(s, NumberStyles.Number, CulturaAR, out decimal d))
                        return Math.Round(d, 2, MidpointRounding.AwayFromZero);
                }
            }
            catch (OverflowException) { }

            return null;
        }

        /// <summary>Mayusculas, sin tildes y solo letras/numeros: "Pr. Sugerido Público" -> "PRSUGERIDOPUBLICO".</summary>
        private static string Normalizar(string texto)
        {
            if (string.IsNullOrWhiteSpace(texto))
                return null;

            var sb = new StringBuilder();
            foreach (char ch in texto.Normalize(NormalizationForm.FormD))
            {
                if (CharUnicodeInfo.GetUnicodeCategory(ch) != UnicodeCategory.NonSpacingMark && char.IsLetterOrDigit(ch))
                    sb.Append(char.ToUpperInvariant(ch));
            }
            return sb.ToString();
        }

        private static string Recortar(string texto, int largo)
        {
            texto = texto?.Trim();
            if (string.IsNullOrEmpty(texto))
                return null;
            return texto.Length <= largo ? texto : texto[..largo];
        }
    }
}
