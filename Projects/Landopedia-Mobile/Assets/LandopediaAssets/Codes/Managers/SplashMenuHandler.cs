using Landers.API;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Landopedia
{
    public class SplashMenuHandler : MonoBehaviour
    {
        private int currentSaved = 0;
        private const int maxSaved = 3;

        private void Awake()
        {
            LanderRepository.Initialize(InitializeCompleted);
            TypeRepository.Initialize(InitializeCompleted);
            MoveRepository.Initialize(InitializeCompleted);
        }

        private void InitializeCompleted<T>(bool isSucceed, T[] result, System.Exception e)
        {
            if (isSucceed)
            {
                if (++currentSaved >= maxSaved)
                    SceneManager.LoadScene("Main");
            }
            else
            {
                DataPanel.current.Clear();
                DataPanel.current.SetText(e.Message);
                DataPanel.current.AddButton("Retry", () =>
                {
                    LanderRepository.Initialize(InitializeCompleted);
                    DataPanel.current.Active = false;
                });
                DataPanel.current.Active = true;
            }
        }
    }
}
