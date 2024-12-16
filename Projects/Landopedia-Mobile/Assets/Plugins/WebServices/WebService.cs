using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace dgames.http
{
    public class WebService : IDisposable
    {
		#region MAIN
		private readonly HttpClient _client;
		private readonly CancellationTokenSource _cancellationTokenSource;

		public WebService(HttpClient client = null)
		{
			_client = client ?? new HttpClient();
			_cancellationTokenSource = new CancellationTokenSource();
		}

		public void Dispose()
		{
			_cancellationTokenSource.Cancel();
			_cancellationTokenSource.Dispose();
			_client.Dispose();
		}
		
		public void Cancel()
		{
			_cancellationTokenSource.Cancel();
		}
		#endregion

		#region PROCESSORS
		public static async Task<T> JsonProcessor<T>(HttpContent content)
		{
			string json = await content.ReadAsStringAsync();
			return Utilities.JsonUtility.FromJson<T>(json);
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
		public void AsyncRequestJson<T>(string req, Action<bool, T, Exception> onCompleted)
			=> AsyncRequest(req, JsonProcessor<T>, onCompleted);

		public void AsyncRequestImage(string req, Action<bool, Texture2D, Exception> onCompleted)
			=> AsyncRequest(req, ImageProcessor, onCompleted);
		#endregion

		#region GLOBAL REQUESTS
		public async void AsyncRequest<T>(string req, Func<HttpContent, Task<T>> processContent, Action<bool, T, Exception> onCompleted)
		{
			try
			{
				using var response = await _client.GetAsync(req, HttpCompletionOption.ResponseContentRead);
				response.EnsureSuccessStatusCode();

				T result = await processContent(response.Content);

				onCompleted?.Invoke(true, result, null);
			}
			catch (Exception e)
			{
				onCompleted?.Invoke(false, default, e);
			}
		}
		#endregion
	}
}
