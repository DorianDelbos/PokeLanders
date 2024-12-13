using System;
using UnityEngine;

namespace dgames.Utilities
{
    public static class ColorUtility
    {
        public static Color ToColor(this string hex)
        {
            if (!hex.StartsWith("#") || (hex.Length != 7 && hex.Length != 9))
                throw new ArgumentException("This string don't seem to be an hex value.");

            hex = hex.Substring(1);

            byte r = byte.Parse(hex.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
            byte g = byte.Parse(hex.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
            byte b = byte.Parse(hex.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);
            byte a = (hex.Length == 8) ? byte.Parse(hex.Substring(6, 2), System.Globalization.NumberStyles.HexNumber) : (byte)255;

            return new Color(r / 255f, g / 255f, b / 255f, a / 255f);
        }

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
