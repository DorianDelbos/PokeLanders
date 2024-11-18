using Lander.Module.API;
using Lander.Module.Utilities;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Landopedia
{
	public class LanderCase : MonoBehaviour
    {
        [SerializeField] private TMP_Text landerIDTextMesh;
        [SerializeField] private Image landerImage;
        private int landerID = 0;
        private bool hasLander = false;

        public bool HasLander => hasLander;

        public async void Initialize(int id, string spriteUrl)
        {
            landerID = id;
            landerIDTextMesh.text = landerID.ToString("D3");
            landerImage.sprite = await WebSpriteUtilities.LoadSpriteFromUrlAsync(spriteUrl);
        }

        public void SetHasLander(bool hasLander)
        {
            this.hasLander = hasLander;
            landerIDTextMesh.gameObject.SetActive(!hasLander);
            landerImage.gameObject.SetActive(hasLander);
        }

        public void OpenPanelLander()
        {
            if (hasLander)
            {
                MenuManager.current.MenuHandler.ChangeMenu("Loading");
                TryLoadLander();
            }
        }

        private async void TryLoadLander()
        {
            await DataFetcher<Lander.Module.API.Lander>.FetchDataAsync($"api/v1/lander/{landerID}", OnSuccess, ExeptionError);
        }

        private void OnSuccess(Lander.Module.API.Lander lander)
        {
            LanderMenuManager.current.SetLander(lander);
            MenuManager.current.MenuHandler.ChangeMenu("Lander");
        }

        private void ExeptionError()
        {
            DataPanelSystem.Instance.CreateDataPanel(new DataPanelStruct()
			    {
				    text = (Resources.Load("ErrorMessageHandler") as DataMessageHandler).webServiceError,
				    buttons = new List<DataPanelStruct.Button>
				    {
					    new DataPanelStruct.Button
					    {
						    text = "Retry",
						    action = () =>
						    {
							    DataPanelSystem.Instance.ClearDataPanel();
							    TryLoadLander();
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
    }
}
