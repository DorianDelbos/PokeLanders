using Dgames.Extern.API;
using Dgames.Extern;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;

namespace Landopedia
{
    public class LanderCase : MonoBehaviour
    {
        [SerializeField] private TMP_Text landerIDTextMesh;
        [SerializeField] private Image landerImage;
        private int landerID = 0;
        private bool hasLander = false;

        public bool HasLander => hasLander;

        public void Initialize(int id, Sprite sprite)
        {
            landerID = id;
            landerIDTextMesh.text = landerID.ToString("D3");
            landerImage.sprite = sprite;
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
            await DataFetcher<Lander>.FetchDataAsync($"api/v1/lander/{landerID}", OnSuccess, ExeptionError);
        }

        private void OnSuccess(Lander lander)
        {
            LanderMenuManager.current.SetLander(lander);
            MenuManager.current.MenuHandler.ChangeMenu("Lander");
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
                        TryLoadLander();
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
