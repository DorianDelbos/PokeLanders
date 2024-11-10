using System;

namespace Dgames
{
    public static class IntUtilities
    {
        /// <summary>
        /// Modifies the value of a bit at a specific position in an int.
        /// </summary>
        /// <param name="_int">The int to modify.</param>
        /// <param name="value">The value to set (true for 1, false for 0).</param>
        /// <param name="position">The position of the bit to modify (0-31).</param>
        /// <returns>The modified int.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when position is less than 0 or greater than 31.</exception>
        public static int SetBit(this int _int, bool value, int position)
        {
            if (position < 0 || position > 31)
                throw new ArgumentOutOfRangeException(nameof(position), "Position must be between 0 and 31.");

            // Calculate the mask for the bit at the specified position
            int mask = 1 << position;

            // Use the mask to set or clear the bit
            return value ? (_int | mask) : (_int & ~mask);
        }

        /// <summary>
        /// Reads the value of a bit at a specific position in an int.
        /// </summary>
        /// <param name="_int">The int to examine.</param>
        /// <param name="position">The position of the bit to read (0-31).</param>
        /// <returns>True if the bit is 1, otherwise false.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when position is less than 0 or greater than 31.</exception>
        public static bool GetBit(this int _int, int position)
        {
            if (position < 0 || position > 31)
                throw new ArgumentOutOfRangeException(nameof(position), "Position must be between 0 and 31.");

            // Check if the bit at the specified position is 1
            return (_int & (1 << position)) != 0;
        }
    }
}
