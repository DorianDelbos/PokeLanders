using Lander.Gameplay.Battle;
using UnityEngine;
using UnityEngine.Playables;

namespace Lander.Gameplay
{
	public class LanderHandler : MonoBehaviour
	{
		private enum LanderId
		{
			Lander1,
			Lander2
		}

		[SerializeField] private LanderId landerId;
		[SerializeField] private BattleState landerState;
		[SerializeField] private PlayableDirector playableDirector;
		[SerializeField] private LanderDisplayHandler landerDisplayHandler;

		private void OnEnable()
		{
			BattleSystem.enterBattleEvents[(int)landerState] += StartTurn;
			BattleSystem.exitBattleEvents[(int)landerState] += EndTurn;
		}

		private void OnDisable()
		{
			BattleSystem.enterBattleEvents[(int)landerState] -= StartTurn;
			BattleSystem.exitBattleEvents[(int)landerState] -= EndTurn;
		}

		private void Start()
		{
			landerDisplayHandler.SetMesh(GameManager.instance.Landers[(int)landerId].Mesh);
		}

		private void StartTurn()
		{
			playableDirector.Play();
		}

		private void EndTurn()
		{
			playableDirector.time = 0.0f;
			playableDirector.Stop();
		}
	}
}
