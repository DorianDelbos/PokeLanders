using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.ResourceManagement.AsyncOperations;

public class ModelLoader : MonoBehaviour
{
	private string APIUrl = "https://localhost:7041/api/v1/bundle/aquapix_assets_all_0448fee7960074ab87b2f3060e479d3e.bundle";
	public string assetName = "aquapix_assets_all_0448fee7960074ab87b2f3060e479d3e.bundle";
	private GameObject instanceReference;
	private AsyncOperationHandle m_Handle;

	private void Update()
	{
		if (Input.GetKeyUp(KeyCode.I))
			StartCoroutine(DownloadAndLoadAsset());
		else if (Input.GetKeyUp(KeyCode.D))
			ReleaseInstance();
	}

	private IEnumerator DownloadAndLoadAsset()
	{
		using (UnityWebRequest www = UnityWebRequestAssetBundle.GetAssetBundle(APIUrl))
		{
			// Send the request and wait for a response
			yield return www.SendWebRequest();

			// Handle any errors that may have occurred
			if (www.result != UnityWebRequest.Result.Success)
			{
				Debug.LogError("Error downloading asset bundle: " + www.error);
				yield break; // Exit the coroutine if there was an error
			}

			// Get the downloaded asset bundle
			AssetBundle bundle = DownloadHandlerAssetBundle.GetContent(www);

			// Load the specific asset from the bundle
			AssetBundleRequest request = bundle.LoadAssetAsync<GameObject>(assetName);
			string[] debugS = bundle.GetAllAssetNames();
			
			yield return request;

			// Check if the loading was successful
			if (request.asset != null)
			{
				instanceReference = Instantiate(request.asset as GameObject); // Instantiate the loaded asset
			}
			else
			{
				Debug.LogError("Failed to load asset: " + assetName);
			}

			// Optionally, you can unload the bundle if you no longer need it
			// bundle.Unload(false); // Unload the asset bundle but keep the loaded assets in memory
		}
	}

	private void ReleaseInstance()
	{
		// Check if there is an instance to release
		if (instanceReference != null)
		{
			Destroy(instanceReference); // Destroy the GameObject
			instanceReference = null; // Clear the reference after releasing
		}
	}
}


//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.AddressableAssets;
//using UnityEngine.Events;
//using UnityEngine.ResourceManagement.AsyncOperations;
//using UnityEngine.ResourceManagement.ResourceLocations;

//internal class LoadWithLocation : MonoBehaviour
//{
//	public Dictionary<string, AsyncOperationHandle<GameObject>> operationDictionary;
//	public List<string> keys;
//	public UnityEvent Ready;

//	IEnumerator LoadAndAssociateResultWithKey(IList<string> keys)
//	{
//		if (operationDictionary == null)
//			operationDictionary = new Dictionary<string, AsyncOperationHandle<GameObject>>();

//		AsyncOperationHandle<IList<IResourceLocation>> locations
//			= Addressables.LoadResourceLocationsAsync(keys,
//				Addressables.MergeMode.Union, typeof(GameObject));

//		yield return locations;

//		var loadOps = new List<AsyncOperationHandle>(locations.Result.Count);

//		foreach (IResourceLocation location in locations.Result)
//		{
//			AsyncOperationHandle<GameObject> handle =
//				Addressables.LoadAssetAsync<GameObject>(location);
//			handle.Completed += obj => operationDictionary.Add(location.PrimaryKey, obj);
//			loadOps.Add(handle);
//		}

//		yield return Addressables.ResourceManager.CreateGenericGroupOperation(loadOps, true);

//		Ready.Invoke();
//	}

//	void Start()
//	{
//		Ready.AddListener(OnAssetsReady);
//		StartCoroutine(LoadAndAssociateResultWithKey(keys));
//	}

//	private void OnAssetsReady()
//	{
//		float x = 0, z = 0;
//		foreach (var item in operationDictionary)
//		{
//			Debug.Log($"{item.Key} = {item.Value.Result.name}");
//			Instantiate(item.Value.Result,
//				new Vector3(x++ * 2.0f, 0, z * 2.0f),
//				Quaternion.identity, transform);
//			if (x > 9)
//			{
//				x = 0;
//				z++;
//			}
//		}
//	}

//	private void OnDestroy()
//	{
//		foreach (var item in operationDictionary)
//		{
//			Addressables.Release(item.Value);
//		}
//	}
//}
