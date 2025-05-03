using dgames.http;
using dgames.nfc;
using Landers.API;
using System.Collections.Generic;
using UnityEngine;

namespace Landopedia
{
    public class MainMenuHandler : MonoBehaviour
    {
        public static MainMenuHandler current;

        [SerializeField] private LanderCase landerCasePrefab;
        [SerializeField] private Transform landerCaseTransform;

        private void Awake()
            => current = this;

        private void Start()
            => Initialize();

        private void ClearLandersCase()
        {
            foreach (Transform child in landerCaseTransform)
                Destroy(child.gameObject);
        }

        private async void Initialize()
        {
            ClearLandersCase();

            List<int> landersSaved = SaveSystem.LoadIDs();

            Lander[] landers = LanderRepository.Instance.GetAll();
            foreach (Lander lander in landers)
            {
                LanderCase instance = Instantiate(landerCasePrefab, landerCaseTransform);
                instance.SetLander(lander);

                instance.HasLander = landersSaved.Contains(lander.id);
                if (instance.HasLander)
                {
                    AsyncOperationWeb<Texture2D> op = WebService.AsyncRequestImage(lander.sprite);

                    await op.AwaitCompletion();

                    if (!op.IsError)
                        instance.SetTexture(op.Result);
                    else
                        Debug.LogError(op.Exception.Message, this);
                }
            }
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

        public void ProcessNfc()
        {
            DataPanel.current.Clear();
            DataPanel.current.SetText("Scan your lander !");
            DataPanel.current.Active = true;

            AsyncOperationNfc operation = NFCSystem.ReadBlock(2, 0, 10000);
            operation.OnComplete += operation =>
            {
                if (!operation.IsError)
                {
                    DataPanel.current.Active = false;
                    short ID = (short)((operation.Result[0] << 8) | (operation.Result[1]));
                    SaveSystem.AddID(ID);
                    Initialize();
                }
                else
                {
                    DataPanel.current.Clear();
                    DataPanel.current.SetText(operation.Exception.Message);
                    DataPanel.current.AddButton("Retry", ProcessNfc);
                    DataPanel.current.AddButton("Stop", () => { DataPanel.current.Active = false; });
                    DataPanel.current.Active = true;
                }
            };
        }

        public void ClearSave()
            => SaveSystem.Clear();
    }
}
