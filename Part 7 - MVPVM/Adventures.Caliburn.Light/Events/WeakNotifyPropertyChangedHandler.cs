using System;
using System.ComponentModel;

namespace Caliburn.Light
{
    internal sealed class WeakNotifyPropertyChangedHandler<TSubscriber> :
        WeakEventHandlerBase<INotifyPropertyChanged, TSubscriber, PropertyChangedEventArgs>
        where TSubscriber : class
    {
        public WeakNotifyPropertyChangedHandler(INotifyPropertyChanged source, TSubscriber subscriber,
            Action<TSubscriber, object, PropertyChangedEventArgs> weakHandler)
            : base(source, subscriber, weakHandler)
        {
            source.PropertyChanged += OnEvent;
        }

        protected override void RemoveEventHandler(INotifyPropertyChanged source)
        {
            source.PropertyChanged -= OnEvent;
        }
    }
}
