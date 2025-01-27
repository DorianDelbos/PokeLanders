using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace dgames.http
{
	public class AsyncOperationWeb<TObject>
	{
		public bool IsDone { get; private set; }
        public bool IsError => Exception != null;
        public Exception Exception { get; private set; }
		public TObject Result { get; private set; }

		private Action<AsyncOperationWeb<TObject>> completeCallback;
		public event Action<AsyncOperationWeb<TObject>> OnComplete
		{
			add
			{
				if (IsDone) value(this);
				else completeCallback = (Action<AsyncOperationWeb<TObject>>)Delegate.Combine(completeCallback, value);
			}
			remove => completeCallback = (Action<AsyncOperationWeb<TObject>>)Delegate.Remove(completeCallback, value);
		}

		public AsyncOperationWeb(string req, Func<HttpContent, Task<TObject>> processContent)
		{
			SetOperation(req, processContent);
		}

		private async void SetOperation(string req, Func<HttpContent, Task<TObject>> processContent)
		{
			IsDone = false;
			try
			{
				HttpResponseMessage response = await new HttpClient().GetAsync(req, HttpCompletionOption.ResponseContentRead);
				response.EnsureSuccessStatusCode();
				Result = await processContent(response.Content);
			}
			catch (Exception e)
			{
				Exception = new Exception($"{req}\n{e.Message}", e);
			}
			finally
			{
				IsDone = true;
				completeCallback?.Invoke(this);
			}
		}
	}
}
