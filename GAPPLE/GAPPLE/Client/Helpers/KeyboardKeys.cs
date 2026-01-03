namespace GAPPLE.Client.Helpers
{
    public static class KeyboardKeys
    {
        public static string Up => "ArrowUp";
        public static string Down => "ArrowDown";
        public static string Right => "ArrowRight";
        public static string Left => "ArrowLeft";
        public static string Enter => "Enter";
        public static string NumpadEnter => "NumpadEnter";
        public static string PageUp => "PageUp";
        public static string PageDown => "PageDown";
        public static string Escape => "Escape";
        public static string Dot => ".";
        public static string Comma => ",";
        public static List<string> Numbers => ["0", "1", "2", "3", "4", "5", "6", "7", "8", "9"];
        public static List<string> Arrows => [Up, Down, Right, Left];
        public static string Backspace => "Backspace";
        public static string Shift => "Shift";
        public static string Tab => "Tab";
        public static string Ctrl => "Control";
        public static List<string> Special => [Ctrl, "c", "v", Tab, Backspace, Shift];
        public static string Space => "Space";
    }
}
