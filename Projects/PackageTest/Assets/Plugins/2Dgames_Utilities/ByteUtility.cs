using System;
using System.Linq;

namespace dgames.Utilities
{
	public static class ByteUtility
    {
        public static byte SetBit(this byte _byte, bool value, int position)
        {
            if (position < 0 || position > 7)
                throw new ArgumentOutOfRangeException(nameof(position), "Position must be between 0 and 7.");

            byte mask = (byte)(1 << position);
            return value ? (byte)(_byte | mask) : (byte)(_byte & ~mask);
        }

        public static bool GetBit(this byte _byte, int position)
        {
            if (position < 0 || position > 7)
                throw new ArgumentOutOfRangeException(nameof(position), "Position must be between 0 and 7.");

            return (_byte & (1 << position)) != 0;
        }

        public static string ToHex(this byte _byte)
            => _byte.ToString("X2");

		public static string ToHex(this byte[] _bytes)
		{
			if (_bytes == null || _bytes.Length == 0)
				return string.Empty;

			return string.Concat(_bytes.Select(b => b.ToHex()));
		}

		public static int ReadBigEndian(this byte[] data, int startIndex, int bytes)
        {
            int result = data[startIndex] << (8 * bytes);

            for (int i = 0; i < bytes; i++)
                result |= data[startIndex + i] << ((bytes - 1 - i) * 8);

            return result;
        }

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
