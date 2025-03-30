namespace LanderFighter
{
    public enum BattleState
    {
        /// <summary>
        /// The beginning of the battle
        /// </summary>
        BeginBattle,

        /// <summary>
        /// All players will choise an action
        /// </summary>
        PlayersChoice,

        /// <summary>
        /// All attacks are procceed
        /// </summary>
        ProcessAttacks,

        /// <summary>
        /// End of the battle
        /// </summary>
        EndBattle,

        /// <summary>
        /// Return main menu
        /// </summary>
        ReturnMainMenu
    }
}
