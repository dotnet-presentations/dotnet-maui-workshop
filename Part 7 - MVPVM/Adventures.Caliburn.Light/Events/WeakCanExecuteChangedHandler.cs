using System;
using System.Windows.Input;

namespace Caliburn.Light
{
    internal sealed class WeakCanExecuteChangedHandler<TSubscriber> :
        WeakEventHandlerBase<ICommand, TSubscriber, EventArgs>
        where TSubscriber : class
    {
        public WeakCanExecuteChangedHandler(ICommand source, TSubscriber subscriber,
            Action<TSubscriber, object, EventArgs> weakHandler)
            : base(source, subscriber, weakHandler)
        {
            source.CanExecuteChanged += OnEvent;
        }

        protected override void RemoveEventHandler(ICommand source)
        {
            source.CanExecuteChanged -= OnEvent;
        }
    }
}
