using System;
using UnityEngine;

namespace dgames.Utilities
{
	public class JsonUtilities
	{
		public static T FromJson<T>(string json)
		{
			if (typeof(T).IsArray)
			{
				Type elementType = typeof(T).GetElementType();
				Type jsonArrayWrapperType = typeof(Wrapper<>).MakeGenericType(elementType);

				string newJson = "{ \"items\": " + json + "}";
				object deserializedObject = JsonUtility.FromJson(newJson, jsonArrayWrapperType);

				var itemsField = jsonArrayWrapperType.GetField("items");
				if (itemsField != null)
				{
					var itemsValue = itemsField.GetValue(deserializedObject);
					return (T)itemsValue;
				}

				return default;
			}

			return JsonUtility.FromJson<T>(json);
		}

		[Serializable]
		private class Wrapper<T>
		{
			public T[] items;
		}

	}
}
