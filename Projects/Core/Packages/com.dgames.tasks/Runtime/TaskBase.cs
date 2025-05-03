using System.Threading.Tasks;

namespace dgames.Tasks
{
    /// <summary>
    /// Abstract base class for tasks that can be managed by the TaskManager.
    /// </summary>
    public abstract class TaskBase
    {
        /// <summary>
        /// Gets the result of the task.
        /// </summary>
        /// <returns>
        /// <see cref="TaskResult.Success"/> means the task succeeded.
        /// <see cref="TaskResult.Retry"/> means the task should be retried.
        /// <see cref="TaskResult.WaitResponse"/> means the task is waiting for a response.
        /// <see cref="TaskResult.Fail"/> means the task failed permanently.
        /// </returns>
        public TaskResult Result { get; protected set; }

        /// <summary>
        /// Runs the task asynchronously.
        /// The derived class must implement this method to define the task's behavior.
        /// </summary>
        public abstract Task Run();
    }
}
