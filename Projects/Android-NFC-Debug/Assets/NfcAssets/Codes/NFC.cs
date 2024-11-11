using UnityEngine;

public class NFC : MonoBehaviour
{
    private Console console => Console.current;

    private AndroidJavaObject mActivity;
    private AndroidJavaObject mIntent;
    private string sAction;

    private void Update() 
	{
		if (Application.platform == RuntimePlatform.Android) 
		{
			try 
			{
                mActivity = new AndroidJavaClass("com.unity3d.player.UnityPlayer").GetStatic<AndroidJavaObject>("currentActivity");
                mIntent = mActivity.Call<AndroidJavaObject>("getIntent");
                sAction = mIntent.Call<System.String>("getAction");

				switch (sAction)
				{
					case "android.nfc.action.NDEF_DISCOVERED":
                        console.AppendText("Tag of type NDEF", "yellow");
                        break;
                    case "android.nfc.action.TECH_DISCOVERED":
                        console.AppendText("This type of tag is not supported !", "yellow");
                        break;
					case "android.nfc.action.TAG_DISCOVERED":
                        ProcessNFC();
                        break;
                    default:
                        console.AppendText(sAction, "yellow");
						break;
                };
			}
			catch (System.Exception ex) 
			{
                console.AppendText(ex.Message, "red");
			}
		}
	}

    private void ProcessNFC()
    {
        try
        {
            // Get the current Android activity
            AndroidJavaObject mActivity = new AndroidJavaClass("com.unity3d.player.UnityPlayer").GetStatic<AndroidJavaObject>("currentActivity");

            // Get the NFC tag from the intent
            AndroidJavaObject mTag = mIntent.Call<AndroidJavaObject>("getParcelableExtra", "android.nfc.extra.TAG");

            // If the tag is null, exit
            if (mTag == null)
            {
                console.AppendText("No NFC tag detected!", "red");
                return;
            }

            // Get the NfcAdapter from the current activity (used to interact with NFC hardware)
            AndroidJavaObject mNfcAdapter = new AndroidJavaClass("android.nfc.NfcAdapter").CallStatic<AndroidJavaObject>("getDefaultAdapter", mActivity);

            if (mNfcAdapter == null)
            {
                console.AppendText("NFC is not available on this device!", "red");
                return;
            }

            console.AppendText("Step 1", "green");

            // Check if the tag contains NDEF messages
            AndroidJavaObject mNdef = new AndroidJavaClass("android.nfc.Ndef").CallStatic<AndroidJavaObject>("get", mTag);

            console.AppendText("Step 2", "green");

            if (mNdef != null)
            {
                // If the tag contains NDEF messages, read them
                AndroidJavaObject mNdefMessage = mNdef.Call<AndroidJavaObject>("getNdefMessage");

                // Get the NDEF records (these contain the actual data)
                AndroidJavaObject[] mNdefRecords = mNdefMessage.Call<AndroidJavaObject[]>("getRecords");

                foreach (AndroidJavaObject mRecord in mNdefRecords)
                {
                    // Extract NDEF record data (usually text or URI)
                    string recordType = mRecord.Call<string>("getTnf");
                    string recordPayload = mRecord.Call<string>("getPayload");

                    // For simplicity, assume the data is plain text for now
                    if (recordType == "TNF_WELL_KNOWN")
                    {
                        string decodedData = System.Text.Encoding.UTF8.GetString(System.Convert.FromBase64String(recordPayload));
                        console.AppendText("NDEF Data: " + decodedData, "green");
                    }
                    else
                    {
                        console.AppendText("Unknown NDEF Record: " + recordPayload, "yellow");
                    }
                }

                console.AppendText("Step 3", "green");
            }
            else
            {
                console.AppendText("NFC tag does not contain an NDEF message!", "red");
            }
        }
        catch (System.Exception ex)
        {
            //console.AppendText("Error processing NFC: " + ex.Message, "red");
            //console.AppendText("Stack Trace: " + ex.StackTrace, "red");
        }
    }
}