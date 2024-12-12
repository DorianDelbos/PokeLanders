#if PLATFORM_ANDROID || UNITY_EDITOR
using System;
using System.Threading.Tasks;
using UnityEngine;

namespace dgames.nfc
{
    public static partial class NFCSystem
    {
		public static int timeout = 5000;

        public static async Task<byte[]> ReadTagAndroid()
        {
            InitializeNfcReader();

            DateTime startTime = DateTime.Now;
			byte[] tagDetected = null;

            while (true)
            {
                TimeSpan elapsed = DateTime.Now - startTime;
				bool isTimeout = elapsed.TotalMilliseconds > timeout;
				bool hasTag = (tagDetected = ReadTagAsync()) != null;

                if (isTimeout || hasTag)
                {
                    ClearNfcIntent();

                    if (hasTag)
                        return tagDetected;
                    else if (isTimeout)
						throw new NFCException("NFC, timeout");
                }

                await Task.Delay(100);
            }
        }

        private static byte[] ReadTagAsync()
        {
            try
            {
                AndroidJavaObject tag = GetNfcTag();
                if (tag != null)
                    return tag.Call<byte[]>("getId");
            }
            catch (NFCException e)
            {
				// TAG ERROR
				throw e;
            }

			return null;
        }


        private static void InitializeNfcReader()
		{
			AndroidJavaObject mActivity = new AndroidJavaClass("com.unity3d.player.UnityPlayer").GetStatic<AndroidJavaObject>("currentActivity");
			AndroidJavaObject nfcReader = new AndroidJavaObject("com.dgames.nfchandlerlib.NfcHandler");
			nfcReader.Call("setContext", mActivity);
			nfcReader.Call("onResume");
		}

		private static AndroidJavaObject GetNfcTag()
		{
			AndroidJavaObject mActivity = new AndroidJavaClass("com.unity3d.player.UnityPlayer").GetStatic<AndroidJavaObject>("currentActivity");
			AndroidJavaObject intent = mActivity.Call<AndroidJavaObject>("getIntent");
			return intent.Call<AndroidJavaObject>("getParcelableExtra", "android.nfc.extra.TAG");
		}

		private static string[] GetTechList(AndroidJavaObject tag)
		{
			return tag.Call<string[]>("getTechList");
		}

		private static byte[] HandleMifareClassicTag(AndroidJavaObject tag, int blockToRead, int sector)
		{
			AndroidJavaClass mifareClassicClass = new AndroidJavaClass("android.nfc.tech.MifareClassic");
			AndroidJavaObject mifareClassic = mifareClassicClass.CallStatic<AndroidJavaObject>("get", tag);

			mifareClassic.Call("connect");

			byte[] defaultKey = new byte[] { 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF };
			bool auth = mifareClassic.Call<bool>("authenticateSectorWithKeyA", sector, defaultKey);

			if (auth)
			{
				byte[] blockData = mifareClassic.Call<byte[]>("readBlock", blockToRead);

				mifareClassic.Call("close");
				return blockData;
			}

			mifareClassic.Call("close");
			return null;
		}

		private static void ClearNfcIntent()
        {
			// Pause
            AndroidJavaObject nfcReader = new AndroidJavaObject("com.dgames.nfchandlerlib.NfcHandler");
            nfcReader.Call("onPause");
			// Clear
            AndroidJavaObject mActivity = new AndroidJavaClass("com.unity3d.player.UnityPlayer").GetStatic<AndroidJavaObject>("currentActivity");
			mActivity.Call("setIntent", new AndroidJavaObject("android.content.Intent"));
		}
    }
}
#endif
