using Lander.Module.Utilities;
using Lander.Module.API;
using System.Collections.Generic;
using UnityEngine;

namespace Landopedia
{
    public class GameManager : MonoBehaviour
    {
        private static GameManager instance;
        public static GameManager Instance => instance;

        [SerializeField] private Transform landerCaseContent;
        [SerializeField] private LanderCase landerCase;
        private Lander.Module.API.Lander[] landers;

        public Lander.Module.API.Lander[] Landers => landers;

        private void Awake()
        {
            if (instance != null)
            {
                Destroy(gameObject);
                return;
            }

            instance = this;
        }

        private void Start()
        {
            LoadGame();
        }

        private async void LoadGame()
        {
            landers = await DataFetcher<Lander.Module.API.Lander>.FetchArrayDataAsync("api/v1/lander", OnSuccess, ExeptionError);

            //List<int> landerIds = SaveSystem.LoadIDs();
            List<int> landerIds = new List<int>() { 1, 3 };

            foreach (Lander.Module.API.Lander lander in landers)
            {
                LanderCase landerCaseInstance = Instantiate(landerCase, landerCaseContent);

                landerCaseInstance.Initialize(lander.id, await WebSpriteUtilities.LoadSpriteFromUrlAsync(lander.sprite));
                landerCaseInstance.SetHasLander(landerIds.Contains(lander.id));
            }
        }

        private void OnSuccess(Lander.Module.API.Lander[] landers)
        {
            MenuManager.current.MenuHandler.ChangeMenu("main");
        }

        private void ExeptionError()
        {
            DataPanelStruct dataPanelStruct = new DataPanelStruct()
            {
                text = (Resources.Load("ErrorMessageHandler") as ErrorMessageHandler).webServiceError,
                buttons = new List<DataPanelStruct.Button>
                {
                    new DataPanelStruct.Button
                    {
                        text = "Retry",
                        action = () =>
                        {
                            DataPanelSystem.Instance.ClearDataPanel();
                            LoadGame();
                        }
                    },
                    new DataPanelStruct.Button
                    {
                        text = "Quit application",
                        action = () => MenuManager.current.QuitApplication()
                    },
                }
            };

            DataPanelSystem.Instance.CreateDataPanel(dataPanelStruct);
        }
    }
}
