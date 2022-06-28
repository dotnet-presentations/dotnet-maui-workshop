using System.Threading.Tasks;

namespace Caliburn.Light
{
    /// <summary>
    /// Helper for asynchronous methods.
    /// </summary>
    public static class TaskHelper
    {
        /// <summary>
        /// A completed boolean task with true.
        /// </summary>
        public static readonly Task<bool> TrueTask = Task.FromResult(true);

        /// <summary>
        /// A completed boolean task with false.
        /// </summary>
        public static readonly Task<bool> FalseTask = Task.FromResult(false);

        /// <summary>
        /// Awaits a task to observe the exception.
        /// </summary>
        /// <param name="task">The task to observe.</param>
        public static async void Observe(this Task task)
        {
            await task.ConfigureAwait(false);
        }
    }
}
