using System;
using UnityEngine;

namespace Lander.Module.Utilities
{
	public class JsonUtilities
	{
		public static T[] GetJsonArray<T>(string json)
		{
			string newJson = "{ \"array\": " + json + "}";
			Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(newJson);
			return wrapper.array;
		}

		[Serializable]
		private class Wrapper<T>
		{
			public T[] array;
		}
	}
}
