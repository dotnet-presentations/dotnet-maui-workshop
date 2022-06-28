using System;
using System.ComponentModel;

namespace Caliburn.Light
{
    internal sealed class WeakNotifyPropertyChangingHandler<TSubscriber> :
        WeakEventHandlerBase<INotifyPropertyChanging, TSubscriber, System.ComponentModel.PropertyChangingEventArgs>
        where TSubscriber : class
    {
        public WeakNotifyPropertyChangingHandler(INotifyPropertyChanging source, TSubscriber subscriber,
            Action<TSubscriber, object, System.ComponentModel.PropertyChangingEventArgs> weakHandler)
            : base(source, subscriber, weakHandler)
        {
            source.PropertyChanging += OnEvent;
        }

        protected override void RemoveEventHandler(INotifyPropertyChanging source)
        {
            source.PropertyChanging -= OnEvent;
        }
    }
}
