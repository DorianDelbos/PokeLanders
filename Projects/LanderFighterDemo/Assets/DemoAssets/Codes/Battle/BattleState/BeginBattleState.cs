using UnityEngine;

namespace LanderFighter
{
    public class BeginBattleState : BattleStateBase
    {
        public override BattleState GetNextState()
        {
            return BattleState.PlayersChoice;
        }

        public override void OnEnter()
        {
            battleSystem.BattleTextInfo.Enable = true;
            battleSystem.BattleTextInfo.DialogueText.ReadText($"You find a {battleSystem.LanderOpponent.Name}.\n<pause=1>Prepare for battle!");
        }

        public override void OnExit()
        {
            battleSystem.BattleTextInfo.Enable = false;
        }

        public override BattleStateResult OnUpdate()
        {
            battleSystem.BattleTextInfo.DialogueText.multSpeed = Input.anyKey ? 5.0f : 1.0f;

            if (!battleSystem.BattleTextInfo.DialogueText.IsReading && Input.anyKeyDown)
            {
                return BattleStateResult.NextState;
            }

            return BattleStateResult.Running;
        }
    }
}
