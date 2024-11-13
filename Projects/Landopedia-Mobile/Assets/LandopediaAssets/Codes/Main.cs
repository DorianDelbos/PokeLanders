using UnityEngine;

public class Main : MonoBehaviour
{
	string qrString = "";
	bool background = true;

	void Start()
	{
		AndroidNFCReader.enableBackgroundScan ();
		AndroidNFCReader.ScanNFC (gameObject.name, "OnFinishScan");
	}

	void OnGUI ()
	{
		if (!background) 
		{
			if (GUI.Button (new Rect (0, Screen.height - 50, Screen.width, 50), "Scan NFC", new GUIStyle { fontSize = 32 })) 
			{
				AndroidNFCReader.ScanNFC (gameObject.name, "OnFinishScan");
			}

			if (GUI.Button (new Rect (0, Screen.height - 100, Screen.width, 50), "Enable Backgraound Mode", new GUIStyle { fontSize = 32 })) 
			{
				AndroidNFCReader.enableBackgroundScan ();
				background = true;
			}
		}
		else
		{
			if (GUI.Button (new Rect (0, Screen.height - 50, Screen.width, 50), "Disable Backgraound Mode")) 
			{
				AndroidNFCReader.disableBackgroundScan ();
				background = false;
			}
		}
		GUI.Label (new Rect (0, 0, Screen.width, 50), $"Result: {qrString}\r\nBackground:{background}", new GUIStyle { fontSize = 32 });
	}

	void OnFinishScan (string result)
	{

		if (result == AndroidNFCReader.CANCELLED) 
		{
			qrString = "CANCELLED";
		} 
		else if (result == AndroidNFCReader.ERROR)
		{
			qrString = "ERROR";
		} 
		else if (result == AndroidNFCReader.NO_HARDWARE)
		{
			qrString = "NO HARDWARE";
		}
		else
        {
            qrString = getToyxFromUrl(result);
        }
	}

	string getToyxFromUrl (string url)
	{		
		int index = url.LastIndexOf ('/') + 1;
		
		if (url.Length > index) 
		{
			return url.Substring (index);		
		} 
		
		return url;
	}
}
