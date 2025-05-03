namespace LanderFighter
{
    public abstract class BattleStateBase
    {
        protected BattleSystem battleSystem => BattleSystem.Current;

        /// <summary>
        /// Gets the next state to switch to if needed.
        /// This is called only if the state transitions.
        /// </summary>
        /// <returns>The next state class to transition to, or null if no transition.</returns>
        public abstract BattleState GetNextState();

        /// <summary>
        /// How to enter the combat state
        /// </summary>
        public abstract void OnEnter();

        /// <summary>
        /// How to leave the combat state
        /// </summary>
        public abstract void OnExit();

        /// <summary>
        /// Method called at each frame (update), returns the result of the state update
        /// </summary>
        /// <returns>The result indicating what should happen after the update</returns>
        public abstract BattleStateResult OnUpdate();
    }
}
