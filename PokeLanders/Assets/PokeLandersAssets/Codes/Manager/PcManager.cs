using Lander.Extern;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Lander.Gameplay
{
    public class PcManager : MonoBehaviour
    {
        private LanderData[] LanderData => GameManager.instance.Landers;

        [SerializeField] private LanderMeshDisplayHandler landerDisplayHandler;
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
            LanderData[0] = new LanderData(data, APIDataFetcher<Extern.API.Lander>.FetchData($"api/v1/lander/{data.id}"));
            landerDisplayHandler.SetMesh(LanderData[0].BundleModel);
            pcHudHandler.UpdatePc(LanderData[0], true);
        }

        private void ResetData(LanderDataNFC data)
        {
            LanderData[0] = null;
            landerDisplayHandler.SetMesh(null);
			pcHudHandler.UpdatePc(LanderData[0], false);
		}

        public void HealLander()
        {
            LanderData[0].Hp = LanderData[0].MaxHp;
            pcHudHandler.UpdatePc(LanderData[0], true);
        }

        public void StartTrainLander()
        {
            LanderData[1] = LanderUtils.RandomLander(LanderData[0].Level);
            SceneManager.LoadScene("FightScene");
        }
    }
}
