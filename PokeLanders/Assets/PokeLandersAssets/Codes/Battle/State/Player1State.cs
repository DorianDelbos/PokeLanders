using UnityEngine;

namespace LandersLegends.Battle
{
    public class Player1State : BattleState
    {
        public Player1State(BattleStateMachine stateMachine)
            : base(stateMachine) { }

        public override void Enter()
        {
            stateMachine.BattleLandersHandler[0].StartTurn();
            stateMachine.HudHandler.SetActiveAttack(true);
        }

        public override void Exit()
        {
            stateMachine.BattleLandersHandler[0].EndTurn();
            stateMachine.HudHandler.SetActiveAttack(false);
        }

        public override void Update()
        {
            if (Input.GetKeyDown(KeyCode.P))
            {
                stateMachine.ProcessState(stateMachine.Factory.GetState<Player2State>());
            }
        }
    }
}
