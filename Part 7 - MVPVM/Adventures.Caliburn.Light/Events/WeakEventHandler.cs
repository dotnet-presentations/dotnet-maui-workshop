using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows.Input;

namespace Caliburn.Light
{
    /// <summary>
    /// Helper to register weak event handlers.
    /// </summary>
    public static class WeakEventHandler
    {
        /// <summary>
        /// Registers a weak handler to <see cref="INotifyPropertyChanging.PropertyChanging"/>.
        /// </summary>
        /// <typeparam name="TSubscriber">The type of the event subscriber.</typeparam>
        /// <param name="source">The event source.</param>
        /// <param name="subscriber">The event subscriber.</param>
        /// <param name="weakHandler">The weak handler.</param>
        /// <returns>A registration object that can be used to deregister from the event.</returns>
        public static IDisposable RegisterPropertyChangingWeak<TSubscriber>(this INotifyPropertyChanging source,
            TSubscriber subscriber, Action<TSubscriber, object, System.ComponentModel.PropertyChangingEventArgs> weakHandler)
            where TSubscriber : class
        {
            return new WeakNotifyPropertyChangingHandler<TSubscriber>(source, subscriber, weakHandler);
        }

        /// <summary>
        /// Registers a weak handler to <see cref="INotifyPropertyChanged.PropertyChanged"/>.
        /// </summary>
        /// <typeparam name="TSubscriber">The type of the event subscriber.</typeparam>
        /// <param name="source">The event source.</param>
        /// <param name="subscriber">The event subscriber.</param>
        /// <param name="weakHandler">The weak handler.</param>
        /// <returns>A registration object that can be used to deregister from the event.</returns>
        public static IDisposable RegisterPropertyChangedWeak<TSubscriber>(this INotifyPropertyChanged source,
            TSubscriber subscriber, Action<TSubscriber, object, PropertyChangedEventArgs> weakHandler)
            where TSubscriber : class
        {
            return new WeakNotifyPropertyChangedHandler<TSubscriber>(source, subscriber, weakHandler);
        }

        /// <summary>
        /// Registers a weak handler to <see cref="INotifyCollectionChanged.CollectionChanged"/>.
        /// </summary>
        /// <typeparam name="TSubscriber">The type of the event subscriber.</typeparam>
        /// <param name="source">The event source.</param>
        /// <param name="subscriber">The event subscriber.</param>
        /// <param name="weakHandler">The weak handler.</param>
        /// <returns>A registration object that can be used to deregister from the event.</returns>
        public static IDisposable RegisterCollectionChangedWeak<TSubscriber>(this INotifyCollectionChanged source,
            TSubscriber subscriber, Action<TSubscriber, object, NotifyCollectionChangedEventArgs> weakHandler)
            where TSubscriber : class
        {
            return new WeakNotifyCollectionChangedHandler<TSubscriber>(source, subscriber, weakHandler);
        }

        /// <summary>
        /// Registers a weak event handler to <see cref="ICommand.CanExecuteChanged"/>.
        /// </summary>
        /// <typeparam name="TSubscriber">The type of the event subscriber.</typeparam>
        /// <param name="source">The event source.</param>
        /// <param name="subscriber">The event subscriber.</param>
        /// <param name="weakHandler">The weak handler.</param>
        /// <returns>A registration object that can be used to deregister from the event.</returns>
        public static IDisposable RegisterCanExecuteChangedWeak<TSubscriber>(this ICommand source,
            TSubscriber subscriber, Action<TSubscriber, object, EventArgs> weakHandler)
            where TSubscriber : class
        {
            return new WeakCanExecuteChangedHandler<TSubscriber>(source, subscriber, weakHandler);
        }
    }
}
