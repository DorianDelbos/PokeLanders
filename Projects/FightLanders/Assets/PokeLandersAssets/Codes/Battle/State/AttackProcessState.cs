using LandersLegends.Extern.API;
using LandersLegends.Gameplay;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

namespace LandersLegends.Battle
{
	public class AttackProcessState : BattleState
    {
        public AttackProcessState(BattleStateMachine stateMachine)
            : base(stateMachine) { }

        public override void Enter()
        {
            LanderBattleHandler[] landers = BattleManager.instance.StateMachine.Landers.OrderBy(x => x.Lander.Speed).ToArray();

            // TODO : Special AND Normal attack
            Move moveUse = MoveRepository.GetById(landers[1].lastMoveProccess);
            if (moveUse.accuracy == 100 || moveUse.accuracy > Random.value * 100)
			    landers[1].Lander.TakeDamage(LanderUtils.CalculAttackDamage(landers[1].Lander, landers[0].Lander, moveUse));

            if (landers[1].Lander.Hp > 0)
			{
				moveUse = MoveRepository.GetById(landers[0].lastMoveProccess);
				if (moveUse.accuracy == 100 || moveUse.accuracy > Random.value * 100)
					landers[0].Lander.TakeDamage(LanderUtils.CalculAttackDamage(landers[0].Lander, landers[1].Lander, moveUse));
			}

			stateMachine.HudHandler.UpdateLandersHUD();

            if (landers[0].Lander.Hp <= 0 || landers[1].Lander.Hp <= 0)
				BattleManager.instance.StateMachine.ProcessState(stateMachine.Factory.GetState<EndState>());
            else
			    BattleManager.instance.StateMachine.ProcessNextState();
		}

        public override void Exit() { }

        public override void Update()
        {

        }
    }
}
