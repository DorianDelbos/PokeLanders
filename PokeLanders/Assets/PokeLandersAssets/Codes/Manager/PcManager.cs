using GLTFast;
using LandersLegends.Extern;
using LandersLegends.Extern.API;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace LandersLegends.Gameplay
{
    public class PcManager : MonoBehaviour
    {
        private Lander[] LanderData => GameManager.instance.Landers;

        [SerializeField] private GltfAsset gltfAsset;
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

        private async void SetData(LanderDataNFC data)
        {
            LanderData[0] = new Lander(data, LanderRepository.GetById(data.id));
            await gltfAsset.Load(LanderData[0].ModelUrl);
			pcHudHandler.UpdatePc(LanderData[0], true);
        }

        private void ResetData(LanderDataNFC data)
        {
            LanderData[0] = null;
			gltfAsset.ClearScenes();
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
