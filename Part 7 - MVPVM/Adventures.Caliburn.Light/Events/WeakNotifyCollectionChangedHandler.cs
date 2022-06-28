using System;
using System.Collections.Specialized;

namespace Caliburn.Light
{
    internal sealed class WeakNotifyCollectionChangedHandler<TSubscriber> :
        WeakEventHandlerBase<INotifyCollectionChanged, TSubscriber, NotifyCollectionChangedEventArgs>
        where TSubscriber : class
    {
        public WeakNotifyCollectionChangedHandler(INotifyCollectionChanged source, TSubscriber subscriber,
            Action<TSubscriber, object, NotifyCollectionChangedEventArgs> weakHandler)
            : base(source, subscriber, weakHandler)
        {
            source.CollectionChanged += OnEvent;
        }

        protected override void RemoveEventHandler(INotifyCollectionChanged source)
        {
            source.CollectionChanged -= OnEvent;
        }
    }
}
