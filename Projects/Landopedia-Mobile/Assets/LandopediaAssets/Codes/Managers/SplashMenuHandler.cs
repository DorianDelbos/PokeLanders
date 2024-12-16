using dgames.http;
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
            AsyncOperationWeb<Lander[]> opLander = LanderRepository.Initialize();
            AsyncOperationWeb<Type[]> opType = TypeRepository.Initialize();
            AsyncOperationWeb<Move[]> opMove = MoveRepository.Initialize();

            opLander.OnComplete += InitializeCompleted;
			opType.OnComplete += InitializeCompleted;
			opMove.OnComplete += InitializeCompleted;
		}

        private void InitializeCompleted<T>(AsyncOperationWeb<T> op)
        {
            if (op.Exception == null)
            {
                if (++currentSaved >= maxSaved)
                    SceneManager.LoadScene("Main");
            }
            else
            {
                DataPanel.current.Clear();
                DataPanel.current.SetText(op.Exception.Message);
                DataPanel.current.AddButton("Retry", () =>
                {
                    LanderRepository.Initialize().OnComplete += InitializeCompleted;
                    DataPanel.current.Active = false;
                });
                DataPanel.current.Active = true;
            }
        }
    }
}
