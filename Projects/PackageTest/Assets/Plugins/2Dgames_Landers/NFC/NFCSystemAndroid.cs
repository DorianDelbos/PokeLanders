#if PLATFORM_ANDROID || UNITY_EDITOR
using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine;
using System.Threading.Tasks;

public static partial class NFCSystem
{
    private static async Task ReadAndroid()
    {
        InitializeNfcReader();
        bool isRead = false;

        while (!isRead)
        {
            try
            {
                AndroidJavaObject tag = GetNfcTag();

                if (tag != null)
                {
                    string[] techList = GetTechList(tag);

                    // TAG DETECT
                    if (techList.Contains("android.nfc.tech.MifareClassic"))
                    {
                        List<byte[]> resultData = new List<byte[]>();
                        resultData.Add(HandleMifareClassicTag(tag, 1, 0));
                        resultData.Add(HandleMifareClassicTag(tag, 2, 0));
                        resultData.Add(HandleMifareClassicTag(tag, 4, 1));

                        isRead = true;

                        // Trigger event
                        onBlocksRead?.Invoke(resultData);
                    }
                    else
                    {
                        // TAG ERROR
                        onError?.Invoke(new Exception("Tag is not MifareClassic."));
                    }

                    PauseNfcReader();
                    ClearNfcIntent();
                }
            }
            catch (Exception e)
            {
                // TAG ERROR
                onError?.Invoke(e);

                PauseNfcReader();
                ClearNfcIntent();
                break;
            }

            await Task.Delay(1000);
        }
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
        else
        {
            // TAG ERROR authentification
            onError?.Invoke(new Exception("Authentication failed to MifareClasic !"));
        }

        mifareClassic.Call("close");
        return null;
    }

    private static void PauseNfcReader()
    {
        AndroidJavaObject nfcReader = new AndroidJavaObject("com.dgames.nfchandlerlib.NfcHandler");
        nfcReader.Call("onPause");
    }

    private static void ClearNfcIntent()
    {
        AndroidJavaObject mActivity = new AndroidJavaClass("com.unity3d.player.UnityPlayer").GetStatic<AndroidJavaObject>("currentActivity");
        mActivity.Call("setIntent", new AndroidJavaObject("android.content.Intent"));
    }
}
#endif