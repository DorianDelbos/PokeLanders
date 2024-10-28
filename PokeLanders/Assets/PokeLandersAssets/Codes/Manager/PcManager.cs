using LandersLegends.Extern;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace LandersLegends.Gameplay
{
    public class PcManager : MonoBehaviour
    {
        private Lander[] LanderData => GameManager.instance.Landers;

        [SerializeField] private LanderMeshDisplayHandler landerDisplayHandler;
        [SerializeField] private PcHudHandler pcHudHandler;

        private void OnEnable()
        {
            ExternLanderManager.onLanderDetect += SetData;
			ExternLanderManager.onLanderRemove += ResetData;
        }

        private void OnDisable()
        {
			ExternLanderManager.onLanderDetect -= SetData;
			ExternLanderManager.onLanderRemove -= ResetData;
        }

        private void SetData(LanderDataNFC data)
        {
            LanderData[0] = new Lander(data, APIDataFetcher<Extern.API.Lander>.FetchData($"api/v1/lander/{data.id}"));
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
