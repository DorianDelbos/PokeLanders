using UnityEditor;
using UnityEngine;

namespace LandersLegends.Battle
{
	[CustomEditor(typeof(BattleStateMachine))]
	public class BattleStateMachineEditor : Editor
	{
		public override void OnInspectorGUI()
		{
			base.OnInspectorGUI();

			BattleStateMachine battleStateMachine = (BattleStateMachine)target;

			if (GUILayout.Button("Next step"))
				battleStateMachine.ProcessNextState();
		}
	}
}