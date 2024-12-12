using System;
using System.Threading.Tasks;
using System.Threading;

namespace dgames.nfc
{
    public static partial class NFCSystem
	{
		public static async void ReadTagAsync(Action<bool, byte[], Exception> onComplete, int timeoutMilliseconds = -1)
		{
			try
			{
				using (var cts = new CancellationTokenSource(timeoutMilliseconds))
				{
					byte[] result = await ReadTagInternalAsync(cts.Token);
					onComplete?.Invoke(true, result, null);
				}
			}
			catch (Exception e)
			{
				onComplete?.Invoke(false, null, e);
			}
		}

		public static async void ReadBlockAsync(int sector, int block, Action<bool, byte[], Exception> onComplete, int timeoutMilliseconds = -1)
		{
			try
			{
				using (var cts = new CancellationTokenSource(timeoutMilliseconds))
				{
					byte[] result = await ReadBlockInternalAsync(block, sector, cts.Token);
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
