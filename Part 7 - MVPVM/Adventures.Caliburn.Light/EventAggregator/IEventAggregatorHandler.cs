using System.Threading.Tasks;

namespace Caliburn.Light
{
    /// <summary>
    /// An <see cref="IEventAggregator"/> handler.
    /// </summary>
    public interface IEventAggregatorHandler
    {
        /// <summary>
        /// Gets the execution context.
        /// </summary>
        IDispatcher Dispatcher { get; }

        /// <summary>
        /// Gets a value indicating whether this handler is dead.
        /// </summary>
        bool IsDead { get; }

        /// <summary>
        /// Determines whether this instance can handle the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <returns>True, when the message can be handled.</returns>
        bool CanHandle(object message);

        /// <summary>
        /// Handles the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task HandleAsync(object message);
    }
}
