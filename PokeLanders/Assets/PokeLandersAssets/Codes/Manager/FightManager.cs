using UnityEngine;

public class FightManager : MonoBehaviour
{
	private string[] tagRegisters;

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
		NfcModule.onNewNfcDetect += CheckData;
		NfcModule.onNfcRemove += OnNfcRemove;
	}

	private void OnDisable()
	{
		NfcModule.onNewNfcDetect -= CheckData;
		NfcModule.onNfcRemove -= OnNfcRemove;
	}

	private void DisplayNfcError(bool isError, string errorText)
	{
		Time.timeScale = (isError ? 0.0f : 1.0f);
		nfcErrorHud.SetActive(isError);
		nfcErrorHud.SetErrorText(errorText);
	}
	
	private void OnNfcRemove(LanderDataNFC data) => DisplayNfcError(true, $"You must replace {data.customName} on the player to continue !");

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
