using dgames.Tasks;
using Landers;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Landopedia
{
    public class SplashMenuHandler : MonoBehaviour
    {
        private TaskManager m_taskManager;

        private void Awake()
        {
            m_taskManager ??= new TaskManager();
            m_taskManager.EnqueueTask(new AilmentInitializeTask());
            m_taskManager.EnqueueTask(new EvolutionChainInitializeTask());
            m_taskManager.EnqueueTask(new LanderInitializeTask());
            m_taskManager.EnqueueTask(new MoveInitializeTask());
            m_taskManager.EnqueueTask(new NatureInitializeTask());
            m_taskManager.EnqueueTask(new StatInitializeTask());
            m_taskManager.EnqueueTask(new TypeInitializeTask());

            LoadRepository();
        }

        public async void LoadRepository()
        {
            await m_taskManager.StartProcessingAsync();

            if (m_taskManager.GetTaskStackCount() > 0)
            {
                HandleError(new Exception("An error occurred while retrieving data from the API. Please try again later or contact support if the issue persists."));
            }
            else
            {
                SceneManager.LoadScene("Main");
            }
        }

        private void HandleError(Exception e)
        {
            DataPanel.current.Clear();
            DataPanel.current.SetText($"{e.Message}");
            DataPanel.current.AddButton("Retry", () =>
            {
                LoadRepository();
                DataPanel.current.Active = false;
            });
            DataPanel.current.Active = true;
        }
    }
}
