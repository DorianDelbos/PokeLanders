using UnityEditor;
using UnityEngine;

namespace Landers.API
{
    [CreateAssetMenu(fileName = "ApiSettings", menuName = "API/Settings")]
    public class ApiSettings : ScriptableObject
	{
		private void OnEnable()
		{
			Initialize();
		}

#if UNITY_EDITOR
		private void OnValidate()
		{
			Initialize();
		}
#endif

		private void Initialize()
		{
			if (_i && _i != this)
			{
				AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(this));
				return;
			}

			_i = this;
		}

		private static ApiSettings _i;
		public static ApiSettings instance => _i;

		[SerializeField] private string apiUrl = "https://landopedia-gwhtbqbqdhd4d5hw.westeurope-01.azurewebsites.net/";
		public string ApiUrl => apiUrl;
	}
}
