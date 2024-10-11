using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using static UnityEngine.Rendering.DebugUI.Table;

public class WebRequests : MonoBehaviour
{
    public static WebRequests instance;
    public string website = "http://localhost/Pokelanders";

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            transform.SetParent(null);
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private IEnumerator GetRequest(string request, System.Action<Dictionary<string, string>> callback)
    {
        using (UnityWebRequest www = UnityWebRequest.Get($"{website}/{request}"))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError(www.error);
                callback(null);
            }
            else
            {
                Dictionary<string, string> dict = new Dictionary<string, string>();
                string res = www.downloadHandler.text;
                string[] rows = res.Split(";");
                foreach (string row in rows)
                {
                    string[] cols = row.Split("=");
                    dict.Add(cols[0], cols[1]);
                }

                // Call the callback with the result
                callback(dict);
            }
        }
    }

    public void DoRequest(string request, System.Action<Dictionary<string, string>> callback)
    {
        StartCoroutine(GetRequest(request, callback));
    }
}
