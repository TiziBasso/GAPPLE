using Integra.Web.Server.Controllers;
using Microsoft.AspNetCore.SignalR;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System.Data;
using System.Globalization;

namespace GAPPLE.Server.Tools
{
    public class ManejoDeArchivos
    {
        public static DataTable CSVToDataTable(byte[] file)
        {
            DataTable dt = new();

            using (MemoryStream ms = new(file))
            {
                using (StreamReader reader = new StreamReader(ms))
                {
                    string[] headers = reader.ReadLine().Split(';');

                    foreach (string header in headers)
                    {
                        dt.Columns.Add(Functions.RemoverCaracteresEspeciales(Functions.ReemplazarTildes(header).ToUpper()));
                    }

                    while (!reader.EndOfStream)
                    {
                        string[] rows = reader.ReadLine().Split(";");
                        DataRow dr = dt.NewRow();
                        for (int i = 0; i < headers.Length; i++)
                        {
                            try
                            {
                                dr[i] = rows[i].Trim();
                            }
                            catch
                            {
                                break;
                            }
                        }
                        dt.Rows.Add(dr);
                    }
                }
            }

            return dt;
        }

        private static bool HasAnyValue(DataRow row)
        {
            foreach (var item in row.ItemArray)
            {
                if (item != null && item != DBNull.Value && !string.IsNullOrEmpty(item.ToString().Trim()))
                    return true;
            }
            return false;
        }

        public static DataTable ExcelToDataTable(byte[] b, bool headersName = true, bool rowNumber = false)
        {
            DataTable dt = new();
            XSSFWorkbook sheets;

            using (var stream = new MemoryStream(b))
            {
                sheets = new XSSFWorkbook(stream);
            }

            ISheet sheet = sheets.GetSheetAt(0);
            IRow headerRows = sheet.GetRow(0);
            if (headerRows != null)
            {
                int cellCount = headerRows.LastCellNum;

                for (int i = 0; i <= cellCount - 1; i++)
                {
                    if (headersName)
                    {
                        ICell cell = headerRows.GetCell(i);
                        dt.Columns.Add(Functions.RemoverCaracteresEspeciales(Functions.ReemplazarTildes(cell.ToString()).ToUpper()));
                    }
                    else
                        dt.Columns.Add(i.ToString());
                }
                if (rowNumber)
                    dt.Columns.Add("originalRow");

                for (int i = sheet.FirstRowNum + 1; i <= sheet.LastRowNum; i++)
                {
                    IRow row = sheet.GetRow(i);

                    if (row != null && row.Count() > 0)
                    {
                        if (row.Count(x => x.CellType == CellType.Blank) != cellCount)
                        {
                            DataRow dataRow = dt.NewRow();
                            for (int r = row.FirstCellNum; r <= cellCount - 1; r++)
                            {
                                if (row.GetCell(r) != null)
                                {
                                    dataRow[r] = row.GetCell(r).ToString();
                                }
                            }
                            if (HasAnyValue(dataRow))
                            {
                                if (rowNumber)
                                    dataRow["originalRow"] = i + 1;
                                dt.Rows.Add(dataRow);
                            }
                        }
                    }
                }
            }
            return dt;
        }

        public static DataTable ExcelToDataTable(byte[] b, int cantColumns, bool headersName = true, bool rowNumber = false)
        {
            DataTable dt = new();
            XSSFWorkbook sheets;

            using (var stream = new MemoryStream(b))
            {
                sheets = new XSSFWorkbook(stream);
            }

            ISheet sheet = sheets.GetSheetAt(0);
            IRow headerRows = sheet.GetRow(0);
            if (headerRows != null)
            {
                for (int i = 0; i < cantColumns; i++)
                {
                    if (headersName)
                    {
                        ICell cell = headerRows.GetCell(i);
                        dt.Columns.Add(Functions.RemoverCaracteresEspeciales(Functions.ReemplazarTildes(cell.ToString()).ToUpper()));
                    }
                    else
                        dt.Columns.Add(i.ToString());
                }
                if (rowNumber)
                    dt.Columns.Add("originalRow");

                for (int i = sheet.FirstRowNum + 1; i <= sheet.LastRowNum; i++)
                {
                    IRow row = sheet.GetRow(i);
                    if (row != null && row.Count() > 0)
                    {
                        if (row.Count(x => x.CellType == CellType.Blank) != cantColumns - 1)
                        {
                            DataRow dataRow = dt.NewRow();
                            for (int r = row.FirstCellNum; r < cantColumns; r++)
                            {
                                if (row.GetCell(r) != null)
                                {
                                    dataRow[r] = row.GetCell(r).ToString();
                                }
                            }
                            if (HasAnyValue(dataRow))
                            {
                                if (rowNumber)
                                    dataRow["originalRow"] = i;
                                dt.Rows.Add(dataRow);
                            }
                        }
                    }
                }
            }
            return dt;
        }

