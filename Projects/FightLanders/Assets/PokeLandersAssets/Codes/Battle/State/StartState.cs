using LandersLegends.Gameplay;
using System.Linq;
using UnityEngine;

namespace LandersLegends.Battle
{
    public class StartState : BattleState
    {
        public StartState(BattleStateMachine stateMachine)
            : base(stateMachine) { }

        public override void Enter()
        {
            string textToRead = $"Let the battle begin!\n<pause=1>{GameManager.instance.Landers.OrderBy(x => x.Speed).First().Name} begins to play.";
            stateMachine.HudHandler.CallDialogue(textToRead);
        }

        public override void Exit()
        {
            stateMachine.HudHandler.ClearDialogue();
        }

        public override void Update()
        {
            bool isReading = stateMachine.HudHandler.UpdateDialogue();

            if (Input.anyKeyDown && !isReading)
                stateMachine.ProcessState(stateMachine.Factory.GetState<Player1State>());
        }
    }
}
