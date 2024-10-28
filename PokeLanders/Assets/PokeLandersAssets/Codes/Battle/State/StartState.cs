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
            stateMachine.HudHandler.SetActiveDialogue(true);
            stateMachine.HudHandler.dialogueMesh.ReadText(textToRead);
        }

        public override void Exit()
        {
            stateMachine.HudHandler.dialogueMesh.multSpeed = 1.0f;
            stateMachine.HudHandler.SetActiveDialogue(false);
        }

        public override void Update()
        {
            bool isReading = stateMachine.HudHandler.dialogueMesh.IsReading;
            stateMachine.HudHandler.dialogueMesh.multSpeed = (Input.anyKey && isReading ? 20.0f : 1.0f);

            if (Input.anyKeyDown && !isReading)
                stateMachine.ProcessState(stateMachine.Factory.GetState<Player1State>());
        }
    }
}
