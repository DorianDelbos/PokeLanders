using Lander.Extern;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Lander.Gameplay
{
    public class PcManager : MonoBehaviour
    {
        private LanderData LanderData { get => GameManager.instance.Landers[0]; set => GameManager.instance.Landers[0] = value; }

        [SerializeField] private LanderDisplayHandler landerDisplayHandler;
        [SerializeField] private PcHudHandler pcHudHandler;

        private void OnEnable()
        {
            NfcRequests.onNewNfcDetect += SetData;
            NfcRequests.onNfcRemove += ResetData;
        }

        private void OnDisable()
        {
            NfcRequests.onNewNfcDetect -= SetData;
            NfcRequests.onNfcRemove -= ResetData;
        }

        private void SetData(LanderDataNFC data)
        {
            LanderData = new LanderData(data, APIDataFetcher<Extern.API.Lander>.FetchData($"api/v1/lander/{data.id}"));
            landerDisplayHandler.SetMesh(LanderData.BundleModel);
            pcHudHandler.UpdatePc(LanderData, true);
        }

        private void ResetData(LanderDataNFC data)
        {
            LanderData = null;
            landerDisplayHandler.SetMesh(null);
			pcHudHandler.UpdatePc(LanderData, false);
		}

        public void StartTrainLander()
        {
            GameManager.instance.Landers[1] = LanderUtils.RandomLander(LanderData.Level);
            SceneManager.LoadScene("FightScene");
        }
    }
}
