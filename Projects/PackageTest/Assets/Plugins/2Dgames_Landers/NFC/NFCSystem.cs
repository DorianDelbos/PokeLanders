using System;
using System.Collections.Generic;

public static partial class NFCSystem
{
#if PLATFORM_ANDROID || UNITY_EDITOR
	public static Action<List<byte[]>> onBlocksRead;
	public static Action<Exception> onError;
#endif

    public static async void Read()
    {
#if PLATFORM_ANDROID || UNITY_EDITOR
		await ReadAndroid();
#elif PLATFORM_STANDALONE_WIN
        throw new NotImplementedException("NFCSystem is not supported on windows platforms !");
#else
		throw new NotImplementedException("NFCSystem is not supported on your platforms !");
#endif
    }

	public static void Write()
    {
#if PLATFORM_ANDROID || UNITY_EDITOR
        throw new NotImplementedException("NFCSystem is not supported on android platforms !");
#elif PLATFORM_STANDALONE_WIN
        throw new NotImplementedException("NFCSystem is not supported on windows platforms !");
#else
		throw new NotImplementedException("NFCSystem is not supported on your platforms !");
#endif
    }
}