using System;
using System.Linq;
using System.Text;

namespace dgames.Utilities
{
    public static class ByteUtilities
    {
        /// <summary>
        /// Modifies the value of a bit at a specific position in a byte.
        /// </summary>
        /// <param name="_byte">The byte to modify.</param>
        /// <param name="value">The value to set (true for 1, false for 0).</param>
        /// <param name="position">The position of the bit to modify (0-7).</param>
        /// <returns>The modified byte.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when position is less than 0 or greater than 7.</exception>
        public static byte SetBit(this byte _byte, bool value, int position)
        {
            if (position < 0 || position > 7)
                throw new ArgumentOutOfRangeException(nameof(position), "Position must be between 0 and 7.");

            // Calculate the mask for the bit at the specified position
            byte mask = (byte)(1 << position);

            // Use the mask to set or clear the bit
            return value ? (byte)(_byte | mask) : (byte)(_byte & ~mask);
        }

        /// <summary>
        /// Reads the value of a bit at a specific position in a byte.
        /// </summary>
        /// <param name="_byte">The byte to examine.</param>
        /// <param name="position">The position of the bit to read (0-7).</param>
        /// <returns>True if the bit is 1, otherwise false.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when position is less than 0 or greater than 7.</exception>
        public static bool GetBit(this byte _byte, int position)
        {
            if (position < 0 || position > 7)
                throw new ArgumentOutOfRangeException(nameof(position), "Position must be between 0 and 7.");

            // Check if the bit at the specified position is 1
            return (_byte & (1 << position)) != 0;
        }

        /// <summary>
        /// Converts a byte to its hexadecimal string representation.
        /// </summary>
        /// <param name="_byte">The byte to convert.</param>
        /// <returns>A string representing the hexadecimal value of the byte.</returns>
        public static string ToHex(this byte _byte)
        {
            // Format the byte as a hexadecimal string with two digits
            return _byte.ToString("X2");
        }

		/// <summary>
		/// Converts a byte array to a hexadecimal string.
		/// Returns an empty string if the input is null or empty.
		/// </summary>
		/// <param name="_bytes">The byte array to convert.</param>
		/// <returns>A string containing the hexadecimal representation of the byte array.</returns>
		public static string ToHex(this byte[] _bytes)
		{
			if (_bytes == null || _bytes.Length == 0)
				return string.Empty;

			// Convert each byte in the array to a hexadecimal string
			return string.Concat(_bytes.Select(b => b.ToString("X2")));
		}

		/// <summary>
		/// Reads an integer value from a byte array in big-endian format, starting from a specified index.
		/// </summary>
		/// <param name="data">The byte array containing the data to read from.</param>
		/// <param name="startIndex">The index in the byte array where reading begins.</param>
		/// <param name="bytes">The number of bytes to read (bits should typically be 1, 2, 4, or 8).</param>
		/// <returns>The integer value read from the byte array.</returns>
		public static int ReadBigEndian(this byte[] data, int startIndex, int bytes)
        {
            int result = data[startIndex] << (8 * bytes);

            for (int i = 0; i < bytes; i++)
                result |= data[startIndex + i] << ((bytes - 1 - i) * 8);

            return result;
        }

        /// <summary>
        /// Reads a specified number of bits from a byte starting from a given index, in Big Endian order.
        /// </summary>
        /// <param name="data">The byte from which to read bits.</param>
        /// <param name="startIndex">The starting bit index (0-based) from which to read.</param>
        /// <param name="bits">The number of bits to read (must be <= 8).</param>
        /// <returns>A byte containing the bits read in Big Endian order.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when startIndex or bits exceed byte bounds.</exception>
        public static byte ReadBitsBigEndian(this byte data, int startIndex, int bits)
        {
            // Validate the input parameters
            if (startIndex < 0 || startIndex + bits > 8)
            {
                throw new ArgumentOutOfRangeException("startIndex or bits exceeds byte bounds.");
            }

            byte result = 0;

            // Read the bits in Big Endian order
            for (int i = 0; i < bits; i++)
            {
                // Calculate the position of the bit to read
                int bitIndex = 7 - (startIndex + i); // Big Endian: the most significant bit is at index 7

                // Check if the bit is 1
                if ((data & (1 << bitIndex)) != 0)
                {
                    result |= (byte)(1 << (bits - 1 - i)); // Place the bit in the correct position in the result
                }
            }

            return result;
        }

        /// <summary>
        /// Converts a byte array to a hexadecimal string representation.
        /// </summary>
        /// <param name="_byte">The byte array to convert.</param>
        /// <returns>A string representing the byte array in hexadecimal format.</returns>
        /// <remarks>
        /// Each byte is represented by two uppercase hexadecimal characters (e.g., "FF").
        /// The bytes are separated by spaces for readability.
        /// </remarks>
        public static string MakeString(this byte[] _byte)
        {
            if (_byte == null || _byte.Length == 0)
            {
                return string.Empty; // Return an empty string if the array is null or empty
            }

            // StringBuilder is used for efficient concatenation of hexadecimal strings
            StringBuilder hexString = new StringBuilder(_byte.Length * 3);

            // Loop through each byte in the array
            foreach (byte b in _byte)
            {
                // Append the byte in hexadecimal format, padded with zero if necessary, followed by a space
                hexString.AppendFormat("{0:X2} ", b);
            }

            // Trim the trailing space for a clean result and return the formatted string
            return hexString.ToString().TrimEnd();
        }

        /// <summary>
        /// Reverses the bits of an 8-bit byte.
        /// This method takes a byte value, processes each bit, and returns a new byte
        /// with the bits in reverse order. For example, the input 10100000
        /// will produce the output 00000101.
        /// </summary>
        /// <param name="value">The byte value whose bits are to be reversed.</param>
        /// <returns>A byte with the bits reversed.</returns>
        public static byte ReverseBits8(this byte value)
        {
            byte reversed = 0; // Variable to hold the reversed bits

            // Loop to process each bit (8 bits total)
            for (int i = 0; i < 8; i++)
            {
                reversed <<= 1; // Shift the reversed byte left by 1 to make room for the next bit
                reversed |= (byte)(value & 1); // Add the least significant bit of value to reversed
                value >>= 1; // Shift the original value right to process the next bit
            }

            return reversed; // Return the byte with reversed bits
        }
    }
}
