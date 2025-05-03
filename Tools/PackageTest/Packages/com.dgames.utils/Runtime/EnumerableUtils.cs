using System;
using System.Collections.Generic;
using System.Linq;

namespace dgames.Utils
{
    public static class EnumerableUtils
    {
        public static Queue<T> ToQueue<T>(this IEnumerable<T> source)
        {
            return new Queue<T>(source);
        }

        public static Stack<T> ToStack<T>(this IEnumerable<T> source)
        {
            return new Stack<T>(source);
        }

        public static bool IsEmpty<T>(this IEnumerable<T> source)
        {
            return !source.Any();
        }

        public static T GetRandom<T>(this IEnumerable<T> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            // Convert IEnumerable to a List to allow random access
            List<T> list = source.ToList();

            if (list.Count == 0)
                throw new InvalidOperationException("Sequence contains no elements.");

            Random random = new Random();
            int index = random.Next(list.Count); // Get a random index
            return list[index]; // Return the element at the random index
        }
    }
}
