using System;
using System.ComponentModel.DataAnnotations;

namespace GAPPLE.Shared.Helpers
{
    public class FilesHelper
    {
        [Flags]
        public enum Types
        {
            [Display(Description = "text/plain")]
            Text,
            [Display(Description = "spreadsheetml.sheet")]
            XLSX,
            [Display(Description = "text/csv")]
            CSV,
            None
        }

        public static Types GetTypeFromMime(string mimeType)
        {
            if (mimeType.EndsWith("spreadsheetml.sheet"))
                return Types.XLSX;
            else
                return mimeType switch
                {
                    "text/csv" => Types.CSV,
                    "text/plain" => Types.Text,
                    _ => Types.None
                };
        }
    }
}
