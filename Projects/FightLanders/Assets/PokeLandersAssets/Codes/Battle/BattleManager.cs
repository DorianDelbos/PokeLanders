using LandersLegends.Extern;
using UnityEngine;
using LandersLegends.Gameplay;
using GLTFast;

namespace LandersLegends.Battle
{
	public class BattleManager : MonoBehaviour
	{
		private static BattleManager i;

		private string[] tagRegisters;
        [SerializeField] private GltfAsset[] gltfAssets = new GltfAsset[2];
		[SerializeField] private BattleStateMachine stateMachine;

        private Lander[] landerData => GameManager.instance.Landers;
		private NfcErrorHandler nfcErrorHandler => NfcErrorHandler.current;
		public BattleStateMachine StateMachine => stateMachine;
		public static BattleManager instance => i;

		private void Awake()
		{
			if (i == null)
			{
				i = this;
			}
			else
			{
				Destroy(gameObject);
				return;
			}

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

        private void Start()
        {
			stateMachine.Start();
			loadModels();
		}

		private async void loadModels()
		{
			for (int i = 0; i < landerData.Length; ++i)
				await gltfAssets[i].Load(landerData[i].ModelUrl);
		}

		private void Update()
		{
			stateMachine.Update();
		}

		private void DisplayNfcError(bool isError, string errorText)
		{
			Time.timeScale = (isError ? 0.0f : 1.0f);

			nfcErrorHandler.Close();

			if (isError)
				nfcErrorHandler.CallError(errorText);
		}

		private void OnNfcRemove(LanderDataNFC data) 
			=> DisplayNfcError(true, $"You must replace {data.name} on the player to continue !");

		private void CheckData(LanderDataNFC data)
		{
			if (!data.Equals(tagRegisters))
			{
				DisplayNfcError(true, $"Wrong Lander detected, make sure it's the same card used !");
				return;
			}

			DisplayNfcError(false, string.Empty);
        }
	}
}