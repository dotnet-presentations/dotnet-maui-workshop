using System;
using System.Runtime.CompilerServices;

namespace Caliburn.Light
{
    /// <summary>
    /// Extensions for <see cref="IDispatcher"/>.
    /// </summary>
    public static class DispatcherHelper
    {
        /// <summary>
        /// Gets an awaiter instance that enables to switch to the dispatcher thread.
        /// </summary>
        /// <param name="dispatcher">The dispatcher.</param>
        /// <returns>An awaiter instance.</returns>
        public static DispatcherAwaitable SwitchTo(this IDispatcher dispatcher)
        {
            return new DispatcherAwaitable(dispatcher);
        }

        /// <summary>
        /// Provides an awaitable object that enables to switch to the dispatcher thread.
        /// </summary>
        public readonly struct DispatcherAwaitable : INotifyCompletion
        {
            private readonly IDispatcher _dispatcher;

            /// <summary>
            /// Initializes a new instance of <see cref="DispatcherAwaitable"/>.
            /// </summary>
            /// <param name="dispatcher">The dispatcher.</param>
            public DispatcherAwaitable(IDispatcher dispatcher)
            {
                _dispatcher = dispatcher;
            }

            /// <summary>
            /// Gets an awaiter used to await this <see cref="DispatcherAwaitable"/>.
            /// </summary>
            /// <returns>An awaiter instance.</returns>
            public DispatcherAwaitable GetAwaiter() => this;

            /// <summary>
            /// Ends the await on the completed task.
            /// </summary>
            public void GetResult()
            {
            }

            /// <summary>
            /// Gets a value that specifies whether the task being awaited is completed.
            /// </summary>
            public bool IsCompleted => _dispatcher.CheckAccess();

            /// <summary>
            /// Schedules the continuation action for the task associated with this awaiter.
            /// </summary>
            /// <param name="continuation">The action to invoke when the await operation completes.</param>
            public void OnCompleted(Action continuation) => _dispatcher.BeginInvoke(continuation);
        }
    }
}
