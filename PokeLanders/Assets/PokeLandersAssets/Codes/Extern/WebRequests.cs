//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.Networking;

//namespace Lander.Gameplay.Web
//{
//	public class WebRequests : MonoBehaviour
//	{
//		public static WebRequests instance;
//		public string website = "http://localhost/Pokelanders";

//		private void Awake()
//		{
//			if (instance == null)
//			{
//				instance = this;
//				transform.SetParent(null);
//				DontDestroyOnLoad(gameObject);
//			}
//			else
//			{
//				Destroy(gameObject);
//				return;
//			}
//		}

//		private Dictionary<string, string> ProcessWebRequest(UnityWebRequest www, out string error)
//		{
//			error = null;

//			if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
//			{
//				Debug.LogError(www.error);
//				error = www.error;
//				return null;
//			}

//			Dictionary<string, string> dict = new Dictionary<string, string>();
//			string res = www.downloadHandler.text;

//			if (string.IsNullOrWhiteSpace(res))
//			{
//				error = "Empty response";
//				return null;
//			}

//			string[] rows = res.Split(';');
//			foreach (string row in rows)
//			{
//				string[] cols = row.Split('=');
//				if (cols.Length == 2)
//				{
//					dict.Add(cols[0].Trim(), cols[1].Trim());
//				}
//			}

//			return dict;
//		}

//		private IEnumerator GetRequestAsync(string request, System.Action<Dictionary<string, string>, string> callback)
//		{
//			using (UnityWebRequest www = UnityWebRequest.Get($"{website}/{request}"))
//			{
//				yield return www.SendWebRequest();

//				string error;
//				Dictionary<string, string> dict = ProcessWebRequest(www, out error);

//				callback(dict, error);
//			}
//		}

//		public void DoRequest(string request, System.Action<Dictionary<string, string>, string> callback)
//		{
//			using (UnityWebRequest www = UnityWebRequest.Get($"{website}/{request}"))
//			{
//				www.SendWebRequest();

//				while (!www.isDone) { }

//				string error;
//				Dictionary<string, string> dict = ProcessWebRequest(www, out error);

//				callback(dict, error);
//			}
//		}

//		public void DoRequestAsync(string request, System.Action<Dictionary<string, string>, string> callback)
//		{
//			StartCoroutine(GetRequestAsync(request, callback));
//		}
//	}
//}
