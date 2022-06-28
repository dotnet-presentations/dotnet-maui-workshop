using System;
using System.Threading.Tasks;

namespace Caliburn.Light
{
    internal sealed class EventAggregatorHandler<TTarget, TMessage> : IEventAggregatorHandler
        where TTarget : class
    {
        private readonly WeakReference<TTarget> _weakTarget;
        private readonly Func<TTarget, TMessage, Task> _handler;

        public EventAggregatorHandler(TTarget target, Func<TTarget, TMessage, Task> handler, IDispatcher dispatcher)
        {
            _weakTarget = new WeakReference<TTarget>(target);
            _handler = handler;
            Dispatcher = dispatcher;
        }

        public IDispatcher Dispatcher { get; }

        public bool IsDead => !_weakTarget.TryGetTarget(out _);

        public bool CanHandle(object message) => message is TMessage;

        public Task HandleAsync(object message)
        {
            if (!_weakTarget.TryGetTarget(out TTarget target))
                return Task.CompletedTask;

            return _handler(target, (TMessage)message);
        }
    }
}
