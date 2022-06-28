using System;
using System.Threading.Tasks;

namespace Caliburn.Light
{
    /// <summary>
    /// Provides data for <see cref="Task"/> events.
    /// </summary>
    public sealed class TaskEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TaskEventArgs"/> class.
        /// </summary>
        /// <param name="task">The supplied Task.</param>
        public TaskEventArgs(Task task)
        {
            if (task is null)
                throw new ArgumentNullException(nameof(task));

            Task = task;
        }

        /// <summary>
        /// The supplied task.
        /// </summary>
        public Task Task { get; }
    }
}
