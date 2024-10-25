using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace Lander.Extern
{
	using BundleAssetsLoad = Dictionary<Type, List<UnityEngine.Object>>;

	public static class BundleLoaderUtils
	{
		public static async Task<BundleAssetsLoad> DownloadAssetsAsync(string apiUrl)
		{
			using (UnityWebRequest www = UnityWebRequestAssetBundle.GetAssetBundle(apiUrl))
			{
				var operation = www.SendWebRequest();
				await operation;

				if (www.result != UnityWebRequest.Result.Success)
					throw new Exception($"Error downloading asset bundle: {www.error}");

				AssetBundle bundle = DownloadHandlerAssetBundle.GetContent(www);
				BundleAssetsLoad result = await LoadAssetsAsync(bundle, bundle.GetAllAssetNames());

				bundle.Unload(false);

				return result;
			}
		}

		public static BundleAssetsLoad DownloadAssets(string apiUrl)
		{
			using (UnityWebRequest www = UnityWebRequestAssetBundle.GetAssetBundle(apiUrl))
			{
				var operation = www.SendWebRequest();
				while (!operation.isDone) { }

				if (www.result != UnityWebRequest.Result.Success)
					throw new Exception($"Error downloading asset bundle: {www.error}");

				AssetBundle bundle = DownloadHandlerAssetBundle.GetContent(www);
				BundleAssetsLoad result = LoadAssets(bundle, bundle.GetAllAssetNames());

				bundle.Unload(false); // Unload the asset bundle but keep loaded assets in memory

				return result;
			}
		}

		private static async Task<BundleAssetsLoad> LoadAssetsAsync(AssetBundle bundle, string[] assetsName)
		{
			BundleAssetsLoad dict = new BundleAssetsLoad();

			foreach (string assetName in assetsName)
			{
				string extension = System.IO.Path.GetExtension(assetName).ToLower();
				UnityEngine.Object asset = null;

				switch (extension)
				{
					case ".mat":
						asset = await LoadAssetAsync<Material>(bundle, assetName);
						AddToDictionary(dict, typeof(Material), asset);
						break;

					case ".fbx":
					case ".obj":
						asset = await LoadAssetAsync<Mesh>(bundle, assetName);
						AddToDictionary(dict, typeof(Mesh), asset);
						break;

					default:
						Debug.LogWarning($"{assetName} is unknown.");
						break;
				}
			}

			return dict;
		}

		private static BundleAssetsLoad LoadAssets(AssetBundle bundle, string[] assetsName)
		{
			BundleAssetsLoad dict = new BundleAssetsLoad();

			foreach (string assetName in assetsName)
			{
				string extension = System.IO.Path.GetExtension(assetName).ToLower();
				UnityEngine.Object asset = null;

				switch (extension)
				{
					case ".mat":
						asset = bundle.LoadAsset<Material>(assetName);
						AddToDictionary(dict, typeof(Material), asset);
						break;

					case ".fbx":
					case ".obj":
						asset = bundle.LoadAsset<Mesh>(assetName);
						AddToDictionary(dict, typeof(Mesh), asset);
						break;

					default:
						Debug.LogWarning($"{assetName} is unknown.");
						break;
				}
			}

			return dict;
		}

		private static async Task<T> LoadAssetAsync<T>(AssetBundle bundle, string assetName) where T : UnityEngine.Object
		{
			var operation = bundle.LoadAssetAsync<T>(assetName);
			await operation;
			return operation.asset as T;
		}

		private static void AddToDictionary(BundleAssetsLoad dict, Type type, UnityEngine.Object asset)
		{
			if (asset != null)
			{
				if (!dict.ContainsKey(type))
				{
					dict[type] = new List<UnityEngine.Object>();
				}
				dict[type].Add(asset);
			}
		}
	}
}
