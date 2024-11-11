using Lander.Module.Utilities;
using System;
using System.Net.Http;
using System.Net.Sockets;
using System.Threading.Tasks;
using UnityEngine;

namespace Lander.Module.API
{
	public static class DataFetcher<T> where T : IBaseModel
	{
		private static readonly HttpClient client = new HttpClient();

#if UNITY_EDITOR
        private static readonly string apiUrl = "http://localhost:5000";
#else
        private static readonly string apiUrl = "http://landopedia-gwhtbqbqdhd4d5hw.westeurope-01.azurewebsites.net";
#endif

        public static async Task<T[]> FetchArrayDataAsync(string req, Action<T[]> onSucess = null, Action onError = null)
		{
			try
			{
				HttpResponseMessage res = await client.GetAsync($"{apiUrl}/{req}");
				res.EnsureSuccessStatusCode();
				string json = await res.Content.ReadAsStringAsync();
				T[] result = JsonUtilities.GetJsonArray<T>(json);
                onSucess?.Invoke(result);
                return result;
			}
			catch (HttpRequestException e)
            {
				onError?.Invoke();
                throw new Exception($"Error during API request: {e.Message}", e);
			}
			catch (SocketException e)
            {
                onError?.Invoke();
                throw new Exception($"Error during API request: {e.Message}", e);
			}
			catch (Exception e)
            {
                onError?.Invoke();
                throw new Exception($"Error during API request: {e.Message}", e);
			}
		}

		public static T[] FetchArrayData(string req, Action<T[]> onSucess = null, Action onError = null) => Task.Run(async () => await FetchArrayDataAsync(req, onSucess, onError)).Result;

		public static async Task<T> FetchDataAsync(string req, Action<T> onSucess = null, Action onError = null)
		{
			try
			{
				HttpResponseMessage res = await client.GetAsync($"{apiUrl}/{req}");
				res.EnsureSuccessStatusCode();
				string json = await res.Content.ReadAsStringAsync();
                T result = JsonUtility.FromJson<T>(json);
                onSucess?.Invoke(result);
                return result;
			}
			catch (HttpRequestException e)
            {
                onError?.Invoke();
                throw new Exception($"Error during API request: {e.Message}", e);
			}
			catch (SocketException e)
            {
                onError?.Invoke();
                throw new Exception($"Error during API request: {e.Message}", e);
			}
			catch (Exception e)
            {
                onError?.Invoke();
                throw new Exception($"Error during API request: {e.Message}", e);
			}
		}

		public static T FetchData(string req, Action<T> onSucess = null, Action onError = null) => Task.Run(async () => await FetchDataAsync(req, onSucess, onError)).Result;
	}
}
