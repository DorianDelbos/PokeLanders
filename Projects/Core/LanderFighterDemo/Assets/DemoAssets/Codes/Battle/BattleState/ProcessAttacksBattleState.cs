using Landers;
using Landers.API;
using Landers.Utils;
using System.Collections;
using UnityEngine;

namespace LanderFighter
{
    public class ProcessAttacksBattleState : BattleStateBase
    {
        private bool attackInProgress = false;

        public override BattleState GetNextState()
        {
            return CheckEndFight()
                ? BattleState.EndBattle
                : BattleState.PlayersChoice;
        }

        public override void OnEnter()
        {
            if (!attackInProgress)
            {
                attackInProgress = true;
                battleSystem.StartCoroutine(ProcessAttacks());
            }
        }

        public override void OnExit()
        {
            battleSystem.AttacksRegister.Clear();
        }

        public override BattleStateResult OnUpdate()
        {
            return attackInProgress
                ? BattleStateResult.Running
                : BattleStateResult.NextState;
        }

        private IEnumerator ProcessAttacks()
        {
            ushort speedPlayer = LanderUtils.GetStatValue(StatsEnum.Speed, battleSystem.LanderPlayer);
            ushort speedOpponent = LanderUtils.GetStatValue(StatsEnum.Speed, battleSystem.LanderOpponent);

            var firstAttacker = speedPlayer < speedOpponent ? battleSystem.LanderOpponent : battleSystem.LanderPlayer;
            var secondAttacker = firstAttacker == battleSystem.LanderPlayer ? battleSystem.LanderOpponent : battleSystem.LanderPlayer;

            ProcessAttack(firstAttacker, secondAttacker, battleSystem.AttacksRegister[firstAttacker.Tag]);
            yield return new WaitForSeconds(2.0f);

            if (secondAttacker.IsAlive())
            {
                ProcessAttack(secondAttacker, firstAttacker, battleSystem.AttacksRegister[secondAttacker.Tag]);
                yield return new WaitForSeconds(2.0f);
            }

            attackInProgress = false;
        }

        private void ProcessAttack(LanderData attacker, LanderData defenser, Move move)
        {
            ushort damages = LanderUtils.CalculAttackDamage(attacker, defenser, move);
            defenser.TakeDamage(damages);
        }

        private bool CheckEndFight()
        {
            return !battleSystem.LanderOpponent.IsAlive() || !battleSystem.LanderPlayer.IsAlive();
        }
    }
}
