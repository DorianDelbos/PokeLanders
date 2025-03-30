using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace dgames.http
{
    /// <summary>
    /// Represents an asynchronous operation for web requests that returns a result of type <typeparamref name="TObject"/>.
    /// </summary>
    /// <typeparam name="TObject">The type of the result object returned by the web request.</typeparam>
    public class AsyncOperationWeb<TObject>
    {
        /// <summary>
        /// Gets a value indicating whether the operation is complete.
        /// </summary>
        public bool IsDone { get; private set; }

        /// <summary>
        /// Gets a value indicating whether an error occurred during the operation.
        /// </summary>
        public bool IsError => Exception != null;

        /// <summary>
        /// Gets the exception that occurred during the operation, if any.
        /// </summary>
        public Exception Exception { get; private set; }

        /// <summary>
        /// Gets the result of the operation, if successful.
        /// </summary>
        public TObject Result { get; private set; }

        private TaskCompletionSource<bool> completionSource;

        /// <summary>
        /// Initializes a new instance of the <see cref="AsyncOperationWeb{TObject}"/> class and starts the operation.
        /// </summary>
        /// <param name="asyncRequest">The asynchronous HTTP request task to execute.</param>
        /// <param name="processContent">A function to process the HTTP content and return a result of type <typeparamref name="TObject"/>.</param>
        public AsyncOperationWeb(Task<HttpResponseMessage> asyncRequest, Func<HttpContent, Task<TObject>> processContent)
        {
            completionSource = new TaskCompletionSource<bool>();
            ExecuteOperation(asyncRequest, processContent);
        }

        /// <summary>
        /// Executes the operation asynchronously.
        /// </summary>
        /// <param name="asyncRequest">The asynchronous HTTP request task.</param>
        /// <param name="processContent">A function to process the HTTP content.</param>
        private async void ExecuteOperation(Task<HttpResponseMessage> asyncRequest, Func<HttpContent, Task<TObject>> processContent)
        {
            try
            {
                HttpResponseMessage response = await asyncRequest;
                response.EnsureSuccessStatusCode();
                Result = await processContent(response.Content);
                IsDone = true;
                completionSource.SetResult(true);
            }
            catch (Exception e)
            {
                Exception = e;
                IsDone = true;
                completionSource.SetResult(false);
                Console.WriteLine($"[WEB SERVICE] An error has occurred. \n {e}");
            }
        }

        /// <summary>
        /// Awaits the completion of the operation, ensuring that the operation has finished before continuing.
        /// </summary>
        public async Task AwaitCompletion()
        {
            // Await the completion of the operation
            await completionSource.Task;
        }
    }
}
