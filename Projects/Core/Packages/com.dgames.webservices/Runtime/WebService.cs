using System;
using System.Net.Http;
using System.Threading.Tasks;
using UnityEngine;

namespace dgames.http
{
	public static class WebService
	{
		#region PROCESSORS

		public static async Task<T> JsonProcessor<T>(HttpContent content)
		{
			string json = await content.ReadAsStringAsync();
			return Utils.JsonUtils.FromJson<T>(json);
		}

		public static async Task<Texture2D> ImageProcessor(HttpContent content)
		{
			byte[] data = await content.ReadAsByteArrayAsync();
			var texture = new Texture2D(2, 2);
			texture.LoadImage(data);
			return texture;
		}

		#endregion

		#region STANDARD REQUESTS

		public static AsyncOperationWeb<T> AsyncRequestJson<T>(string req)
			=> AsyncRequest(req, JsonProcessor<T>);

		public static AsyncOperationWeb<Texture2D> AsyncRequestImage(string req)
			=> AsyncRequest(req, ImageProcessor);

		#endregion

		#region GLOBAL REQUESTS

		public static AsyncOperationWeb<T> AsyncRequest<T>(string req, Func<HttpContent, Task<T>> processContent)
		{
			Debug.Log($"[WEB SERVICE] {req}");
			return new AsyncOperationWeb<T>(new HttpClient().GetAsync(req, HttpCompletionOption.ResponseContentRead), processContent);
        }

		#endregion
	}
}
