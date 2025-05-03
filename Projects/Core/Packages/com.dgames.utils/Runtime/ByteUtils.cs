using System;
using System.Linq;

namespace dgames.Utils
{
    /// <summary>
    /// Provides utility methods for byte manipulation.
    /// </summary>
    public static class ByteUtils
    {
        /// <summary>
        /// Sets the value of a specific bit in a byte.
        /// </summary>
        /// <param name="_byte">The byte to modify.</param>
        /// <param name="value">The value to set the bit to (true for 1, false for 0).</param>
        /// <param name="position">The position of the bit (0 is the least significant bit).</param>
        /// <returns>A new byte with the specified bit set to the given value.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if the position is outside the valid range (0 to 7).</exception>
        public static byte SetBit(this byte _byte, bool value, int position)
        {
            if (position < 0 || position > 7)
                throw new ArgumentOutOfRangeException(nameof(position), "Position must be between 0 and 7.");

            byte mask = (byte)(1 << position);
            return value ? (byte)(_byte | mask) : (byte)(_byte & ~mask);
        }

        /// <summary>
        /// Gets the value of a specific bit in a byte.
        /// </summary>
        /// <param name="_byte">The byte to check.</param>
        /// <param name="position">The position of the bit (0 is the least significant bit).</param>
        /// <returns>True if the bit at the specified position is set to 1, otherwise false.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if the position is outside the valid range (0 to 7).</exception>
        public static bool GetBit(this byte _byte, int position)
        {
            if (position < 0 || position > 7)
                throw new ArgumentOutOfRangeException(nameof(position), "Position must be between 0 and 7.");

            return (_byte & (1 << position)) != 0;
        }

        /// <summary>
        /// Converts a byte to its hexadecimal string representation.
        /// </summary>
        /// <param name="_byte">The byte to convert.</param>
        /// <returns>The hexadecimal string representation of the byte.</returns>
        public static string ToHex(this byte _byte)
            => _byte.ToString("X2");

        /// <summary>
        /// Converts an array of bytes to its hexadecimal string representation.
        /// </summary>
        /// <param name="_bytes">The byte array to convert.</param>
        /// <returns>A concatenated string of the hexadecimal representations of the bytes.</returns>
        public static string ToHex(this byte[] _bytes)
        {
            if (_bytes == null || _bytes.Length == 0)
                return string.Empty;

            return string.Concat(_bytes.Select(b => b.ToHex()));
        }

        /// <summary>
        /// Reads a specified number of bytes from a byte array as a big-endian integer.
        /// </summary>
        /// <param name="data">The byte array containing the data.</param>
        /// <param name="startIndex">The starting index in the byte array.</param>
        /// <param name="bytes">The number of bytes to read.</param>
        /// <returns>The integer representation of the bytes in big-endian order.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if the startIndex or bytes count exceeds the array bounds.</exception>
        public static int ReadBigEndian(this byte[] data, int startIndex, int bytes)
        {
            if (startIndex < 0 || startIndex + bytes > data.Length)
                throw new ArgumentOutOfRangeException("startIndex or bytes exceeds array bounds.");

            int result = 0;
            for (int i = 0; i < bytes; i++)
            {
                result |= data[startIndex + i] << ((bytes - 1 - i) * 8);
            }
            return result;
        }

        /// <summary>
        /// Reads a specified number of bits from a byte as a big-endian value.
        /// </summary>
        /// <param name="data">The byte containing the data.</param>
        /// <param name="startIndex">The starting index of the bits (0 is the most significant bit).</param>
        /// <param name="bits">The number of bits to read.</param>
        /// <returns>A byte representing the read bits in big-endian order.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if the startIndex or bits count exceeds the byte bounds.</exception>
        public static byte ReadBitsBigEndian(this byte data, int startIndex, int bits)
        {
            if (startIndex < 0 || startIndex + bits > 8)
                throw new ArgumentOutOfRangeException("startIndex or bits exceeds byte bounds.");

            byte result = 0;

            for (int i = 0; i < bits; i++)
            {
                int bitIndex = 7 - (startIndex + i);
                if ((data & (1 << bitIndex)) != 0)
                    result |= (byte)(1 << (bits - 1 - i));
            }

            return result;
        }

        /// <summary>
        /// Reverses the bits in a byte.
        /// </summary>
        /// <param name="value">The byte whose bits are to be reversed.</param>
        /// <returns>A new byte with the bits reversed.</returns>
        public static byte ReverseBits8(this byte value)
        {
            byte reversed = 0;

            for (int i = 0; i < 8; i++)
            {
                reversed <<= 1;
                reversed |= (byte)(value & 1);
                value >>= 1;
            }

            return reversed;
        }
    }
}
