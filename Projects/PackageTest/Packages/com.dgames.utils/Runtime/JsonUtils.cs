using System;

namespace dgames.Utils
{
    /// <summary>
    /// Provides utility methods for JSON serialization and deserialization.
    /// </summary>
    public class JsonUtils
    {
        /// <summary>
        /// Deserializes a JSON string into an object of the specified type.
        /// Supports both single objects and arrays.
        /// </summary>
        /// <typeparam name="T">The type to which the JSON string should be deserialized.</typeparam>
        /// <param name="json">The JSON string to deserialize.</param>
        /// <returns>An object of type T populated with data from the JSON string.</returns>
        public static T FromJson<T>(string json)
        {
            // Check if T is an array type
            if (typeof(T).IsArray)
            {
                Type elementType = typeof(T).GetElementType(); // Get the type of elements in the array
                Type jsonArrayWrapperType = typeof(Wrapper<>).MakeGenericType(elementType); // Create a wrapper type for the array

                // Wrap the JSON array into a new JSON object with an "items" field
                string newJson = "{ \"items\": " + json + "}";

                // Deserialize the new JSON into the wrapper type
                object deserializedObject = UnityEngine.JsonUtility.FromJson(newJson, jsonArrayWrapperType);

                // Access the "items" field in the wrapper class
                var itemsField = jsonArrayWrapperType.GetField("items");
                if (itemsField != null)
                {
                    var itemsValue = itemsField.GetValue(deserializedObject); // Get the value of the "items" field
                    return (T)itemsValue; // Cast and return the deserialized array
                }

                return default; // Return default value if "items" field is not found
            }

            // For non-array types, simply use Unity's JsonUtility
            return UnityEngine.JsonUtility.FromJson<T>(json);
        }

        /// <summary>
        /// A helper class used to wrap an array of elements when deserializing JSON arrays.
        /// </summary>
        /// <typeparam name="T">The type of elements in the array.</typeparam>
        [Serializable]
        private class Wrapper<T>
        {
            public T[] items; // Field to hold the array of items
        }
    }
}
