
namespace Adventures.Common.Presenters
{
    public class PresenterBase : IMvpPresenter
    {
        private IMvpViewModel _viewModel;

        public Dictionary<string, IMvpView> Views { get; set; } = new Dictionary<string, IMvpView>();
        public IMvpViewModel ViewModel {
            get { return _viewModel; }
            set {
                _viewModel = value;
                OnViewModel();
            }
        }
        public bool IsInitialized { get; set; }

        protected IServiceProvider _serviceProvider;

        public PresenterBase(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public virtual void OnViewModel()
        {
            var commands = _serviceProvider
                .GetServices<IMvpCommand>()
                .Where(x => true);

            if (commands != null)
            {
                ViewModel.ButtonItems = new ObservableCollection<Button>();
                foreach(var command in commands)
                {
                    if (command.MatchDataType == "ListItem") continue;
                    var supportedBy = command?.SupportedBy ?? "NA";
                    var name = this.GetType().Name;

                    if (!supportedBy.Contains(name))
                        if(!supportedBy.Contains("GLOBAL")) continue;

                    var buttonVm = new Button {
                        Margin = new Thickness(2,10),
                        Text = command.MatchButtonText,
                        Style = (Style)Application.Current.Resources["ButtonOutline"]
                    };
                    ViewModel.ButtonItems.Add(buttonVm);
                }
            }
            OnViewModelSet();
        }

        public virtual void OnViewModelSet() { }

        public virtual void Initialize(object sender = null, EventArgs e = null)
        {
            var view = sender as IMvpView;
            var name = view.GetType().Name;
            if(!Views.ContainsKey(name))
                Views.Add(name, view);
        }

        public async void InvokeCommand(string commandKey)
        {
            var args = new ButtonEventArgs();
            await ButtonClickHandler(new Button { Text = commandKey });
        }

        public async Task ButtonClickHandler(object sender = null, EventArgs e = null)
        {
            var button = sender as Button;
            var buttonArgs = (e as ButtonEventArgs) ?? new ButtonEventArgs();

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

