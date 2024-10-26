namespace Lander.Battle
{
    public abstract class BattleState
    {
        protected BattleStateMachine stateMachine;

        public BattleState(BattleStateMachine stateMachine)
        {
            this.stateMachine = stateMachine;
        }

        public abstract void Enter();
        public abstract void Update();
        public abstract void Exit();
    }
}
