using LandersLegends.Extern;
using UnityEngine;
using LandersLegends.Gameplay;
using GLTFast;

namespace LandersLegends.Battle
{
	public class FightSceneManager : MonoBehaviour
	{
		private string[] tagRegisters;
        [SerializeField] private GltfAsset[] gltfAssets = new GltfAsset[2];

        private Lander[] landerData => GameManager.instance.Landers;
		private NfcErrorHudManager nfcErrorHud => NfcErrorHudManager.current;

		private void Awake()
		{
			int lenghtLander = landerData.Length;
			tagRegisters = new string[lenghtLander];
			for (int i = 0; i < lenghtLander; i++)
			{
				tagRegisters[i] = landerData[i].Tag;
			}
		}

		private void OnEnable()
		{
			ExternLanderManager.onLanderDetect += CheckData;
			ExternLanderManager.onLanderRemove += OnNfcRemove;
		}

		private void OnDisable()
		{
			ExternLanderManager.onLanderDetect -= CheckData;
			ExternLanderManager.onLanderRemove -= OnNfcRemove;
		}

        private async void Start()
        {
            for (int i = 0; i < landerData.Length; ++i)
            {
				await gltfAssets[i].Load(landerData[i].ModelUrl);
			}
        }

        private void DisplayNfcError(bool isError, string errorText)
		{
			Time.timeScale = (isError ? 0.0f : 1.0f);
			nfcErrorHud.SetActive(isError);
			nfcErrorHud.SetErrorText(errorText);
		}

		private void OnNfcRemove(LanderDataNFC data) => DisplayNfcError(true, $"You must replace {data.name} on the player to continue !");

		private void CheckData(LanderDataNFC data)
		{
			if (!CheckIfSameTag(data))
			{
				DisplayNfcError(true, $"Wrong Lander detected, make sure it's the same card used !");
				return;
			}

			DisplayNfcError(false, string.Empty);
        }

		private bool CheckIfSameTag(LanderDataNFC data)
		{
			for (int i = 0; i < landerData.Length; i++)
			{
				if (tagRegisters[i] == data.tag)
				{
					return true;
				}
			}

			return false;
		}
	}
}