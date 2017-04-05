using System.Windows.Media;

namespace CodeGaze
{
    internal static class ColorExtensions
    {
        public static string Serialize(this Color c)
            => string.Format("#{0:X2}{1:X2}{2:X2}{3:X2}", c.A, c.R, c.G, c.B);

        public static Color DeserializeColor(this string s)
        {
            int color = int.Parse(s.Replace("#", ""), System.Globalization.NumberStyles.HexNumber);
            byte b = (byte)(color & 0x0000000FF);
            byte g = (byte)((color & 0x0000FF00) >> 8);
            byte r = (byte)((color & 0x00FF0000) >> 16);
            byte a = (byte)((color & 0xFF000000) >> 24);
            return Color.FromArgb(a, r, g, b);
        }

        public static System.Windows.Media.Color ToMediaColor(this System.Drawing.Color c)
        {
            return Color.FromArgb(c.A, c.R, c.G, c.B);
        }

        public static System.Drawing.Color ToDrawingColor(this Color c)
        {
            return System.Drawing.Color.FromArgb(c.A, c.R, c.G, c.B);
        }
    }
}
