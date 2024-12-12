#if PLATFORM_STANDALONE_WIN
using System;
using System.Threading.Tasks;

namespace dgames.nfc
{
    public static partial class NFCSystem
    {
        public static async Task ReadTagWindows()
        {
            throw new NotImplementedException("NFCSystem is not supported on your platforms !");
        }
    }
}
#endif
