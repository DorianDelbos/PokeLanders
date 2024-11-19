using System;
using System.Net.Http;
using System.Threading.Tasks;
using UnityEngine;

namespace LandersLegends.Extern
{
	public static class DataFetcher<T> where T : IBaseModel
	{
		private static readonly HttpClient client = new HttpClient();
#if UNITY_EDITOR
		private static readonly string apiUrl = "http://localhost:5000";
#else
		private static readonly string apiUrl = "https://landopedia-gwhtbqbqdhd4d5hw.westeurope-01.azurewebsites.net";
#endif

		public static async Task<T[]> FetchArrayDataAsync(string req)
		{
			try
			{
				HttpResponseMessage res = await client.GetAsync($"{apiUrl}/{req}");
				res.EnsureSuccessStatusCode();
				string json = await res.Content.ReadAsStringAsync();
				return JsonUtils.GetJsonArray<T>(json);
			}
			catch (HttpRequestException e)
			{
				throw new Exception($"Error during API request: {e.Message}", e);
			}
			catch (Exception e)
			{
				throw new Exception($"Error during API request: {e.Message}", e);
			}
		}

		public static T[] FetchArrayData(string req) => Task.Run(async () => await FetchArrayDataAsync(req)).Result;

		public static async Task<T> FetchDataAsync(string req)
		{
			try
			{
				HttpResponseMessage res = await client.GetAsync($"{apiUrl}/{req}");
				res.EnsureSuccessStatusCode();
				string json = await res.Content.ReadAsStringAsync();
				return JsonUtility.FromJson<T>(json);
			}
			catch (HttpRequestException e)
			{
				throw new Exception($"Error during API request: {e.Message}", e);
			}
			catch (Exception e)
			{
				throw new Exception($"Error during API request: {e.Message}", e);
			}
		}

		public static T FetchData(string req) => Task.Run(async () => await FetchDataAsync(req)).Result;
	}
}
