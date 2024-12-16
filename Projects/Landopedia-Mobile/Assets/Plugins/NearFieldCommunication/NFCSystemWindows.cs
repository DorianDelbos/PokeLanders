#if PLATFORM_STANDALONE_WIN
using System;
using System.Threading;
using System.Threading.Tasks;

namespace dgames.nfc
{
    public static partial class NFCSystem
    {
#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        private static async Task<byte[]> ReadTagAsync(CancellationToken cancellationToken)
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
            throw new NotImplementedException();
        }
        
#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        private static async Task<byte[]> ReadBlockAsync(int block, int sector, CancellationToken cancellationToken)
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
            throw new NotImplementedException();
        }
        
#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
		private static async Task<byte[]> WriteBlockAsync(int block, int sector, CancellationToken cancellationToken)
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
		{
            throw new NotImplementedException();
		}
    }
}
#endif
