using UnityEngine;

public class NFC : MonoBehaviour
{
    private Console console => Console.current;

    AndroidJavaObject mActivity;
    AndroidJavaObject nfcReader;

    private void OnEnable()
    {
#if PLATFORM_ANDROID
        nfcReader.Call("onResume");
#endif
    }

    private void OnDisable()
    {
#if PLATFORM_ANDROID
        nfcReader.Call("onPause");
#endif
    }

    void Start()
    {
#if PLATFORM_ANDROID
        try
        {
            mActivity = new AndroidJavaClass("com.unity3d.player.UnityPlayer").GetStatic<AndroidJavaObject>("currentActivity");

            using (nfcReader = new AndroidJavaObject("com.dgames.nfchandlerlib.NfcHandler"))
            {
                nfcReader.Call("setContext", mActivity);
                nfcReader.Call("onResume");
            }

            console.AppendText($"NFC reader initialized successfully.", "green");
        }
        catch (System.Exception e)
        {
            console.AppendText(e.StackTrace, "red");
        }
#endif
    }

    private void Update()
    {
#if PLATFORM_ANDROID
        //try
        //{
        //    AndroidJavaObject intent = mActivity.Call<AndroidJavaObject>("getIntent");
        //    string tag = intent.Call<string>("getAction");
        //    AndroidJavaObject mNdefMessage = intent.Call<AndroidJavaObject>("getParcelableExtra", "android.nfc.extra.TAG");

        //    if (mNdefMessage != null)
        //    {
        //        byte[] payLoad = mNdefMessage.Call<byte[]>("getId");
        //        console.AppendText($"This is your tag text: {bytesToHex(payLoad)}");
        //    }
        //    else
        //    {
        //        console.AppendText("No ID found !");
        //    }
        //}
        //catch (System.Exception e)
        //{
        //    console.AppendText(e.StackTrace, "red");
        //}

        try
        {
            AndroidJavaObject intent = mActivity.Call<AndroidJavaObject>("getIntent");
            nfcReader.Call("handleNewIntent", intent);

            byte[] tagBytes = nfcReader.Call<byte[]>("fetchTag");

            if (tagBytes != null)
                console.AppendText($"tag detect : {bytesToHex(tagBytes)}");
        }
        catch (System.Exception e)
        {
            console.AppendText(e.StackTrace, "red");
        }
#endif
    }

    private string bytesToHex(byte[] bytes)
    {
        string hexString = "";

        foreach (byte b in bytes)
            hexString += b.ToString("X2");

        return hexString;
    }
}
