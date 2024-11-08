using UnityEngine;

namespace LandersLegends.Battle
{
    public class AttackProcessState : BattleState
    {
        public AttackProcessState(BattleStateMachine stateMachine)
            : base(stateMachine) { }

        public override void Enter()
        {
            Debug.Log("Attack Process");
        }

        public override void Exit() { }

        public override void Update()
        {
            if (Input.GetKeyDown(KeyCode.P))
            {
                stateMachine.ProcessState(stateMachine.Factory.GetState<EndState>());
            }
        }
    }
}
