using System.Linq;
using UnityEngine;

namespace LandersLegends.Battle
{
	public class Player2State : BattleState
    {
        public Player2State(BattleStateMachine stateMachine)
            : base(stateMachine) { }

        public override void Enter()
        {
            if (stateMachine.Lander2.IsAi)
            {
                stateMachine.Lander2.lastMoveProccess = (ushort)stateMachine.Lander2.Moves
                    .Where(x => x.id != 0 && x.pp > 0)
                    .OrderBy(x => Random.value)
                    .First()
                    .id;
				stateMachine.ProcessNextState();
                return;
            }

			stateMachine.Lander2.CinematicHandler.StartTurn();
			stateMachine.HudHandler.UpdateAttackUI(stateMachine.Lander2);
		}

        public override void Exit()
		{
			if (stateMachine.Lander2.IsAi)
                return;

			stateMachine.Lander2.CinematicHandler.EndTurn();
			stateMachine.HudHandler.ClearAttackUI();
		}

        public override void Update()
        {

        }
    }
}
