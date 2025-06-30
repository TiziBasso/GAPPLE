using DocumentFormat.OpenXml.Spreadsheet;
using System.Text.RegularExpressions;

namespace GAPPLE.Server.Tools
{
    internal class Functions
    {
        internal static string EliminarSaltosDeLinea(string text)
        {
            if (text.Contains((char)10))
                text = text.Replace((char)10, char.Parse(string.Empty));
            if (text.Contains((char)13))
                text = text.Replace((char)13, char.Parse(string.Empty));
            return text;
        }

        internal static string StringEnter(int cantidad)
        {
            string ret = null;
            for (int n = 1; n <= cantidad; n++)
            {
                ret += Environment.NewLine;
            }
            return ret;
        }

        internal static string ReemplazarTildes(string str)
        {
            return str.ToLower().Replace("á", "a").Replace("é", "e").Replace("í", "i").Replace("ó", "o").Replace("ú", "u");
        }

        internal static string RemoverCaracteresEspeciales(string str)
        {
            return Regex.Replace(str, @"[^a-zA-Z0-9]", "");
        }


        internal static Cell CreateCell(string value)
        {
            return new Cell()
            {
                DataType = CellValues.String,
                CellValue = new CellValue(value)
            };
        }

        internal static Cell CreateCell(int value)
        {
            return new Cell()
            {
                DataType = CellValues.Number,
                CellValue = new CellValue(value)
            };
        }

        internal static Cell CreateCell(long value)
        {
            return new Cell()
            {
                DataType = CellValues.Number,
                CellValue = new CellValue(value.ToString())
            };
        }
    }
}
