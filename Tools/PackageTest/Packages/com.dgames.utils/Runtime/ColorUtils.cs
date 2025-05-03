using System;
using UnityEngine;

namespace dgames.Utils
{
    /// <summary>
    /// A utility class for working with colors, providing methods to convert between HEX and Color.
    /// </summary>
    public static class ColorUtils
    {
        /// <summary>
        /// Converts a HEX string to a Unity <see cref="Color"/>.
        /// </summary>
        /// <param name="hex">The HEX string (either 7 or 9 characters long, starting with '#').</param>
        /// <returns>A <see cref="Color"/> object corresponding to the HEX value.</returns>
        /// <exception cref="ArgumentException">Thrown if the provided string is not a valid HEX value.</exception>
        public static Color ToColor(this string hex)
        {
            if (!hex.StartsWith("#") || (hex.Length != 7 && hex.Length != 9))
                throw new ArgumentException("This string doesn't seem to be a valid hex value.");

            hex = hex.Substring(1);

            byte r = byte.Parse(hex.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
            byte g = byte.Parse(hex.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
            byte b = byte.Parse(hex.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);
            byte a = (hex.Length == 8) ? byte.Parse(hex.Substring(6, 2), System.Globalization.NumberStyles.HexNumber) : (byte)255;

            return new Color(r / 255f, g / 255f, b / 255f, a / 255f);
        }

        /// <summary>
        /// Converts a Unity <see cref="Color"/> to a HEX string.
        /// </summary>
        /// <param name="color">The <see cref="Color"/> to be converted.</param>
        /// <param name="includeAlpha">If <c>true</c>, the alpha channel will be included in the result. If <c>false</c>, alpha is excluded (default is true).</param>
        /// <returns>A HEX string representation of the color.</returns>
        public static string ToHex(this Color color, bool includeAlpha = true)
        {
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
