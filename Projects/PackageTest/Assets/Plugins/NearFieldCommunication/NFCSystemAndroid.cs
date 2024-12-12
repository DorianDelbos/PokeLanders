#if PLATFORM_ANDROID || UNITY_EDITOR
using dgames.Utilities;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UIElements;

namespace dgames.nfc
{
    public static partial class NFCSystem
	{
		private static async Task<byte[]> ReadTagInternalAsync(CancellationToken cancellationToken)
		{
			if (Application.platform != RuntimePlatform.Android)
				throw new PlatformNotSupportedException("NFC is not supported on this platform.");

			// Initialization
			AndroidJavaObject tag = null;

			// Get all nfc objects
			AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
			AndroidJavaObject activity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
			AndroidJavaObject nfcAdapter = activity.Call<AndroidJavaObject>("getSystemService", "nfc");
			AndroidJavaObject nfcReader = new AndroidJavaObject("com.dgames.nfchandlerlib.NfcHandler");

			// Enable reader
			InitializeNfcReader(nfcReader, activity);

			if (nfcAdapter != null)
			{
				while (!cancellationToken.IsCancellationRequested && tag == null)
				{
					tag = GetNfcTag();
					await Task.Delay(100);
				}

				if (tag != null)
				{
					byte[] data = tag.Call<byte[]>("getId");
					ClearNfcIntent(nfcReader, activity);
					return data;
				}

				ClearNfcIntent(nfcReader, activity);

				if (cancellationToken.IsCancellationRequested)
					throw new Exception("NFC timeout.");
			}
			else
			{
				ClearNfcIntent(nfcReader, activity);
				throw new Exception("NFC adapter not available.");
			}

			throw new Exception("Failed to read NFC tag.");
		}

		private static async Task<byte[]> ReadBlockInternalAsync(int block, int sector, CancellationToken cancellationToken)
		{
			if (Application.platform != RuntimePlatform.Android)
				throw new PlatformNotSupportedException("NFC is not supported on this platform.");

			// Get all nfc objects
			AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
			AndroidJavaObject activity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
			AndroidJavaObject nfcAdapter = activity.Call<AndroidJavaObject>("getSystemService", "nfc");
			AndroidJavaObject nfcReader = new AndroidJavaObject("com.dgames.nfchandlerlib.NfcHandler");

			// Initialization
			AndroidJavaObject tag = null;

			// Enable reader
			InitializeNfcReader(nfcReader, activity);

			if (nfcAdapter != null)
			{
				while (!cancellationToken.IsCancellationRequested && HasNfcDetect())
				{
					await Task.Delay(100);
				}

				if (tag != null)
				{
					byte[] data = GetMifareBlock(tag, block, sector);
					ClearNfcIntent(nfcReader, activity);
					return data;
				}

				ClearNfcIntent(nfcReader, activity);

				if (cancellationToken.IsCancellationRequested)
					throw new Exception("NFC timeout.");
			}
			else
			{
				ClearNfcIntent(nfcReader, activity);
				throw new Exception("NFC adapter not available.");
			}

			throw new Exception("Failed to read NFC block.");
		}



		private static void InitializeNfcReader(AndroidJavaObject nfcReader, AndroidJavaObject activity)
		{
			nfcReader.Call("setContext", activity);
			nfcReader.Call("onResume");
		}

		private static void ClearNfcIntent(AndroidJavaObject nfcReader, AndroidJavaObject activity)
		{
			nfcReader.Call("onPause");
			activity.Call("setIntent", new AndroidJavaObject("android.content.Intent"));
		}

		private static AndroidJavaObject GetNfcTag()
		{
			AndroidJavaObject mActivity = new AndroidJavaClass("com.unity3d.player.UnityPlayer").GetStatic<AndroidJavaObject>("currentActivity");
			AndroidJavaObject intent = mActivity.Call<AndroidJavaObject>("getIntent");
			return intent.Call<AndroidJavaObject>("getParcelableExtra", "android.nfc.extra.TAG");
		}

		private static bool HasNfcDetect()
			=> GetNfcTag() != null;

		private static bool CheckMifare(AndroidJavaObject tag)
		{
			string[] techs = tag.Call<string[]>("getTechList");
			return techs.Contains("android.nfc.tech.MifareClassic");
		}

		private static bool IsAuthentificate(AndroidJavaObject mifareClassic, int sector)
		{
			mifareClassic.Call("connect");
			byte[] defaultKey = new byte[] { 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF };
			bool auth = mifareClassic.Call<bool>("authenticateSectorWithKeyA", sector, defaultKey);

			return auth;
		}

		private static byte[] GetMifareBlock(AndroidJavaObject tag, int block, int sector)
		{
			if (!CheckMifare(tag))
				return null;

			AndroidJavaClass mifareClassicClass = new AndroidJavaClass("android.nfc.tech.MifareClassic");
			AndroidJavaObject mifareClassic = mifareClassicClass.CallStatic<AndroidJavaObject>("get", tag);
			byte[] result = null;

			if (IsAuthentificate(mifareClassic, sector))
				result = mifareClassic.Call<byte[]>("readBlock", block);

			mifareClassic.Call("close");
			return result;
		}
	}
}
#endif
