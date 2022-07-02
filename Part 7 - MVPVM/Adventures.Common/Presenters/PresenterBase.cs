
namespace Adventures.Common.Presenters
{
    public class PresenterBase : IMvpPresenter
    {
        private IMvpViewModel _viewModel;
        private IListViewModel _listVm;
        private string _title;

        protected ConnectivityUtil Internet = new ConnectivityUtil();
        protected IMvpEventAggregator EventAggregator;
        protected IDataService DataService;
        protected string Title
        {
            get { return _title; }
            set {
                _title = value;
                if(_listVm!=null)
                    _listVm.Title = value;   
            }
        }

        public IServiceProvider Provider { get; set; }

        public List<string> SupportedButtons { get; set; } = new List<string>();

        public Dictionary<string, IMvpView> Views { get; set; } =
            new Dictionary<string, IMvpView>();

        public IMvpViewModel ViewModel {
            get { return _viewModel; }
            set {
                _viewModel = value;
                OnViewModel();
            }
        }

        public bool IsInitialized { get; set; }

        public PresenterBase(IServiceProvider serviceProvider)
        {
            Provider = serviceProvider;

            Internet.ConnectivityChanged += OnInternetConnectivityChanged;
        }

        public PresenterBase(
              IServiceProvider serviceProvider
            , IMvpEventAggregator eventAggregator
            , IDataService dataService
            , IListViewModel listVm) : this(serviceProvider)
        {
            EventAggregator = eventAggregator;
            DataService = dataService;
            _listVm = listVm;

            _listVm.Mode = DataService.Mode;
            _listVm.Presenter = this;

            ViewModel = _listVm;
        }

        public void OnViewModel()
        {
            var commands = ViewModel.Presenter.Provider
               .GetServices<IMvpCommand>()
               .Where(x => true);

            if (commands != null)
            {
                SetSupportedButtons(); // Get list of supported buttons

                // Build supported button items for this presenter (using order)
                ViewModel.ButtonItems = new ObservableCollection<Button>();
                foreach(var button in SupportedButtons)
                {
                    var command = commands.FirstOrDefault(c => c.Name == button);
                    if (command == null) continue; 
                    var buttonVm = new Button
                    {
                        Margin = new Thickness(2, 10),
                        Text = command.MatchButtonText,
                        Style = (Style)
                            Application.Current.Resources["ButtonOutline"]
                    };
                    ViewModel.ButtonItems.Add(buttonVm);
                }
            }
            OnViewModelSet();
        }

        protected virtual void SetSupportedButtons() { }

        public virtual void OnViewModelSet() { }

        protected virtual void OnInternetConnectivityChanged(
            object sender, ConnectivityEventArgs e)
        {
        }

        public void Initialize(object sender = null, EventArgs e = null)
        {
            var view = sender as IMvpView;
            var name = view.GetType().Name;
            if(!Views.ContainsKey(name))
                Views.Add(name, view);

            // as a singleton we only need to do this once
            if (this.IsInitialized)
                return;

            OnInitialize(sender, e);
        }

        protected virtual void OnInitialize(
            object sender=null, EventArgs e=null) { }

        public async void InvokeCommand(string commandKey)
        {
            var args = new ButtonEventArgs();
            await ButtonClickHandler(new Button { Text = commandKey });
        }

        public async Task ButtonClickHandler(
            object sender = null, EventArgs e = null)
        {
            var button = sender as Button;
            var buttonArgs = (e as ButtonEventArgs) ?? new ButtonEventArgs();

            buttonArgs.Presenter = this;
            buttonArgs.Sender = sender;
            buttonArgs.Views = Views;
            buttonArgs.Key = button?.Text ?? sender.GetType().Name;

            var command = Provider.GetNamedCommand(buttonArgs.Key);
            command.Execute(buttonArgs);

            await Task.Delay(1); // 1 millisecond for our async process
            return;
        }
    }
}

