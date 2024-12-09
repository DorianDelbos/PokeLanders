using LandersLegends.Gameplay;
using UnityEngine;

namespace LandersLegends.Battle
{
    public class EndState : BattleState
    {
        public EndState(BattleStateMachine stateMachine)
            : base(stateMachine) { }

        public override void Enter()
        {
            if (GameManager.instance.Landers[0].Hp <= 0)
            {
                Debug.Log($"{GameManager.instance.Landers[1].Name} win");
            }
            else if (GameManager.instance.Landers[1].Hp <= 0)
            {
                Debug.Log($"{GameManager.instance.Landers[0].Name} win");
            }
            else
            {
                Debug.Log("Game continue !");
                stateMachine.ProcessState(stateMachine.Factory.GetState<Player1State>());
            }
        }

        public override void Exit() { }

        public override void Update() { }
    }
}
