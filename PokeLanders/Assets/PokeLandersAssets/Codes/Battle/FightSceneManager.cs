using Lander.Extern;
using UnityEngine;
using Lander.Gameplay;

namespace Lander.Battle
{
	public class FightSceneManager : MonoBehaviour
	{
		private string[] tagRegisters;
        [SerializeField] private LanderMeshDisplayHandler[] landerDisplayHandler = new LanderMeshDisplayHandler[2];

        private LanderData[] landerData => GameManager.instance.Landers;
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
			NfcRequests.onNewNfcDetect += CheckData;
			NfcRequests.onNfcRemove += OnNfcRemove;
		}

		private void OnDisable()
		{
			NfcRequests.onNewNfcDetect -= CheckData;
			NfcRequests.onNfcRemove -= OnNfcRemove;
		}

        private void Start()
        {
            for (int i = 0; i < landerData.Length; ++i)
            {
                landerDisplayHandler[i].SetMesh(landerData[i].BundleModel);
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