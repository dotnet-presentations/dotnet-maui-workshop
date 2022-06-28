using System;

namespace Caliburn.Light
{
    /// <summary>
    /// Base class for weak event handler on static event.
    /// </summary>
    /// <typeparam name="TSubscriber">The type of the event subscriber.</typeparam>
    /// <typeparam name="TEventArgs">The type of the event arguments.</typeparam>
    public abstract class WeakEventHandlerBase<TSubscriber, TEventArgs> : IWeakEventHandler, IDisposable
        where TSubscriber : class
    {
        private readonly WeakReference<TSubscriber> _subscriber;
        private readonly Action<TSubscriber, object, TEventArgs> _weakHandler;

        /// <summary>
        /// Initializes a new instance of the <see cref="WeakEventHandlerBase&lt;TSubscriber, TEventArgs&gt;"/> class.
        /// </summary>
        /// <param name="subscriber">The event subscriber.</param>
        /// <param name="weakHandler">The weak handler.</param>
        protected WeakEventHandlerBase(TSubscriber subscriber, Action<TSubscriber, object, TEventArgs> weakHandler)
        {
            if (subscriber is null)
                throw new ArgumentNullException(nameof(subscriber));
            if (weakHandler is null)
                throw new ArgumentNullException(nameof(weakHandler));

            _subscriber = new WeakReference<TSubscriber>(subscriber);
            _weakHandler = weakHandler;
        }

        /// <summary>
        /// Removes the event handler from the event source.
        /// </summary>
        protected abstract void RemoveEventHandler();

        /// <summary>
        /// Method called when the event is raised.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The <typeparamref name="TEventArgs"/> instance containing the event data.</param>
        /// <remarks>Register this method on the source event.</remarks>
        protected void OnEvent(object sender, TEventArgs args)
        {
            if (_subscriber.TryGetTarget(out var subscriber))
            {
                _weakHandler(subscriber, sender, args);
            }
            else
            {
                RemoveEventHandler();
            }
        }

        /// <summary>
        /// Unregisters the event handler from the event source.
        /// </summary>
        public void Dispose()
        {
            RemoveEventHandler();
        }
    }

    /// <summary>
    /// Base class for weak event handler.
    /// </summary>
    /// <typeparam name="TSource">The type of the event source.</typeparam>
    /// <typeparam name="TSubscriber">The type of the event subscriber.</typeparam>
    /// <typeparam name="TEventArgs">The type of the event arguments.</typeparam>
    public abstract class WeakEventHandlerBase<TSource, TSubscriber, TEventArgs> : IWeakEventHandler, IDisposable
        where TSource : class
        where TSubscriber : class
    {
        private readonly WeakReference<TSource> _source;
        private readonly WeakReference<TSubscriber> _subscriber;
        private readonly Action<TSubscriber, object, TEventArgs> _weakHandler;

        /// <summary>
        /// Initializes a new instance of the <see cref="WeakEventHandlerBase&lt;TSubscriber, TEventArgs&gt;"/> class.
        /// </summary>
        /// <param name="source">The event source.</param>
        /// <param name="subscriber">The event subscriber.</param>
        /// <param name="weakHandler">The weak handler.</param>
        protected WeakEventHandlerBase(TSource source, TSubscriber subscriber, Action<TSubscriber, object, TEventArgs> weakHandler)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));
            if (subscriber is null)
                throw new ArgumentNullException(nameof(subscriber));
            if (weakHandler is null)
                throw new ArgumentNullException(nameof(weakHandler));

            _source = new WeakReference<TSource>(source);
            _subscriber = new WeakReference<TSubscriber>(subscriber);
            _weakHandler = weakHandler;
        }

        private void RemoveEventHandler()
        {
            if (_source.TryGetTarget(out var source))
            {
                RemoveEventHandler(source);
            }
        }

        /// <summary>
        /// Removes the event handler from the event source.
        /// </summary>
        /// <param name="source">The event source.</param>
        protected abstract void RemoveEventHandler(TSource source);

        /// <summary>
        /// Method called when the event is raised.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The <typeparamref name="TEventArgs"/> instance containing the event data.</param>
        /// <remarks>Register this method on the source event.</remarks>
        protected void OnEvent(object sender, TEventArgs args)
        {
            if (_subscriber.TryGetTarget(out var subscriber))
            {
                _weakHandler(subscriber, sender, args);
            }
            else
            {
                RemoveEventHandler();
            }
        }

        /// <summary>
        /// Unregisters the event handler from the event source.
        /// </summary>
        public void Dispose()
        {
            RemoveEventHandler();
        }
    }
}
