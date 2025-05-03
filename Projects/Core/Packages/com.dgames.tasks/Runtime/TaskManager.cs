using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dgames.Tasks
{
    public class TaskManager
    {
        private Stack<TaskBase> m_taskStack;
        private List<TaskBase> m_runningTasks;
        private List<TaskBase> m_failedTasks;
        private bool m_isRunning;

        public bool IsRunning => m_isRunning;

        public TaskManager()
        {
            m_taskStack = new Stack<TaskBase>();
            m_runningTasks = new List<TaskBase>();
            m_failedTasks = new List<TaskBase>();
            m_isRunning = false;
        }

        /// <summary>
        /// Enqueues a task to be run later.
        /// </summary>
        /// <param name="task">The task to be enqueued.</param>
        public void EnqueueTask(TaskBase task)
        {
            m_taskStack.Push(task);
        }

        /// <summary>
        /// Enqueues a task to be run later.
        /// </summary>
        /// <param name="task">The task to be enqueued.</param>
        public void EnqueueTask(IEnumerable<TaskBase> tasks)
        {
            tasks.ToList().ForEach(task => m_taskStack.Push(task));
        }

        /// <summary>
        /// Starts processing tasks in the stack.
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task StartProcessingAsync()
        {
            m_isRunning = true;

            while (m_taskStack.Any() && m_isRunning)
            {
                var task = m_taskStack.Pop();
                await RunTaskAsync(task);
            }
        }

        /// <summary>
        /// Stops processing tasks.
        /// </summary>
        public void StopProcessing()
        {
            m_isRunning = false;
        }

        /// <summary>
        /// Gets all tasks that have failed.
        /// </summary>
        /// <returns>An enumerable of failed tasks.</returns>
        public IEnumerable<TaskBase> GetFailedTasks()
        {
            return m_failedTasks;
        }

        /// <summary>
        /// Gets all currently running tasks.
        /// </summary>
        /// <returns>An enumerable of running tasks.</returns>
        public IEnumerable<TaskBase> GetRunningTasks()
        {
            return m_runningTasks;
        }

        /// <summary>
        /// Gets the count of tasks in the stack.
        /// </summary>
        /// <returns>The count of tasks in the stack.</returns>
        public int GetTaskStackCount()
        {
            return m_taskStack.Count;
        }

        public async Task RunTaskAsync(TaskBase task)
        {
            if (m_runningTasks.Contains(task))
            {
                return;
            }

            m_runningTasks.Add(task);
            await task.Run();

            switch (task.Result)
            {
                case TaskResult.Success:
                    OnTaskSuccess(task);
                    break;

                case TaskResult.Retry:
                    OnTaskRetry(task);
                    break;

                case TaskResult.WaitResponse:
                    OnTaskWaitResponse(task);
                    break;

                case TaskResult.Fail:
                    OnTaskFail(task);
                    break;
            }

            m_runningTasks.Remove(task);
        }

        #region Private methods

        private void OnTaskSuccess(TaskBase task)
        {
            Console.WriteLine($"Task {task.GetType().Name} succeeded.");
        }

        private void OnTaskRetry(TaskBase task)
        {
            Console.WriteLine($"Task {task.GetType().Name} failed, retrying...");
            EnqueueTask(task);
        }

        private void OnTaskWaitResponse(TaskBase task)
        {
            Console.WriteLine($"Task {task.GetType().Name} is waiting for response. Re-enqueuing at the top of the stack and stopping further execution.");
            m_taskStack.Push(task);
            StopProcessing();
        }

        private void OnTaskFail(TaskBase task)
        {
            Console.WriteLine($"Task {task.GetType().Name} failed.");
            m_failedTasks.Add(task);
        }

        #endregion
    }
}
