namespace LanderFighter
{
    /// <summary>
    /// Enum to represent the result of an update state in a battle system.
    /// </summary>
    public enum BattleStateResult
    {
        /// <summary>
        /// Indicates that the battle state has transitioned to the next state.
        /// </summary>
        NextState,

        /// <summary>
        /// Indicates that the battle state is still running and hasn't transitioned.
        /// </summary>
        Running
    }
}
