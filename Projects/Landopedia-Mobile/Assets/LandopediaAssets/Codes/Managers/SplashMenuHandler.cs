using Landers.API;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Landopedia
{
    public class SplashMenuHandler : MonoBehaviour
    {
        private void Awake()
        {
            InitializeLanderRepository();
        }

        private void InitializeLanderRepository()
        {
            LanderRepository.Initialize().OnComplete += op =>
            {
                if (op.IsError) HandleError(op.Exception);
                else InitializeTypeRepository();
            };
        }

        private void InitializeTypeRepository()
        {
            TypeRepository.Initialize().OnComplete += op =>
            {
                if (op.IsError) HandleError(op.Exception);
                else InitializeMoveRepository();
            };
        }

        private void InitializeMoveRepository()
        {
            MoveRepository.Initialize().OnComplete += op =>
            {
                if (op.IsError) HandleError(op.Exception);
                else SceneManager.LoadScene("Main");
            };
        }

        private void HandleError(Exception e)
        {
            DataPanel.current.Clear();
            DataPanel.current.SetText($"{e.Message}");
            DataPanel.current.AddButton("Retry", () =>
            {
                InitializeLanderRepository();
                DataPanel.current.Active = false;
            });
            DataPanel.current.Active = true;
        }
    }
}
