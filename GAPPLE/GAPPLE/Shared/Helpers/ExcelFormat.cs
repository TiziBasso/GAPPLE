namespace GAPPLE.Shared.Helpers
{
    [AttributeUsage(AttributeTargets.Property)]
    public class ExcelFormat : Attribute
    {
        public ExcelFormats Format { get; }

        public ExcelFormat(ExcelFormats format)
        {
            Format = format;
        }
    }

    [AttributeUsage(AttributeTargets.Property)]
    public class ExcelSum : Attribute
    {
    }

    public enum ExcelFormats
    {
        General,
        Number,
        Currency,
        OnlyDate,
        FullDate,
        Boolean
    }
}
