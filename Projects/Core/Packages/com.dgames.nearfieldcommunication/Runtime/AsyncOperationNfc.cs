using System;
using System.Threading.Tasks;

namespace dgames.nfc
{
    public class AsyncOperationNfc
    {
        public bool IsDone { get; private set; }
        public bool IsError => Exception != null;
        public Exception Exception { get; private set; }
        public byte[] Result { get; private set; }

        private Action<AsyncOperationNfc> completeCallback;
        public event Action<AsyncOperationNfc> OnComplete
        {
            add
            {
                if (IsDone) value(this);
                else completeCallback = (Action<AsyncOperationNfc>)Delegate.Combine(completeCallback, value);
            }
            remove => completeCallback = (Action<AsyncOperationNfc>)Delegate.Remove(completeCallback, value);
        }

        public AsyncOperationNfc(Task<byte[]> asyncRequest)
        {
            SetOperation(asyncRequest);
        }

        private async void SetOperation(Task<byte[]> asyncRequest)
        {
            IsDone = false;
            try
            {
                Result = await asyncRequest;
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

        public async Task WaitOperation()
        {
            // Wait for the operation to complete
            while (!IsDone)
            {
                await Task.Delay(10);  // Use a small delay to prevent busy-waiting
            }
        }
    }
}
