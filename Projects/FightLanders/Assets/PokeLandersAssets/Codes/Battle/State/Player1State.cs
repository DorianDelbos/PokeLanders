namespace LandersLegends.Battle
{
	public class Player1State : BattleState
    {
        public Player1State(BattleStateMachine stateMachine)
            : base(stateMachine) { }

        public override void Enter()
        {
            stateMachine.Lander1.CinematicHandler.StartTurn();
            stateMachine.HudHandler.UpdateAttackUI(stateMachine.Lander1);
        }

        public override void Exit()
        {
			stateMachine.Lander1.CinematicHandler.EndTurn();
            stateMachine.HudHandler.ClearAttackUI();
        }

        public override void Update()
        {

        }
    }
}
