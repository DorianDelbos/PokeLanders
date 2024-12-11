using System.Globalization;

namespace dgames.Utilities
{
	public static class StringUtils
	{
		/// <summary>
		/// Convert a string (HEX) to byte.
		/// Exemple : "FF FF 00 A0" -> { 255, 255, 0, 160 }
		/// </summary>
		/// <param name="_string">The hex value to convert.</param>
		/// <returns>The byte value.</returns>
		public static byte[] ToByte(this string _string)
		{
			_string = _string.Replace(" ", "");
			byte[] bytes = new byte[_string.Length / 2];

			for (int i = 0; i < _string.Length; i += 2)
				bytes[i / 2] = byte.Parse(_string.Substring(i, 2), NumberStyles.HexNumber);

			return bytes;
		}
	}
}
