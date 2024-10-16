using UnityEngine;
using UnityEngine.Playables;

public class LanderHandler : MonoBehaviour
{
	[SerializeField] private BattleState landerState;
	[SerializeField] private PlayableDirector playableDirector;

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
