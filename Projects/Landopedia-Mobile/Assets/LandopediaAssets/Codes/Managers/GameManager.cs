using Lander.Module.Utilities;
using Lander.Module.API;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

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
			NFC.onBlocksRead += blocks =>
			{
				short ID = (short)(
					(blocks[1][0] << 8) |
					(blocks[1][1])
				);

				SaveSystem.AddID(ID);
				UpdateLanderDisplay(SaveSystem.LoadIDs());

				LanderMenuManager.current.SetLander(landers.FirstOrDefault(x => x.id == ID));
				MenuManager.current.MenuHandler.ChangeMenu("Lander");
			};
		}

		public void ResetData()
        {
            DataPanelSystem.Instance.CreateDataPanel(new DataPanelStruct()
            {
                text = DataPanelSystem.ErrorMessageHandler.resetTextAlert,
                buttons = new List<DataPanelStruct.Button>
                    {
                        new DataPanelStruct.Button
                        {
                            text = "No",
                            action = () =>
                            {
                                DataPanelSystem.Instance.ClearDataPanel();
                            }
                        },
                        new DataPanelStruct.Button
                        {
                            text = "Yes",
                            action = () =>
                            {
                                DataPanelSystem.Instance.ClearDataPanel();
                                SaveSystem.Clear();
								UpdateLanderDisplay(SaveSystem.LoadIDs());
                            }
                        },
                    }
				}
            );
		}

		private async void LoadGame()
		{
			landers = await DataFetcher<Lander.Module.API.Lander>.FetchArrayDataAsync(
				"api/v1/lander", 
				lander =>
				{
                    MenuManager.current.MenuHandler.ChangeMenu("main");
                }, 
				() =>
                {
                    DataPanelSystem.Instance.CreateDataPanel(new DataPanelStruct()
						{
							text = DataPanelSystem.ErrorMessageHandler.webServiceError,
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
						}
                    );
                }
			);
			UpdateLanderDisplay(SaveSystem.LoadIDs());
		}

		private void UpdateLanderDisplay(List<int> landerIds)
		{
			// Clear
			foreach (Transform child in landerCaseContent)
			{
				Destroy(child.gameObject);
			}

			// Set
			foreach (Lander.Module.API.Lander lander in landers)
			{
				LanderCase landerCaseInstance = Instantiate(landerCase, landerCaseContent);

				landerCaseInstance.Initialize(lander.id, lander.sprite);
				landerCaseInstance.SetHasLander(landerIds.Contains(lander.id));
			}
		}
	}
}
