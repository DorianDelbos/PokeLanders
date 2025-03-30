using System.Threading;

namespace dgames.nfc
{
    public static partial class NFCSystem
	{
        public static AsyncOperationNfc ReadTag(int timeout = -1)
        {
            using (var cts = new CancellationTokenSource(timeout))
            {
                return new AsyncOperationNfc(ReadTagAsync(cts.Token));
            }
        }

        public static AsyncOperationNfc ReadBlock(int block, int sector, int timeout = -1)
        {
            using (var cts = new CancellationTokenSource(timeout))
            {
                return new AsyncOperationNfc(ReadBlockAsync(block, sector, cts.Token));
            }
		}

		public static AsyncOperationNfc WriteBlock(int block, int sector, int timeout = -1)
        {
            using (var cts = new CancellationTokenSource(timeout))
            {
                return new AsyncOperationNfc(WriteBlockAsync(block, sector, cts.Token));
            }
		}
	}
}
