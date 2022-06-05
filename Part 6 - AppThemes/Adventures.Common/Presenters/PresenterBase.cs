using System;
using Adventures.Common.Events;
using Adventures.Common.Extensions;
using Adventures.Common.Interfaces;

namespace Adventures.Common.Presenters
{
	public class PresenterBase : IPresenter
    {
        protected IServiceProvider _serviceProvider;

        public PresenterBase(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public virtual void Initialize(object sender = null, EventArgs e = null) { }


        public async Task ButtonClickHandler(object sender = null, EventArgs e = null)
        {
            var button = sender as Button;
            var buttonArgs = e as ButtonEventArgs;

            buttonArgs.Presenter = this;
            buttonArgs.Sender = sender;
            buttonArgs.Key = button?.Text ?? sender.GetType().Name;

            var command = _serviceProvider.GetNamedCommand(buttonArgs.Key);
            command.Execute(buttonArgs);

            await Task.Delay(1); // 1 millisecond for our async process
            return;
        }

    }
}

