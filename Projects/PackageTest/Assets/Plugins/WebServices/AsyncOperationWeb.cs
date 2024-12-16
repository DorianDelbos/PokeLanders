using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace dgames.http
{
	public class AsyncOperationWeb<TObject>
	{
		public bool IsDone { get; private set; }
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

		public AsyncOperationWeb(Task<HttpResponseMessage> asyncRequest, Func<HttpContent, Task<TObject>> processContent)
		{
			SetOperation(asyncRequest, processContent);
		}

		private async void SetOperation(Task<HttpResponseMessage> asyncRequest, Func<HttpContent, Task<TObject>> processContent)
		{
			IsDone = false;
			try
			{
				HttpResponseMessage response = await asyncRequest;
				response.EnsureSuccessStatusCode();
				Result = await processContent(response.Content);
			}
			catch (Exception e)
			{
				Exception = e;
			}
			finally
			{
				IsDone = true;
				completeCallback?.Invoke(this);
			}
		}
	}
}
