using System.Collections;
using System.Linq;
using UnityEngine;

public class NFC : MonoBehaviour
{
    private Console console => Console.current;
	private Coroutine routine = null;

    public void TryReadTag()
    {
		if (routine == null)
			routine = StartCoroutine(ReadTagCoroutine());
	}

	private IEnumerator ReadTagCoroutine()
	{
#if PLATFORM_ANDROID
		InitializeNfcReader();

		bool isRead = false;

		while (!isRead)
		{
			console.AppendText("Place a NFC tag ...");

			try
			{
				AndroidJavaObject tag = GetNfcTag();
				if (tag != null)
				{
					string[] techList = GetTechList(tag);

					if (techList.Contains("android.nfc.tech.MifareClassic"))
					{
						HandleMifareClassicTag(tag, 1, 0);
						HandleMifareClassicTag(tag, 2, 0);
						HandleMifareClassicTag(tag, 4, 1);

						isRead = true;
					}
					else
					{
						console.AppendText("Tag is not MifareClassic.");
					}

					PauseNfcReader();
					ClearNfcIntent();
				}
			}
			catch (System.Exception e)
			{
				console.AppendText(e.StackTrace, "red");

				PauseNfcReader();
				ClearNfcIntent();

				break;
			}

			yield return new WaitForSeconds(1.0f);
		}

		routine = null;
#endif
	}

	private void InitializeNfcReader()
	{
		AndroidJavaObject mActivity = new AndroidJavaClass("com.unity3d.player.UnityPlayer").GetStatic<AndroidJavaObject>("currentActivity");
		AndroidJavaObject nfcReader = new AndroidJavaObject("com.dgames.nfchandlerlib.NfcHandler");
		nfcReader.Call("setContext", mActivity);
		nfcReader.Call("onResume");
	}

	private AndroidJavaObject GetNfcTag()
	{
		AndroidJavaObject mActivity = new AndroidJavaClass("com.unity3d.player.UnityPlayer").GetStatic<AndroidJavaObject>("currentActivity");
		AndroidJavaObject intent = mActivity.Call<AndroidJavaObject>("getIntent");
		return intent.Call<AndroidJavaObject>("getParcelableExtra", "android.nfc.extra.TAG");
	}

	private string[] GetTechList(AndroidJavaObject tag)
	{
		return tag.Call<string[]>("getTechList");
	}

	private byte[] HandleMifareClassicTag(AndroidJavaObject tag, int blockToRead, int sector)
	{
		AndroidJavaClass mifareClassicClass = new AndroidJavaClass("android.nfc.tech.MifareClassic");
		AndroidJavaObject mifareClassic = mifareClassicClass.CallStatic<AndroidJavaObject>("get", tag);

		mifareClassic.Call("connect");

		byte[] defaultKey = new byte[] { 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF };
		bool auth = mifareClassic.Call<bool>("authenticateSectorWithKeyA", sector, defaultKey);

		if (auth)
		{
			byte[] blockData = mifareClassic.Call<byte[]>("readBlock", blockToRead);
			console.AppendText($"Block {blockToRead} data: {blockData.BytesToHex()}");

			mifareClassic.Call("close");
			return blockData;
		}
		else
		{
			console.AppendText("Authentication failed!", "yellow");
		}

		mifareClassic.Call("close");
		return null;
	}

	private void PauseNfcReader()
	{
		AndroidJavaObject nfcReader = new AndroidJavaObject("com.dgames.nfchandlerlib.NfcHandler");
		nfcReader.Call("onPause");
	}

	private void ClearNfcIntent()
	{
		AndroidJavaObject mActivity = new AndroidJavaClass("com.unity3d.player.UnityPlayer").GetStatic<AndroidJavaObject>("currentActivity");
		mActivity.Call("setIntent", new AndroidJavaObject("android.content.Intent"));
	}
}

static class BytesUtilities
{
	public static string BytesToHex(this byte[] bytes)
	{
		string hexString = "";

		foreach (byte b in bytes)
			hexString += b.ToString("X2");

		return hexString;
	}
}