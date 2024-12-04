using UnityEngine;

namespace LandersLegends.Utilities
{
    public static class ColorUtilities
    {
        /// <summary>
        /// Converts a hexadecimal string to a Unity Color.
        /// </summary>
        /// <param name="hex">Hexadecimal color string in the format "#RRGGBB" or "#RRGGBBAA" or "RRGGBB" or "RRGGBBAA".</param>
        /// <returns>A Color object representing the color.</returns>
        public static Color ToColor(this string hex)
        {
            if (!hex.StartsWith("#") || (hex.Length != 7 && hex.Length != 9))
            {
                Debug.LogError("Invalid hex string length. Must be 6 or 8 characters.");
                return Color.black;
            }

            hex = hex.Substring(1);

            byte r = byte.Parse(hex.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
            byte g = byte.Parse(hex.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
            byte b = byte.Parse(hex.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);
            byte a = (hex.Length == 8) ? byte.Parse(hex.Substring(6, 2), System.Globalization.NumberStyles.HexNumber) : (byte)255;

            return new Color(r / 255f, g / 255f, b / 255f, a / 255f);
        }

        /// <summary>
        /// Converts a Unity Color to a hexadecimal string.
        /// </summary>
        /// <param name="color">The Color to convert.</param>
        /// <param name="includeAlpha">If true, includes the alpha channel in the hex string.</param>
        /// <returns>A hexadecimal string representing the color.</returns>
        public static string ToHex(this Color color, bool includeAlpha = true)
        {
            // Convert float (0-1) to byte (0-255) and format as a hex string
            int r = Mathf.Clamp((int)(color.r * 255), 0, 255);
            int g = Mathf.Clamp((int)(color.g * 255), 0, 255);
            int b = Mathf.Clamp((int)(color.b * 255), 0, 255);
            int a = Mathf.Clamp((int)(color.a * 255), 0, 255);

            return includeAlpha
                ? $"#{r:X2}{g:X2}{b:X2}{a:X2}"
                : $"#{r:X2}{g:X2}{b:X2}";
        }
    }
}
