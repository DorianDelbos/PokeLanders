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
}
