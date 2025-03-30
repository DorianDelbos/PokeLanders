using dgames.Tasks;
using dgames.Utils;
using GLTFast;
using Landers;
using System;

namespace LanderFighter
{
    public class TaskHandler : BaseUnitySingleton<TaskHandler>
    {
        private LoadingScreenHandler m_loadingScreenHandler => LoadingScreenHandler.Instance;
        private TaskManager m_taskManager;
        private bool m_isLoaded;

        private event Action m_onRepositoryLoaded;
        public event Action OnRepositoryLoaded
        {
            add
            {
                // Add event handler
                m_onRepositoryLoaded += value;

                // If the repository is already loaded, invoke the event for the new subscriber
                if (m_isLoaded)
                {
                    value?.Invoke();
                }
            }
            remove
            {
                // Remove event handler
                m_onRepositoryLoaded -= value;
            }
        }

        private void Awake()
        {
            TryInitializeInstance();
        }

        private void Start()
        {
            LoadRepository();
        }

        public async void LoadRepository()
        {
            m_loadingScreenHandler.Enable = true;
            m_taskManager ??= new TaskManager();

            m_loadingScreenHandler.Text = "Load lander ailments from API ...";
            await m_taskManager.RunTaskAsync(new AilmentInitializeTask());
            m_loadingScreenHandler.Text = "Load lander evolution chains from API ...";
            await m_taskManager.RunTaskAsync(new EvolutionChainInitializeTask());
            m_loadingScreenHandler.Text = "Load lander landers from API ...";
            await m_taskManager.RunTaskAsync(new LanderInitializeTask());
            m_loadingScreenHandler.Text = "Load lander moves from API ...";
            await m_taskManager.RunTaskAsync(new MoveInitializeTask());
            m_loadingScreenHandler.Text = "Load lander natures from API ...";
            await m_taskManager.RunTaskAsync(new NatureInitializeTask());
            m_loadingScreenHandler.Text = "Load lander stats from API ...";
            await m_taskManager.RunTaskAsync(new StatInitializeTask());
            m_loadingScreenHandler.Text = "Load lander types from API ...";
            await m_taskManager.RunTaskAsync(new TypeInitializeTask());

            m_loadingScreenHandler.Enable = false;
            m_isLoaded = true;
            m_onRepositoryLoaded?.Invoke();
        }

        public async void LoadModel3D(GltfAsset gltfAsset, string url)
        {
            m_loadingScreenHandler.Enable = true;

            m_loadingScreenHandler.Text = "Load lander 3D model from API ...";
            await gltfAsset.Load(url);

            m_loadingScreenHandler.Enable = false;
        }
    }
}
