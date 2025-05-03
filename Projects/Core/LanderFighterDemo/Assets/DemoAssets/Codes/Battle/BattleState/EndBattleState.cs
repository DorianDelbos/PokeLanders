using UnityEngine;

namespace LanderFighter
{
    public class EndBattleState : BattleStateBase
    {
        public override BattleState GetNextState()
        {
            return BattleState.ReturnMainMenu;
        }

        public override void OnEnter()
        {
            bool playerIsWinner = battleSystem.LanderPlayer.IsAlive();
            battleSystem.BattleTextInfo.Enable = true;
            battleSystem.BattleTextInfo.DialogueText.ReadText(playerIsWinner
                ? $"{battleSystem.LanderPlayer.Name} win this battle !"
                : $"{battleSystem.LanderOpponent.Name} win this battle !");
        }

        public override void OnExit()
        {

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
