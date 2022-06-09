using System;
using Adventures.Common.Events;
using Adventures.Common.Extensions;
using Adventures.Common.Interfaces;

namespace Adventures.Common.Presenters
{
	public class PresenterBase : IMvpPresenter
    {
        public Dictionary<string, IMvpView> Views { get; set; } = new Dictionary<string, IMvpView>();
        public IMvpViewModel ViewModel { get; set; }

        protected IServiceProvider _serviceProvider;

        public PresenterBase(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public virtual void Initialize(object sender = null, EventArgs e = null)
        {
            var view = sender as IMvpView;
            var name = view.GetType().Name;
            if(!Views.ContainsKey(name))
                Views.Add(name, view);
        }

        public async Task ButtonClickHandler(object sender = null, EventArgs e = null)
        {
            var button = sender as Button;
            var buttonArgs = e as ButtonEventArgs;

            buttonArgs.Presenter = this;
            buttonArgs.Sender = sender;
            buttonArgs.Views = Views;
            buttonArgs.Key = button?.Text ?? sender.GetType().Name;

            var command = _serviceProvider.GetNamedCommand(buttonArgs.Key);
            command.Execute(buttonArgs);

            await Task.Delay(1); // 1 millisecond for our async process
            return;
        }

    }
}

