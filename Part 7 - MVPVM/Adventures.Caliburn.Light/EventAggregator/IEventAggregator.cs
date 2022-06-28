using System;
using System.Threading.Tasks;

namespace Caliburn.Light
{
    /// <summary>
    /// Enables loosely-coupled publication of and subscription to events.
    /// </summary>
    public interface IEventAggregator
    {
        /// <summary>
        /// Subscribes the specified handler for messages of type <typeparamref name="TMessage"/>.
        /// </summary>
        /// <typeparam name="TTarget">The type of the handler target.</typeparam>
        /// <typeparam name="TMessage">The type of the message.</typeparam>
        /// <param name="target">The message handler target.</param>
        /// <param name="handler">The message handler to register.</param>
        /// <param name="dispatcher">Specifies in which context the <paramref name="handler"/> is executed.</param>
        /// <returns>The <see cref="IEventAggregatorHandler"/>.</returns>
        IEventAggregatorHandler Subscribe<TTarget, TMessage>(TTarget target, Action<TTarget, TMessage> handler, IDispatcher dispatcher = default)
            where TTarget : class;

        /// <summary>
        /// Subscribes the specified handler for messages of type <typeparamref name="TMessage"/>.
        /// </summary>
        /// <typeparam name="TTarget">The type of the handler target.</typeparam>
        /// <typeparam name="TMessage">The type of the message.</typeparam>
        /// <param name="target">The message handler target.</param>
        /// <param name="handler">The message handler to register.</param>
        /// <param name="dispatcher">Specifies in which context the <paramref name="handler"/> is executed.</param>
        /// <returns>The <see cref="IEventAggregatorHandler"/>.</returns>
        IEventAggregatorHandler Subscribe<TTarget, TMessage>(TTarget target, Func<TTarget, TMessage, Task> handler, IDispatcher dispatcher = default)
            where TTarget : class;

        /// <summary>
        /// Unsubscribes the specified handler.
        /// </summary>
        /// <param name="handler">The handler to unsubscribe.</param>
        void Unsubscribe(IEventAggregatorHandler handler);

        /// <summary>
        /// Publishes a message.
        /// </summary>
        /// <param name="message">The message instance.</param>
        void Publish(object message);
    }
}
