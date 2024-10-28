using System;

public static class ByteUtils
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
}
