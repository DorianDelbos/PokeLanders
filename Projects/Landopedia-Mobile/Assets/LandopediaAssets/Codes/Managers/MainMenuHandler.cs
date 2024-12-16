using dgames.http;
using dgames.nfc;
using dgames.Utilities;
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

        private void Initialize()
        {
            ClearLandersCase();
            
            List<int> landersSaved = SaveSystem.LoadIDs();

            Lander[] landers = LanderRepository.GetAll();
            foreach (Lander lander in landers)
            {
                LanderCase instance = Instantiate(landerCasePrefab, landerCaseTransform);
                instance.SetLander(lander);

                instance.HasLander = landersSaved.Contains(lander.id);
                if (instance.HasLander)
                {
                    AsyncOperationWeb<Texture2D> op = WebService.AsyncRequestImage(lander.sprite);
					op.OnComplete += op =>
					{
                        if (op.Exception != null)
                            instance.SetTexture(op.Result);
                        else
                            Debug.LogError(op.Exception.Message, this);
                    };
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

            NFCSystem NFCSystem = new NFCSystem() { timeout = 10000 };
            NFCSystem.ReadBlockAsync(2, 0, (isSucceed, result, e) =>
            {
                if (isSucceed)
                {
                    DataPanel.current.Active = false;
                    short ID = (short)((result[0] << 8) | (result[1]));
                    SaveSystem.AddID(ID);
                    Initialize();
                }
                else
                {
                    DataPanel.current.Clear();
                    DataPanel.current.SetText(e.Message);
                    DataPanel.current.AddButton("Retry", ProcessNfc);
                    DataPanel.current.AddButton("Stop", () => { DataPanel.current.Active = false; });
                    DataPanel.current.Active = true;
                }
            });
        }

        public void ClearSave()
            => SaveSystem.Clear();
    }
}
