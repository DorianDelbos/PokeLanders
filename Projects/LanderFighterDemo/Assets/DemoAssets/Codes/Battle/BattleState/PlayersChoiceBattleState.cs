using dgames.Utils;
using Landers.API;
using System.Linq;

namespace LanderFighter
{
    public class PlayersChoiceBattleState : BattleStateBase
    {
        public override BattleState GetNextState()
        {
            return BattleState.ProcessAttacks;
        }

        public override void OnEnter()
        {
            battleSystem.BattleAttackListInfo.Enable = true;
            battleSystem.BattleAttackListInfo.InitializeAttacks(battleSystem.LanderPlayer);

            // AI
            ushort randomMoveId = battleSystem.LanderOpponent.Moves.Where(x => x > 0).GetRandom();
            Move randomMove = MoveRepository.Instance.GetById(randomMoveId);
            battleSystem.ProcessAttack(battleSystem.LanderOpponent, randomMove);
        }

        public override void OnExit()
        {
            battleSystem.BattleAttackListInfo.Enable = false;
        }

        public override BattleStateResult OnUpdate()
        {
            if (battleSystem.AttacksRegister.Count >= 2)
            {
                return BattleStateResult.NextState;
            }

            return BattleStateResult.Running;
        }
    }
}
