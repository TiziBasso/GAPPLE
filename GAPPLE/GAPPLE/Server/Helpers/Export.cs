using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using GAPPLE.Shared.Helpers;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Globalization;
using System.Linq.Dynamic.Core;
using System.Reflection;
using System.Text;

namespace GAPPLE.Server.Helpers
{
    internal class Export
    {
        #region Helpers
        public IQueryable ApplyQuery<T>(IQueryable items, IQueryCollection query = null) where T : class
        {
            if (query != null)
            {
                if (query.ContainsKey("$expand"))
                {
                    var propertiesToExpand = query["$expand"].ToString().Split(',');
                    foreach (var p in propertiesToExpand)
                    {
                        items = items.ToDynamicList().Append(p).AsQueryable();
                    }
                }

                var filter = query.ContainsKey("$filter") ? query["$filter"].ToString() : null;
                if (!string.IsNullOrEmpty(filter))
                {
                    items = items.Where(filter);
                }

                if (query.ContainsKey("$orderBy"))
                {
                    items = items.OrderBy(query["$orderBy"].ToString());
                }

                if (query.ContainsKey("$skip"))
                {
                    items = items.Skip(int.Parse(query["$skip"].ToString()));
                }

                if (query.ContainsKey("$top"))
                {
                    items = items.Take(int.Parse(query["$top"].ToString()));
                }

                if (query.ContainsKey("$select"))
                {
                    return items.Select($"new ({query["$select"]})");
                }
            }

            return items;
        }

        private string GetExcelColumnName(int columnNumber)
        {
            string columnName = "";
            while (columnNumber > 0)
            {
                columnNumber--;
                columnName = (char)('A' + columnNumber % 26) + columnName;
                columnNumber /= 26;
            }
            return columnName;
        }

        public object GetValue(object target, string name)
        {
            return target.GetType().GetProperty(name).GetValue(target);
        }

        public IEnumerable<KeyValuePair<string, Type>> GetProperties(Type type)
        {
            return type.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                    .Where(p => p.CanRead && IsSimpleType(p.PropertyType)).Select(p => new KeyValuePair<string, Type>(p.Name, p.PropertyType));
        }

        public bool IsSimpleType(Type type)
        {
            var underlyingType = type.IsGenericType &&
                type.GetGenericTypeDefinition() == typeof(Nullable<>) ?
                Nullable.GetUnderlyingType(type) : type;

            if (underlyingType == typeof(Guid))
                return true;

            var typeCode = Type.GetTypeCode(underlyingType);

            return typeCode switch
            {
                TypeCode.Boolean or TypeCode.Byte or TypeCode.Char or TypeCode.DateTime or TypeCode.Decimal or TypeCode.Double or TypeCode.Int16 or TypeCode.Int32 or TypeCode.Int64 or TypeCode.SByte or TypeCode.Single or TypeCode.String or TypeCode.UInt16 or TypeCode.UInt32 or TypeCode.UInt64 => true,
                _ => false,
            };
        }

        private bool IsNumeric(TypeCode typeCode)
        {
            return typeCode switch
            {
                TypeCode.Decimal or TypeCode.Double or TypeCode.Int16 or TypeCode.Int32 or TypeCode.Int64 or TypeCode.UInt16 or TypeCode.UInt32 or TypeCode.UInt64 => true,
                _ => false,
            };
        }

