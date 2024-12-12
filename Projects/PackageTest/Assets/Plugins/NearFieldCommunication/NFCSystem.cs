using System;

namespace dgames.nfc
{
    public static partial class NFCSystem
    {
#pragma warning disable CS1998
        public static async void ReadTag()
#pragma warning restore CS1998
        {
#if PLATFORM_STANDALONE_WIN
            await ReadTagWindows();
#elif PLATFORM_ANDROID
            await ReadTagAndroid();
#else
		    throw new NotImplementedException("NFCSystem is not supported on your platforms !");
#endif
        }
    }
}
