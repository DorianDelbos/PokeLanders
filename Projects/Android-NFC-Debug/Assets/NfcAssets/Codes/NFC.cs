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
			console.AppendText("TAG DISCOVERED", "green");

			// Get the NFC Tag from the intent
			AndroidJavaObject mTag = mIntent.Call<AndroidJavaObject>("getParcelableExtra", "android.nfc.extra.TAG");
			AndroidJavaClass ndef = new AndroidJavaClass("android.nfc.tech.Ndef");
			AndroidJavaObject mNdef = ndef.CallStatic<AndroidJavaObject>("get", mTag);

			mNdef.Call("connect");
		}
		catch (System.Exception e)
		{
			console.AppendText($"Process Error : {e.Message}\r\n{e.StackTrace}", "red");
		}
	}
}