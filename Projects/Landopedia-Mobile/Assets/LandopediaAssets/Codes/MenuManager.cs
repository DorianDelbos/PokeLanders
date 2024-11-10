using UnityEngine;

namespace Landopedia
{
    public class MenuManager : MonoBehaviour
    {
        private static MenuManager instance;
        public static MenuManager current => instance;

        private MenuHandler menuHandler;
        public MenuHandler MenuHandler => menuHandler;

        private void Awake()
        {
            instance = this;
            menuHandler = GetComponent<MenuHandler>();
        }

        public void QuitApplication()
        {
#if UNITY_EDITOR
            if (UnityEditor.EditorApplication.isPlaying)
                UnityEditor.EditorApplication.isPlaying = false;
            else
#endif
                Application.Quit();
        }
    }
}
