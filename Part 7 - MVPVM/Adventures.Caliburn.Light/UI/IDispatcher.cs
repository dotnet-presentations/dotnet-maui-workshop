using System;

namespace Caliburn.Light
{
    /// <summary>
    /// The UI execution context.
    /// </summary>
    public interface IDispatcher
    {
        /// <summary>
        /// Determines whether the calling thread is the thread associated with the UI context.
        /// </summary>
        bool CheckAccess();

        /// <summary>
        /// Queues the specified work to run on the UI thread.
        /// </summary>
        /// <param name="action">The work to execute asynchronously.</param>
        void BeginInvoke(Action action);
    }
}
