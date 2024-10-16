using System;
using System.Collections;
using UnityEngine;

public enum BattleState
{
	Start,			// Initialise landers; displays; ...
	Lander1Turn,	// Enable Lander 1 HUD; change camera; ...
	Lander2Turn,	// If lander 2 is player, enable Lander 2 HUD; change camera; 
	// Attack,		// Show all landers attack, and make it attack each other.
	End				// Add xp to winner, change scene, ...
}

public class BattleSystem : MonoBehaviour
{
	public static BattleSystem current;
	public static Action[] enterBattleEvents = new Action[Enum.GetValues(typeof(BattleState)).Length];
	public static Action[] exitBattleEvents = new Action[Enum.GetValues(typeof(BattleState)).Length];
	private BattleState state = BattleState.Start;

	public BattleState State { get => state; }

	private void Awake()
	{
		if (current != null)
			Debug.LogWarning($"Another {name} script exist !");

		current = this;
	}

	private void Start()
	{
		ProcessState(BattleState.Start);
		StartCoroutine(DebugToDelete());
	}

	public void ProcessState(BattleState state)
	{
		exitBattleEvents[(int)this.state]?.Invoke();
		this.state = state;
		enterBattleEvents[(int)state]?.Invoke();
	}

	private IEnumerator DebugToDelete()
	{
		while (true)
		{
			switch (state)
			{
				case BattleState.Start:
					ProcessState(BattleState.Lander1Turn);
					break;
				case BattleState.Lander1Turn:
					ProcessState(BattleState.Lander2Turn);
					break;
				case BattleState.Lander2Turn:
					ProcessState(BattleState.Lander1Turn);
					break;
				default:
					break;
			}

			yield return new WaitForSeconds(15.0f);
		}
	}
}