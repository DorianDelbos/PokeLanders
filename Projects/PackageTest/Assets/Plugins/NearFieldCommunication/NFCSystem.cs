using System;
using System.Threading;

namespace dgames.nfc
{
    public partial class NFCSystem
	{
		public int timeout = -1;

        public async void ReadTagAsync(Action<bool, byte[], Exception> onComplete)
		{
			try
			{
                using (var cts = new CancellationTokenSource(timeout))
				{
					byte[] result = await ReadTagAsync(cts.Token);
					onComplete?.Invoke(true, result, null);
				}
			}
			catch (Exception e)
			{
				onComplete?.Invoke(false, null, e);
			}
		}

		public async void ReadBlockAsync(int sector, int block, Action<bool, byte[], Exception> onComplete)
		{
			try
            {
                using (var cts = new CancellationTokenSource(timeout))
                {
					byte[] result = await ReadBlockAsync(block, sector, cts.Token);
					onComplete?.Invoke(true, result, null);
				}
			}
			catch (Exception e)
			{
				onComplete?.Invoke(false, null, e);
			}
		}
    }
}
