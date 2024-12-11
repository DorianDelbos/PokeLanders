using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace dgames.http
{
    /// <summary>
    /// Provides functionality to perform web service requests using an HttpClient.
    /// </summary>
    public class WebService : IDisposable
    {
        private readonly HttpClient _client;

        /// <summary>
        /// Initializes a new instance of the <see cref="WebService"/> class with an optional HttpClient.
        /// </summary>
        /// <param name="client">An optional <see cref="HttpClient"/> instance. If null, a new HttpClient is created.</param>
        public WebService(HttpClient? client = null)
        {
            _client = client ?? new HttpClient();
        }

        /// <summary>
        /// Sends an asynchronous GET request to the specified URL and returns the response content as a byte array.
        /// </summary>
        /// <param name="req">The URL to send the GET request to.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the response content as a byte array.</returns>
        /// <exception cref="ArgumentException">Thrown if the request URL is null or whitespace.</exception>
        /// <exception cref="Exception">
        /// Thrown if an HTTP request error occurs or if the response content is empty.
        /// </exception>
        public async Task<byte[]> AsyncRequest(string req)
        {
            if (string.IsNullOrWhiteSpace(req))
                throw new ArgumentException("Request URL cannot be null or whitespace.", nameof(req));

            try
            {
                using var response = await _client.GetAsync(req, HttpCompletionOption.ResponseContentRead);
                response.EnsureSuccessStatusCode();

                byte[] content = await response.Content.ReadAsByteArrayAsync();
                if (content == null || content.Length == 0)
                    throw new Exception("Response content is empty.");

                return content;
            }
            catch (HttpRequestException e)
            {
                throw new Exception($"HTTP request error: {e.Message}", e);
            }
            catch (Exception e)
            {
                throw new Exception($"Unexpected error: {e.Message}", e);
            }
        }

        /// <summary>
        /// A wrapper for <see cref="AsyncRequest"/> to send an asynchronous GET request to the specified URL.
        /// </summary>
        /// <param name="req">The URL to send the GET request to.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the response content as a byte array.</returns>
        public async Task<byte[]> RequestAsync(string req) => await AsyncRequest(req);

        /// <summary>
        /// Releases the resources used by the <see cref="WebService"/> instance, including the underlying HttpClient.
        /// </summary>
        public void Dispose()
        {
            _client.Dispose();
        }
    }
}