        private void GenerateWorkbookStylesPartContent(WorkbookStylesPart workbookStylesPart1)
        {
            Stylesheet stylesheet1 = new() { MCAttributes = new MarkupCompatibilityAttributes() { Ignorable = "x14ac x16r2 xr" } };
            stylesheet1.AddNamespaceDeclaration("mc", "http://schemas.openxmlformats.org/markup-compatibility/2006");
            stylesheet1.AddNamespaceDeclaration("x14ac", "http://schemas.microsoft.com/office/spreadsheetml/2009/9/ac");
            stylesheet1.AddNamespaceDeclaration("x16r2", "http://schemas.microsoft.com/office/spreadsheetml/2015/02/main");
            stylesheet1.AddNamespaceDeclaration("xr", "http://schemas.microsoft.com/office/spreadsheetml/2014/revision");

            Fonts fonts1 = new() { Count = (UInt32Value)1U, KnownFonts = true };

            Font font1 = new();
            FontSize fontSize1 = new() { Val = 11D };
            Color color1 = new() { Theme = (UInt32Value)1U };
            FontName fontName1 = new() { Val = "Calibri" };
            FontFamilyNumbering fontFamilyNumbering1 = new() { Val = 2 };
            FontScheme fontScheme1 = new() { Val = FontSchemeValues.Minor };

            font1.Append(fontSize1);
            font1.Append(color1);
            font1.Append(fontName1);
            font1.Append(fontFamilyNumbering1);
            font1.Append(fontScheme1);

            fonts1.Append(font1);

            Fills fills1 = new() { Count = (UInt32Value)2U };

            Fill fill1 = new();
            PatternFill patternFill1 = new() { PatternType = PatternValues.None };

            fill1.Append(patternFill1);

            Fill fill2 = new();
            PatternFill patternFill2 = new() { PatternType = PatternValues.Gray125 };

            fill2.Append(patternFill2);

            fills1.Append(fill1);
            fills1.Append(fill2);

            Borders borders1 = new() { Count = (UInt32Value)1U };

            Border border1 = new();
            LeftBorder leftBorder1 = new();
            RightBorder rightBorder1 = new();
            TopBorder topBorder1 = new();
            BottomBorder bottomBorder1 = new();
            DiagonalBorder diagonalBorder1 = new();

            border1.Append(leftBorder1);
            border1.Append(rightBorder1);
            border1.Append(topBorder1);
            border1.Append(bottomBorder1);
            border1.Append(diagonalBorder1);

            borders1.Append(border1);

            CellStyleFormats cellStyleFormats1 = new() { Count = (UInt32Value)1U };
            CellFormat cellFormat1 = new() { NumberFormatId = (UInt32Value)0U, FontId = (UInt32Value)0U, FillId = (UInt32Value)0U, BorderId = (UInt32Value)0U };
            cellStyleFormats1.Append(cellFormat1);

            CellFormats cellFormats1 = new() { Count = (UInt32Value)2U };
            CellFormat cellFormat2 = new() { NumberFormatId = (UInt32Value)0U, FontId = (UInt32Value)0U, FillId = (UInt32Value)0U, BorderId = (UInt32Value)0U, FormatId = (UInt32Value)0U };
            CellFormat cellFormat3 = new() { NumberFormatId = (UInt32Value)14U, FontId = (UInt32Value)0U, FillId = (UInt32Value)0U, BorderId = (UInt32Value)0U, FormatId = (UInt32Value)0U, ApplyNumberFormat = true };
            cellFormats1.Append(cellFormat2);
            cellFormats1.Append(cellFormat3);

            CellStyles cellStyles1 = new() { Count = (UInt32Value)1U };
            CellStyle cellStyle1 = new() { Name = "Normal", FormatId = (UInt32Value)0U, BuiltinId = (UInt32Value)0U };
            cellStyles1.Append(cellStyle1);
            DifferentialFormats differentialFormats1 = new() { Count = (UInt32Value)0U };
            TableStyles tableStyles1 = new() { Count = (UInt32Value)0U, DefaultTableStyle = "TableStyleMedium2", DefaultPivotStyle = "PivotStyleLight16" };

            StylesheetExtensionList stylesheetExtensionList1 = new();

            StylesheetExtension stylesheetExtension1 = new() { Uri = "{EB79DEF2-80B8-43e5-95BD-54CBDDF9020C}" };
            stylesheetExtension1.AddNamespaceDeclaration("x14", "http://schemas.microsoft.com/office/spreadsheetml/2009/9/main");

            StylesheetExtension stylesheetExtension2 = new() { Uri = "{9260A510-F301-46a8-8635-F512D64BE5F5}" };
            stylesheetExtension2.AddNamespaceDeclaration("x15", "http://schemas.microsoft.com/office/spreadsheetml/2010/11/main");

            OpenXmlUnknownElement openXmlUnknownElement4 = workbookStylesPart1.CreateUnknownElement("<x15:timelineStyles defaultTimelineStyle=\"TimeSlicerStyleLight1\" xmlns:x15=\"http://schemas.microsoft.com/office/spreadsheetml/2010/11/main\" />");
            stylesheetExtension2.Append(openXmlUnknownElement4);

            stylesheetExtensionList1.Append(stylesheetExtension1);
            stylesheetExtensionList1.Append(stylesheetExtension2);

            stylesheet1.Append(fonts1);
            stylesheet1.Append(fills1);
            stylesheet1.Append(borders1);
            stylesheet1.Append(cellStyleFormats1);
            stylesheet1.Append(cellFormats1);
            stylesheet1.Append(cellStyles1);
            stylesheet1.Append(differentialFormats1);
            stylesheet1.Append(tableStyles1);
            stylesheet1.Append(stylesheetExtensionList1);

            workbookStylesPart1.Stylesheet = stylesheet1;
        }
        #endregion

