namespace dgames.Utils
{
    /// <summary>
    /// Provides utility methods for working with strings.
    /// </summary>
    public static class StringUtils
    {
        /// <summary>
        /// Converts a hexadecimal string (with optional spaces) into a byte array.
        /// </summary>
        /// <param name="_string">The hexadecimal string to convert. The string should consist of characters 0-9 and A-F (case-insensitive).</param>
        /// <returns>A byte array corresponding to the hexadecimal string.</returns>
        /// <remarks>
        /// Any spaces within the input string are ignored. The string must have an even length as each byte is represented by two hexadecimal characters.
        /// </remarks>
        public static byte[] ToByte(this string _string)
        {
            // Remove any spaces in the string.
            _string = _string.Replace(" ", "");

            // Initialize a byte array to store the result.
            byte[] bytes = new byte[_string.Length / 2];

            // Iterate over the string, converting pairs of hexadecimal characters into bytes.
            for (int i = 0; i < _string.Length; i += 2)
                bytes[i / 2] = byte.Parse(_string.Substring(i, 2), System.Globalization.NumberStyles.HexNumber);

            return bytes;
        }
    }
}
