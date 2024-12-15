using UnityEngine;

namespace Landers.API
{
    [CreateAssetMenu(fileName = "ApiSettings", menuName = "API/Settings")]
    public class ApiSettings : ScriptableObject
    {
        private static ApiSettings _instance;

        public static ApiSettings instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = Resources.Load<ApiSettings>("ApiSettings");
                    if (_instance == null)
                        Debug.LogError("PluginSettings asset not found in Resources folder!");
                }
                return _instance;
            }
        }

        [SerializeField] private string apiUrl = "https://landopedia-gwhtbqbqdhd4d5hw.westeurope-01.azurewebsites.net/";
		public string ApiUrl => apiUrl;
	}
}
