namespace dgames.Tasks
{
    public enum TaskResult
    {
        /// <summary>
        /// Not run yet
        /// </summary>
        None = -1,

        /// <summary>
        /// The task is finished and succeeded
        /// </summary>
        Success = 0,

        /// <summary>
        /// The task failed and needs to retry
        /// </summary>
        Retry = 1,

        /// <summary>
        /// The task failed and needs a response from the user before retry
        /// </summary>
        WaitResponse = 2,

        /// <summary>
        /// The task failed without retries
        /// </summary>
        Fail = 3,
    }
}
