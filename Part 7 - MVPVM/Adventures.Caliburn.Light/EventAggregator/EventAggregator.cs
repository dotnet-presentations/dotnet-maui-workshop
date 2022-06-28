using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Caliburn.Light
{
    /// <summary>
    /// Enables loosely-coupled publication of and subscription to events.
    /// </summary>
    public sealed class EventAggregator : IEventAggregator
    {
        private readonly object _lockObject = new object();
        private readonly List<KeyValuePair<IDispatcher, List<IEventAggregatorHandler>>> _contexts = new List<KeyValuePair<IDispatcher, List<IEventAggregatorHandler>>>();

        /// <summary>
        /// Subscribes the specified handler for messages of type <typeparamref name="TMessage" />.
        /// </summary>
        /// <typeparam name="TTarget">The type of the handler target.</typeparam>
        /// <typeparam name="TMessage">The type of the message.</typeparam>
        /// <param name="target">The message handler target.</param>
        /// <param name="handler">The message handler to register.</param>
        /// <param name="dispatcher">Specifies in which context the <paramref name="handler"/> is executed</param>
        /// <returns>The <see cref="IEventAggregatorHandler" />.</returns>
        public IEventAggregatorHandler Subscribe<TTarget, TMessage>(TTarget target, Action<TTarget, TMessage> handler, IDispatcher dispatcher = default)
            where TTarget : class
        {
            if (target is null)
                throw new ArgumentNullException(nameof(target));
            if (handler is null)
                throw new ArgumentNullException(nameof(handler));

            Task wrapper(TTarget t, TMessage m)
            {
                handler(t, m);
                return Task.CompletedTask;
            }

            var item = new EventAggregatorHandler<TTarget, TMessage>(target, wrapper, dispatcher ?? CurrentThreadDispatcher.Instance);
            SubscribeCore(item);
            return item;
        }

        /// <summary>
        /// Subscribes the specified handler for messages of type <typeparamref name="TMessage" />.
        /// </summary>
        /// <typeparam name="TTarget">The type of the handler target.</typeparam>
        /// <typeparam name="TMessage">The type of the message.</typeparam>
        /// <param name="target">The message handler target.</param>
        /// <param name="handler">The message handler to register.</param>
        /// <param name="dispatcher">Specifies in which context the <paramref name="handler"/> is executed.</param>
        /// <returns>The <see cref="IEventAggregatorHandler" />.</returns>
        public IEventAggregatorHandler Subscribe<TTarget, TMessage>(TTarget target, Func<TTarget, TMessage, Task> handler, IDispatcher dispatcher = default)
            where TTarget : class
        {
            if (target is null)
                throw new ArgumentNullException(nameof(target));
            if (handler is null)
                throw new ArgumentNullException(nameof(handler));

            var item = new EventAggregatorHandler<TTarget, TMessage>(target, handler, dispatcher ?? CurrentThreadDispatcher.Instance);
            SubscribeCore(item);
            return item;
        }

        private void SubscribeCore(IEventAggregatorHandler handler)
        {
            lock (_lockObject)
            {
                // find or create target context
                var targetContext = _contexts.Find(x => x.Key.Equals(handler.Dispatcher));
                if (targetContext.Key is null)
                {
                    targetContext = new KeyValuePair<IDispatcher, List<IEventAggregatorHandler>>(handler.Dispatcher, new List<IEventAggregatorHandler>());
                    _contexts.Add(targetContext);
                }

                targetContext.Value.Add(handler);

                CleanupCore(_contexts);
            }
        }

        /// <summary>
        /// Unsubscribes the specified handler.
        /// </summary>
        /// <param name="handler">The handler to unsubscribe.</param>
        public void Unsubscribe(IEventAggregatorHandler handler)
        {
            if (handler is null)
                throw new ArgumentNullException(nameof(handler));

            lock (_lockObject)
            {
                var targetContext = _contexts.Find(x => x.Key.Equals(handler.Dispatcher));
                if (targetContext.Key is not null)
                    targetContext.Value.RemoveAll(h => ReferenceEquals(h, handler));

                CleanupCore(_contexts);
            }
        }

        /// <summary>
        /// Publishes a message.
        /// </summary>
        /// <param name="message">The message instance.</param>
        public void Publish(object message)
        {
            if (message is null)
                throw new ArgumentNullException(nameof(message));

            lock (_lockObject)
            {
                CleanupCore(_contexts);

                // publish to current context
                foreach (var context in _contexts)
                {
                    if (context.Key.CheckAccess())
                        PublishCore(message, context.Value);
                }

                // publish to other contexts
                foreach (var context in _contexts)
                {
                    if (!context.Key.CheckAccess())
                        context.Key.BeginInvoke(() => PublishCore(message, context.Value));
                }
            }
        }

        private static void PublishCore(object message, List<IEventAggregatorHandler> handlers)
        {
            for (var i = 0; i < handlers.Count; i++)
            {
                var handler = handlers[i];

                if (!handler.CanHandle(message))
                    continue;

                var task = handler.HandleAsync(message);

                task.Observe();

                if (!task.IsCompleted)
                    Executing?.Invoke(null, new TaskEventArgs(task));
            }
        }

        private static void CleanupCore(List<KeyValuePair<IDispatcher, List<IEventAggregatorHandler>>> contexts)
        {
            // remove dead subscribers
            foreach (var context in contexts)
                context.Value.RemoveAll(h => h.IsDead);

            // cleanup contexts
            for (var i = contexts.Count - 1; i >= 0; i--)
            {
                if (contexts[i].Value.Count == 0)
                    contexts.RemoveAt(i);
            }
        }

        /// <summary>
        /// Occurs when <see cref="IEventAggregatorHandler.HandleAsync(object)"/> is invoked and the operation has not completed synchronously.
        /// </summary>
        public static event EventHandler<TaskEventArgs> Executing;

        private sealed class CurrentThreadDispatcher : IDispatcher
        {
            public static CurrentThreadDispatcher Instance = new CurrentThreadDispatcher();

            private CurrentThreadDispatcher()
            {
            }

            public void BeginInvoke(Action action) => action.Invoke();

            public bool CheckAccess() => true;
        }
    }
}
