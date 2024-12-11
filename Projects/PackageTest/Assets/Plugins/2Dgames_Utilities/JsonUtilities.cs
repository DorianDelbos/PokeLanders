using System;
using UnityEngine;

namespace dgames.Utilities
{
	public class JsonUtilities
	{
        /// <summary>
        /// Parses a JSON array string into an array of the specified type.
        /// </summary>
        /// <typeparam name="T">The type of elements in the array.</typeparam>
        /// <param name="json">The JSON array string to parse. Example: "[1, 2, 3]".</param>
        /// <returns>An array of type <typeparamref name="T"/> parsed from the JSON array string.</returns>
        /// <remarks>
        /// This method wraps the JSON array string into an object with a single field named "array"
        /// to facilitate parsing using Unity's `JsonUtility`. It then deserializes the wrapped object
        /// and extracts the array.
        /// </remarks>
        public static T[] GetJsonArray<T>(string json)
        {
            // Wrap the incoming JSON array string into an object with a single field named "array"
            // Example: If `json` is "[1, 2, 3]", it becomes "{ \"array\": [1, 2, 3] }"
            string newJson = "{ \"array\": " + json + "}";

            // Parse the new JSON string into a Wrapper object containing an array of the desired type
            // Wrapper<T> is a helper class that wraps the array
            Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(newJson);

            // Return the array extracted from the Wrapper object
            return wrapper.array;
        }

        [Serializable]
		private class Wrapper<T>
		{
			public T[] array;
		}
	}
}