        #region ToCSV
        public FileStreamResult ToCSV(IQueryable query, string fileName = null)
        {
            var columns = GetProperties(query.ElementType);

            var sb = new StringBuilder();

            foreach (var item in query)
            {
                var row = new List<string>();

                foreach (var column in columns)
                {
                    row.Add($"{GetValue(item, column.Key)}".Trim());
                }

                sb.AppendLine(string.Join(",", [.. row]));
            }

            var result = new FileStreamResult(new MemoryStream(Encoding.Default.GetBytes($"{string.Join(",", columns.Select(c => c.Key))}{Environment.NewLine}{sb}")), "text/csv")
            {
                FileDownloadName = (!string.IsNullOrEmpty(fileName) ? fileName : "Export") + ".csv"
            };

            return result;
        }
        #endregion

        #region ToExcel
        public FileStreamResult ToExcel(IQueryable query, string fileName = null)
        {
            var columns = GetProperties(query.ElementType);
            var stream = new MemoryStream();

            using (var document = SpreadsheetDocument.Create(stream, SpreadsheetDocumentType.Workbook))
            {
                var workbookPart = document.AddWorkbookPart();
                workbookPart.Workbook = new Workbook();

                var worksheetPart = workbookPart.AddNewPart<WorksheetPart>();
                worksheetPart.Worksheet = new Worksheet();

                var workbookStylesPart = workbookPart.AddNewPart<WorkbookStylesPart>();
                GenerateWorkbookStylesPartContent(workbookStylesPart);

                var sheets = workbookPart.Workbook.AppendChild(new Sheets());
                var sheet = new Sheet() { Id = workbookPart.GetIdOfPart(worksheetPart), SheetId = 1, Name = "Sheet1" };
                sheets.Append(sheet);

                workbookPart.Workbook.Save();

                var sheetData = worksheetPart.Worksheet.AppendChild(new SheetData());
                var headerRow = new Row();

                foreach (var column in columns)
                {
                    headerRow.Append(new Cell()
                    {
                        CellValue = new CellValue(column.Key),
                        DataType = new EnumValue<CellValues>(CellValues.String)
                    });
                }

                sheetData.AppendChild(headerRow);

                foreach (var item in query)
                {
                    var row = new Row();

                    foreach (var column in columns)
                    {
                        var value = GetValue(item, column.Key);
                        var stringValue = $"{value}".Trim();

                        var cell = new Cell();

                        var underlyingType = column.Value.IsGenericType &&
                            column.Value.GetGenericTypeDefinition() == typeof(Nullable<>) ?
                            Nullable.GetUnderlyingType(column.Value) : column.Value;

                        var typeCode = Type.GetTypeCode(underlyingType);

                        if (typeCode == TypeCode.DateTime)
                        {
                            if (!string.IsNullOrWhiteSpace(stringValue))
                            {
                                cell.CellValue = new CellValue() { Text = ((DateTime)value).ToOADate().ToString(CultureInfo.InvariantCulture) };
                                cell.DataType = new EnumValue<CellValues>(CellValues.Number);
                                cell.StyleIndex = (UInt32Value)1U;
                            }
                        }
                        else if (typeCode == TypeCode.Boolean)
                        {
                            cell.CellValue = new CellValue(stringValue.ToLower());
                            cell.DataType = new EnumValue<CellValues>(CellValues.Boolean);
                        }
                        else if (IsNumeric(typeCode))
                        {
                            if (value != null)
                            {
                                stringValue = Convert.ToString(value, CultureInfo.InvariantCulture);
                            }
                            cell.CellValue = new CellValue(stringValue);
                            cell.DataType = new EnumValue<CellValues>(CellValues.Number);
                        }
                        else
                        {
                            cell.CellValue = new CellValue(stringValue);
                            cell.DataType = new EnumValue<CellValues>(CellValues.String);
                        }

                        row.Append(cell);
                    }

                    sheetData.AppendChild(row);
                }

                workbookPart.Workbook.Save();
            }

            if (stream?.Length > 0)
            {
                stream.Seek(0, SeekOrigin.Begin);
            }

            var result = new FileStreamResult(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
            {
                FileDownloadName = (!string.IsNullOrEmpty(fileName) ? fileName : "Export") + ".xlsx"
            };

            return result;
        }

        public FileStreamResult ToExcel(IEnumerable<Dictionary<string, string>> data, Dictionary<string, Type> headers)
        {
            var stream = new MemoryStream();

            using (var document = SpreadsheetDocument.Create(stream, SpreadsheetDocumentType.Workbook))
            {
                var workbookPart = document.AddWorkbookPart();
                workbookPart.Workbook = new Workbook();

                var worksheetPart = workbookPart.AddNewPart<WorksheetPart>();
                worksheetPart.Worksheet = new Worksheet();

                var workbookStylesPart = workbookPart.AddNewPart<WorkbookStylesPart>();
                GenerateWorkbookStylesPartContent(workbookStylesPart);

                var sheets = workbookPart.Workbook.AppendChild(new Sheets());
                var sheet = new Sheet() { Id = workbookPart.GetIdOfPart(worksheetPart), SheetId = 1, Name = "Sheet1" };
                sheets.Append(sheet);

                workbookPart.Workbook.Save();

                var sheetData = worksheetPart.Worksheet.AppendChild(new SheetData());
                var headerRow = new Row();

                foreach (var column in headers)
                {
                    headerRow.Append(new Cell()
                    {
                        CellValue = new CellValue(column.Key),
                        DataType = new EnumValue<CellValues>(CellValues.String)
                    });
                }

                sheetData.AppendChild(headerRow);
                foreach (var dict in data)
                {
                    var row = new Row();
                    foreach (var column in headers)
                    {
                        var value = dict[column.Key];
                        var stringValue = $"{value}".Trim();

                        var cell = new Cell();

                        var underlyingType = column.Value.IsGenericType &&
                            column.Value.GetGenericTypeDefinition() == typeof(Nullable<>) ?
                            Nullable.GetUnderlyingType(column.Value) : column.Value;

                        var typeCode = Type.GetTypeCode(underlyingType);

                        if (typeCode == TypeCode.DateTime)
                        {
                            if (!string.IsNullOrWhiteSpace(stringValue))
                            {
                                cell.CellValue = new CellValue() { Text = DateTime.Parse(value).ToOADate().ToString(CultureInfo.InvariantCulture) };
                                cell.DataType = new EnumValue<CellValues>(CellValues.Number);
                                cell.StyleIndex = (UInt32Value)1U;
                            }
                        }
                        else if (typeCode == TypeCode.Boolean)
                        {
                            cell.CellValue = new CellValue(stringValue.ToLower());
                            cell.DataType = new EnumValue<CellValues>(CellValues.Boolean);
                        }
                        else if (IsNumeric(typeCode))
                        {
                            if (value != null)
                            {
                                stringValue = Convert.ToString(value, CultureInfo.InvariantCulture);
                            }
                            cell.CellValue = new CellValue(stringValue);
                            cell.DataType = new EnumValue<CellValues>(CellValues.Number);
                        }
                        else
                        {
                            cell.CellValue = new CellValue(stringValue);
                            cell.DataType = new EnumValue<CellValues>(CellValues.String);
                        }

                        row.Append(cell);
                    }
                    sheetData.AppendChild(row);
                }

                workbookPart.Workbook.Save();
            }

            if (stream?.Length > 0)
            {
                stream.Seek(0, SeekOrigin.Begin);
            }

            var result = new FileStreamResult(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
            {
                FileDownloadName = "Export.xlsx"
            };

            return result;
        }

        public FileStreamResult ToExcel<T>(List<T> data)
        {
            var stream = new MemoryStream();
            using (var document = SpreadsheetDocument.Create(stream, SpreadsheetDocumentType.Workbook))
            {
                // Crear el workbook y la hoja
                var workbookPart = document.AddWorkbookPart();
                workbookPart.Workbook = new Workbook();

                var worksheetPart = workbookPart.AddNewPart<WorksheetPart>();
                worksheetPart.Worksheet = new Worksheet(new SheetData());

                var sheets = document.WorkbookPart.Workbook.AppendChild(new Sheets());
                sheets.Append(new Sheet() { Id = document.WorkbookPart.GetIdOfPart(worksheetPart), SheetId = 1, Name = "Sheet1" });

                var sheetData = worksheetPart.Worksheet.GetFirstChild<SheetData>();

                var properties = typeof(T).GetProperties()
                    .Where(p => p.GetCustomAttribute<DisplayAttribute>() != null)
                    .ToList();

                var sumColumns = new Dictionary<int, string>();

                var headerRow = new Row();
                foreach (var prop in properties)
                {
                    var displayAttribute = prop.GetCustomAttribute<DisplayAttribute>();
                    string headerText = displayAttribute?.Name ?? prop.Name;

                    var cell = new Cell
                    {
                        DataType = CellValues.String,
                        CellValue = new CellValue(headerText)
                    };
                    headerRow.AppendChild(cell);
                }
                sheetData.AppendChild(headerRow);

                int rowIndex = 2;
                foreach (var item in data)
                {
                    var dataRow = new Row();
                    int colIndex = 0;
                    foreach (var prop in properties)
                    {
                        var cellValue = prop.GetValue(item)?.ToString() ?? string.Empty;
                        var formatAttribute = prop.GetCustomAttribute<ExcelFormat>();
                        var sumAttribute = prop.GetCustomAttribute<ExcelSum>();

                        var cell = new Cell();
                        string cellReference = GetExcelColumnName(colIndex + 1) + rowIndex;

                        if (formatAttribute != null)
                        {
                            switch (formatAttribute.Format)
                            {
                                case ExcelFormats.Number:
                                    cell.DataType = CellValues.Number;
                                    cell.CellValue = new CellValue(cellValue?.ToString() ?? "0");
                                    break;
                                case ExcelFormats.Currency:
                                    cell.DataType = CellValues.Number;
                                    cell.CellValue = new CellValue(cellValue?.ToString() ?? "0");
                                    break;
                                case ExcelFormats.OnlyDate:
                                    cell.DataType = CellValues.String;
                                    cell.CellValue = new CellValue(DateTime.Parse(cellValue).ToString("dd/MM/yyyy"));
                                    break;
                                case ExcelFormats.FullDate:
                                    cell.DataType = CellValues.String;
                                    cell.CellValue = new CellValue(DateTime.Parse(cellValue).ToString("dd/MM/yyyy HH:mm"));
                                    break;
                                default:
                                    cell.DataType = CellValues.String;
                                    cell.CellValue = new CellValue(cellValue?.ToString() ?? "");
                                    break;
                            }
                        }
                        else
                        {
                            cell.DataType = CellValues.String;
                            cell.CellValue = new CellValue(cellValue?.ToString() ?? "");
                        }

                        if (sumAttribute != null)
                            sumColumns[colIndex] = GetExcelColumnName(colIndex + 1);

                        dataRow.AppendChild(cell);
                        colIndex++;
                    }
                    sheetData.AppendChild(dataRow);
                    rowIndex++;
                }
                if (sumColumns.Count != 0)
                {
                    var sumRow = new Row();
                    for (int colIndex = 0; colIndex < properties.Count; colIndex++)
                    {
                        var cell = new Cell();
                        if (sumColumns.TryGetValue(colIndex, out string columnLetter))
                        {
                            string sumFormula = $"SUM({columnLetter}2:{columnLetter}{rowIndex - 1})";
                            cell.CellFormula = new CellFormula(sumFormula);
                            cell.DataType = CellValues.Number;
                        }
                        else
                        {
                            cell.DataType = CellValues.String;
                            cell.CellValue = new CellValue("");
                        }
                        sumRow.AppendChild(cell);
                    }
                    sheetData.AppendChild(sumRow);
                }
            }

            if (stream?.Length > 0)
            {
                stream.Seek(0, SeekOrigin.Begin);
            }

            var result = new FileStreamResult(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
            {
                FileDownloadName = "Export.xlsx"
            };

            return result;
        }

        public FileStreamResult ToExcel(DataTable table)
        {
            if (table == null || table.Columns.Count == 0)
                throw new ArgumentException("El campo que admite valores null debe contener un valor no null");

            var stream = new MemoryStream();

            using (var document = SpreadsheetDocument.Create(stream, SpreadsheetDocumentType.Workbook))
            {
                var workbookPart = document.AddWorkbookPart();
                workbookPart.Workbook = new Workbook();

                var worksheetPart = workbookPart.AddNewPart<WorksheetPart>();
                worksheetPart.Worksheet = new Worksheet(new SheetData());

                var sheets = workbookPart.Workbook.AppendChild(new Sheets());
                sheets.Append(new Sheet
                {
                    Id = workbookPart.GetIdOfPart(worksheetPart),
                    SheetId = 1,
                    Name = "Sheet1"
                });

                var sheetData = worksheetPart.Worksheet.GetFirstChild<SheetData>();

                // HEADER
                var headerRow = new Row();
                foreach (DataColumn column in table.Columns)
                {
                    headerRow.AppendChild(new Cell
                    {
                        DataType = CellValues.String,
                        CellValue = new CellValue(column.ColumnName)
                    });
                }
                sheetData.AppendChild(headerRow);

                // DATA
                foreach (DataRow row in table.Rows)
                {
                    var dataRow = new Row();

                    foreach (var value in row.ItemArray)
                    {
                        var cell = new Cell();

                        if (value == DBNull.Value)
                        {
                            cell.DataType = CellValues.String;
                            cell.CellValue = new CellValue(string.Empty);
                        }
                        else if (value is int or long or decimal or double or float)
                        {
                            cell.DataType = CellValues.Number;
                            cell.CellValue = new CellValue(
                                Convert.ToString(value, CultureInfo.InvariantCulture)
                            );
                        }
                        else if (value is DateTime date)
                        {
                            cell.DataType = CellValues.String;
                            cell.CellValue = new CellValue(date.ToString("dd/MM/yyyy HH:mm"));
                        }
                        else
                        {
                            cell.DataType = CellValues.String;
                            cell.CellValue = new CellValue(value.ToString());
                        }

                        dataRow.AppendChild(cell);
                    }

                    sheetData.AppendChild(dataRow);
                }
            }

            stream.Seek(0, SeekOrigin.Begin);

            return new FileStreamResult(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
            {
                FileDownloadName = "Export.xlsx"
            };
        }
        #endregion

        #region ToExcelCustoms
        public FileStreamResult ToExcelCustomHeaders(IQueryable query, List<string> headers, string fileName = null)
        {
            var columns = GetProperties(query.ElementType);
            var stream = new MemoryStream();

            using (var document = SpreadsheetDocument.Create(stream, SpreadsheetDocumentType.Workbook))
            {
                var workbookPart = document.AddWorkbookPart();
                workbookPart.Workbook = new Workbook();

                var worksheetPart = workbookPart.AddNewPart<WorksheetPart>();
                worksheetPart.Worksheet = new Worksheet();

                var workbookStylesPart = workbookPart.AddNewPart<WorkbookStylesPart>();
                GenerateWorkbookStylesPartContent(workbookStylesPart);

                var sheets = workbookPart.Workbook.AppendChild(new Sheets());
                var sheet = new Sheet() { Id = workbookPart.GetIdOfPart(worksheetPart), SheetId = 1, Name = "Sheet1" };
                sheets.Append(sheet);

                workbookPart.Workbook.Save();

                var sheetData = worksheetPart.Worksheet.AppendChild(new SheetData());
                var headerRow = new Row();

                foreach (var header in headers)
                {
                    headerRow.Append(new Cell()
                    {
                        CellValue = new CellValue(header),
                        DataType = new EnumValue<CellValues>(CellValues.String)
                    });
                }

                sheetData.AppendChild(headerRow);

                foreach (var item in query)
                {
                    var row = new Row();

                    foreach (var column in columns)
                    {
                        var value = GetValue(item, column.Key);
                        var stringValue = $"{value}".Trim();

                        var cell = new Cell();

                        var underlyingType = column.Value.IsGenericType &&
                            column.Value.GetGenericTypeDefinition() == typeof(Nullable<>) ?
                            Nullable.GetUnderlyingType(column.Value) : column.Value;

                        var typeCode = Type.GetTypeCode(underlyingType);

                        if (typeCode == TypeCode.DateTime)
                        {
                            if (!string.IsNullOrWhiteSpace(stringValue))
                            {
                                cell.CellValue = new CellValue() { Text = ((DateTime)value).ToOADate().ToString(CultureInfo.InvariantCulture) };
                                cell.DataType = new EnumValue<CellValues>(CellValues.Number);
                                cell.StyleIndex = (UInt32Value)1U;
                            }
                        }
                        else if (typeCode == TypeCode.Boolean)
                        {
                            cell.CellValue = new CellValue(stringValue.ToLower());
                            cell.DataType = new EnumValue<CellValues>(CellValues.Boolean);
                        }
                        else if (IsNumeric(typeCode))
                        {
                            if (value != null)
                            {
                                stringValue = Convert.ToString(value, CultureInfo.InvariantCulture);
                            }
                            cell.CellValue = new CellValue(stringValue);
                            cell.DataType = new EnumValue<CellValues>(CellValues.Number);
                        }
                        else
                        {
                            cell.CellValue = new CellValue(stringValue);
                            cell.DataType = new EnumValue<CellValues>(CellValues.String);
                        }

                        row.Append(cell);
                    }

                    sheetData.AppendChild(row);
                }

                workbookPart.Workbook.Save();
            }

            if (stream?.Length > 0)
            {
                stream.Seek(0, SeekOrigin.Begin);
            }

            var result = new FileStreamResult(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
            {
                FileDownloadName = (!string.IsNullOrEmpty(fileName) ? fileName : "Export") + ".xlsx"
            };

            return result;
        }

        public FileStreamResult ToExcelHeaders(List<string> headers, string fileName = null)
        {
            var stream = new MemoryStream();

            using (var document = SpreadsheetDocument.Create(stream, SpreadsheetDocumentType.Workbook))
            {
                var workbookPart = document.AddWorkbookPart();
                workbookPart.Workbook = new Workbook();

                var worksheetPart = workbookPart.AddNewPart<WorksheetPart>();
                worksheetPart.Worksheet = new Worksheet();

                var workbookStylesPart = workbookPart.AddNewPart<WorkbookStylesPart>();
                GenerateWorkbookStylesPartContent(workbookStylesPart);

                var sheets = workbookPart.Workbook.AppendChild(new Sheets());
                var sheet = new Sheet()
                {
                    Id = workbookPart.GetIdOfPart(worksheetPart),
                    SheetId = 1,
                    Name = "Sheet1"
                };
                sheets.Append(sheet);

                workbookPart.Workbook.Save();

                var sheetData = worksheetPart.Worksheet.AppendChild(new SheetData());
                var headerRow = new Row();

                foreach (var header in headers)
                {
                    headerRow.Append(new Cell()
                    {
                        CellValue = new CellValue(header),
                        DataType = new EnumValue<CellValues>(CellValues.String)
                    });
                }

                sheetData.AppendChild(headerRow);

                workbookPart.Workbook.Save();
            }

            if (stream?.Length > 0)
                stream.Seek(0, SeekOrigin.Begin);

            var result = new FileStreamResult(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
            {
                FileDownloadName = (!string.IsNullOrEmpty(fileName) ? fileName : "Export") + ".xlsx"
            };

            return result;
        }

        public FileStreamResult ExportExcelWithDetail<TMaster, TDetail>(List<TMaster> masterData, Func<TMaster, List<TDetail>> getDetailsFunc, string fileName)
        {
            var stream = new MemoryStream();
            using (var document = SpreadsheetDocument.Create(stream, SpreadsheetDocumentType.Workbook))
            {
                var workbookPart = document.AddWorkbookPart();
                workbookPart.Workbook = new Workbook();

                var worksheetPart = workbookPart.AddNewPart<WorksheetPart>();
                worksheetPart.Worksheet = new Worksheet(new SheetData());
                var sheetData = worksheetPart.Worksheet.GetFirstChild<SheetData>();

                var sheets = workbookPart.Workbook.AppendChild(new Sheets());
                sheets.Append(new Sheet()
                {
                    Id = workbookPart.GetIdOfPart(worksheetPart),
                    SheetId = 1,
                    Name = $"{fileName} {DateTime.Now:dd-MM-yyyy}"
                });

                int currentRow = 1;

                var masterProperties = typeof(TMaster).GetProperties()
                    .Where(p => p.GetCustomAttribute<DisplayAttribute>() != null)
                    .ToList();

                var detailProperties = typeof(TDetail).GetProperties()
                    .Where(p => p.GetCustomAttribute<DisplayAttribute>() != null)
                    .ToList();

                //headers master
                var headerRow = new Row() { RowIndex = (uint)currentRow++ };
                foreach (var prop in masterProperties)
                {
                    var displayAttribute = prop.GetCustomAttribute<DisplayAttribute>();
                    string headerText = displayAttribute?.Name ?? prop.Name;

                    var cell = new Cell()
                    {
                        DataType = CellValues.String,
                        CellValue = new CellValue(headerText)
                    };
                    headerRow.AppendChild(cell);
                }

                var nonEmptyDetailProperties = new List<PropertyInfo>();
                foreach (var prop in detailProperties)
                {
                    bool hasData = masterData.Any(m => getDetailsFunc(m).Any(d => prop.GetValue(d) != null));
                    if (hasData)
                    {
                        nonEmptyDetailProperties.Add(prop);
                        var displayAttribute = prop.GetCustomAttribute<DisplayAttribute>();
                        string headerText = displayAttribute?.Name ?? prop.Name;

                        var cell = new Cell()
                        {
                            DataType = CellValues.String,
                            CellValue = new CellValue(headerText)
                        };
                        headerRow.AppendChild(cell);
                    }
                }

                sheetData.AppendChild(headerRow);

                foreach (var masterItem in masterData)
                {
                    var details = getDetailsFunc(masterItem);

                    foreach (var detailItem in details)
                    {
                        var row = new Row() { RowIndex = (uint)currentRow++ };

                        foreach (var prop in masterProperties)
                        {
                            var cellValue = prop.GetValue(masterItem)?.ToString() ?? string.Empty;
                            row.AppendChild(new Cell()
                            {
                                DataType = CellValues.String,
                                CellValue = new CellValue(cellValue)
                            });
                        }

                        foreach (var prop in nonEmptyDetailProperties)
                        {
                            var cellValue = prop.GetValue(detailItem)?.ToString() ?? string.Empty;
                            row.AppendChild(new Cell()
                            {
                                DataType = CellValues.String,
                                CellValue = new CellValue(cellValue)
                            });
                        }

                        sheetData.AppendChild(row);
                    }
                }

                worksheetPart.Worksheet.Save();
                workbookPart.Workbook.Save();
            }

            if (stream?.Length > 0)
            {
                stream.Seek(0, SeekOrigin.Begin);
            }

            return new FileStreamResult(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
            {
                FileDownloadName = $"{fileName} {DateTime.Now:dd-MM-yyyy}.xlsx"
            };
        }

        public FileStreamResult ToExcel(DataTable table, string exceptColumns)
        {
            if (table == null || table.Columns.Count == 0)
                throw new ArgumentException("El campo que admite valores null debe contener un valor no null");

            // 1) Columnas excluidas por parámetro
            var excluded = string.IsNullOrWhiteSpace(exceptColumns)
                ? new HashSet<string>()
                : exceptColumns
                    .Split('|', StringSplitOptions.RemoveEmptyEntries)
                    .Select(x => x.Trim())
                    .ToHashSet(StringComparer.OrdinalIgnoreCase);

            // 2) Columnas finales a exportar
            var columnsToExport = table.Columns
                .Cast<DataColumn>()
                .Where(col =>
                    !excluded.Contains(col.ColumnName) &&          // no está en exceptColumns
                    table.AsEnumerable().Any(r => r[col] != DBNull.Value) // no es toda NULL
                )
                .ToList();

            var stream = new MemoryStream();

            using (var document = SpreadsheetDocument.Create(stream, SpreadsheetDocumentType.Workbook))
            {
                var workbookPart = document.AddWorkbookPart();
                workbookPart.Workbook = new Workbook();

                var worksheetPart = workbookPart.AddNewPart<WorksheetPart>();
                worksheetPart.Worksheet = new Worksheet(new SheetData());

                var sheets = workbookPart.Workbook.AppendChild(new Sheets());
                sheets.Append(new Sheet
                {
                    Id = workbookPart.GetIdOfPart(worksheetPart),
                    SheetId = 1,
                    Name = "Sheet1"
                });

                var sheetData = worksheetPart.Worksheet.GetFirstChild<SheetData>();

                // HEADER
                var headerRow = new Row();
                foreach (var column in columnsToExport)
                {
                    headerRow.AppendChild(new Cell
                    {
                        DataType = CellValues.String,
                        CellValue = new CellValue(column.ColumnName)
                    });
                }
                sheetData.AppendChild(headerRow);

                // DATA
                foreach (DataRow row in table.Rows)
                {
                    var dataRow = new Row();

                    foreach (var column in columnsToExport)
                    {
                        var value = row[column];
                        var cell = new Cell();

                        if (value == DBNull.Value)
                        {
                            cell.DataType = CellValues.String;
                            cell.CellValue = new CellValue(string.Empty);
                        }
                        else if (value is int or long or decimal or double or float)
                        {
                            cell.DataType = CellValues.Number;
                            cell.CellValue = new CellValue(
                                Convert.ToString(value, CultureInfo.InvariantCulture)
                            );
                        }
                        else if (value is DateTime date)
                        {
                            cell.DataType = CellValues.String;
                            cell.CellValue = new CellValue(date.ToString("dd/MM/yyyy HH:mm"));
                        }
                        else
                        {
                            cell.DataType = CellValues.String;
                            cell.CellValue = new CellValue(value.ToString());
                        }

                        dataRow.AppendChild(cell);
                    }

                    sheetData.AppendChild(dataRow);
                }
            }

            stream.Seek(0, SeekOrigin.Begin);

            return new FileStreamResult(stream,
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
            {
                FileDownloadName = "Export.xlsx"
            };
        }
        #endregion

    }
}