        public static IEnumerable<Dictionary<string, string>> DataTableToDictionary(DataTable dataTable)
        {
            IEnumerable<Dictionary<string, string>> lstDictionary = Enumerable.Empty<Dictionary<string, string>>();
            foreach (DataRow row in dataTable.Rows)
            {
                Dictionary<string, string> dictionary = new();

                foreach (DataColumn column in dataTable.Columns)
                {
                    if (int.TryParse(row[column.ColumnName.ToString()].ToString(), out int outInt))
                        dictionary.Add(column.ColumnName.ToString().ToUpper().Replace(" ", ""), outInt.ToString());
                    else if (long.TryParse(row[column.ColumnName.ToString()].ToString(), out long outLong))
                        dictionary.Add(column.ColumnName.ToString().ToUpper().Replace(" ", ""), outLong.ToString());
                    else if (decimal.TryParse(row[column.ColumnName.ToString()].ToString(), out decimal outDecimal))
                        dictionary.Add(column.ColumnName.ToString().ToUpper().Replace(" ", ""), outDecimal.ToString().Replace(",", "."));
                    else if (DateTime.TryParse(row[column.ColumnName.ToString()].ToString(), out DateTime outDateTime))
                        dictionary.Add(column.ColumnName.ToString().ToUpper().Replace(" ", ""), outDateTime.ToString("dd/MM/yyyy"));
                    else
                        dictionary.Add(column.ColumnName.ToString().ToUpper().Replace(" ", ""), row[column.ColumnName.ToString()].ToString());
                }
                lstDictionary = lstDictionary.Append(dictionary);
            }
            return lstDictionary;
        }

        public static async Task<IEnumerable<Dictionary<string, string>>> DataTableToDictionary(DataTable dataTable, IHubClients clientes, string connectionId)
        {
            SignalRController srC = new();
            IEnumerable<Dictionary<string, string>> lstDictionary = Enumerable.Empty<Dictionary<string, string>>();
            int i = 0;
            foreach (DataRow row in dataTable.Rows)
            {
                Dictionary<string, string> dictionary = new();

                foreach (DataColumn column in dataTable.Columns)
                {
                    if (int.TryParse(row[column.ColumnName.ToString()].ToString(), out int outInt))
                        dictionary.Add(column.ColumnName.ToString().ToUpper().Replace(" ", ""), outInt.ToString());
                    else if (long.TryParse(row[column.ColumnName.ToString()].ToString(), out long outLong))
                        dictionary.Add(column.ColumnName.ToString().ToUpper().Replace(" ", ""), outLong.ToString());
                    else if (decimal.TryParse(row[column.ColumnName.ToString()].ToString(), out decimal outDecimal))
                        dictionary.Add(column.ColumnName.ToString().ToUpper().Replace(" ", ""), outDecimal.ToString().Replace(",", "."));
                    else if (DateTime.TryParse(row[column.ColumnName.ToString()].ToString(), out DateTime outDateTime))
                        dictionary.Add(column.ColumnName.ToString().ToUpper().Replace(" ", ""), outDateTime.ToString("dd/MM/yyyy"));
                    else
                        dictionary.Add(column.ColumnName.ToString().ToUpper().Replace(" ", ""), row[column.ColumnName.ToString()].ToString());
                }
                lstDictionary = lstDictionary.Append(dictionary);
                i++;
                await srC.CambiarPorcentajeTarea(clientes, connectionId, i * 100 / dataTable.Rows.Count);
            }
            return lstDictionary;
        }

        public static Dictionary<string, Type> GetHeadersFromDictionary(Dictionary<string, string> dato)
        {
            Dictionary<string, Type> headers = new Dictionary<string, Type>();
            foreach (var p in dato)
            {
                if (int.TryParse(p.Value, out int outInt))
                    headers.Add(p.Key.ToUpper().Replace(" ", ""), typeof(int));
                else if (long.TryParse(p.Value, out long outLong))
                    headers.Add(p.Key.ToUpper().Replace(" ", ""), typeof(long));
                else if (decimal.TryParse(p.Value, out decimal outDecimal))
                    headers.Add(p.Key.ToUpper().Replace(" ", ""), typeof(decimal));
                else if (DateTime.TryParseExact(p.Value, "dd/MM/yyyy", null, DateTimeStyles.AssumeUniversal, out DateTime outDateTime))
                    headers.Add(p.Key.ToUpper().Replace(" ", ""), typeof(DateTime));
                else
                    headers.Add(p.Key.ToUpper().Replace(" ", ""), typeof(string));
            }
            return headers;
        }

        public static Dictionary<string, Type> GetHeadersFromDictionaryAllStrings(Dictionary<string, string> dato)
        {
            Dictionary<string, Type> headers = new Dictionary<string, Type>();
            foreach (var p in dato)
            {
                headers.Add(p.Key.ToUpper().Replace(" ", ""), typeof(string));
            }
            return headers;
        }
    }
}
