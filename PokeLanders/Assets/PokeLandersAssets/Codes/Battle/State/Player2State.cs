using UnityEngine;

namespace Lander.Battle
{
    public class Player2State : BattleState
    {
        public Player2State(BattleStateMachine stateMachine)
            : base(stateMachine) { }

        public override void Enter()
        {
            stateMachine.BattleLandersHandler[1].StartTurn();
        }

        public override void Exit()
        {
            stateMachine.BattleLandersHandler[1].EndTurn();
        }

        public override void Update()
        {
            if (Input.GetKeyDown(KeyCode.P))
            {
                stateMachine.ProcessState(stateMachine.Factory.GetState<AttackProcessState>());
            }
        }
    }
}
