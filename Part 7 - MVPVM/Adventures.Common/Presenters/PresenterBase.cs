
namespace Adventures.Common.Presenters
{
    public class PresenterBase : IMvpPresenter
    {
        #region Fields and Properties 

        private string _title;
        private IMvpViewModel _viewModel;
        private ConnectivityUtil _internet = new ConnectivityUtil();

        /// <summary>
        /// Framework event aggregator for communication between presenters
        /// </summary>
        protected IMvpEventAggregator EventAggregator;

        /// <summary>
        /// Primary data service to be used by framework.  Implementation
        /// can be swapped out as required.
        /// </summary>
        protected IDataService DataService;

        /// <summary>
        /// Main List View Model [IListViewModel] which derives from
        /// IMvpViewModel.
        /// </summary>
        protected IListViewModel ListViewModel;

        /// <summary>
        /// Title to be used by view model.  When Title is set it the
        /// value is propagated to the ListViewModel:IListViewModel. Note
        /// that IListViewModel derives from IMvpViewModel so ViewModel
        /// and ListViewModel are the same instance; ViewModel is a downcast
        /// from ListViewModel.
        /// </summary>
        protected string Title
        {
            get { return _title; }
            set {
                _title = value;
                if(ListViewModel!=null)
                    ListViewModel.Title = value;   
            }
        }

        /// <summary>
        /// IOC service provider instance for framework use.  Set by the
        /// PresenterBase contructor(s)
        /// </summary>
        public IServiceProvider Provider { get; set; }

        /// <summary>
        /// When the ViewModel is set, the ViewModelSet() will update this
        /// from available commands if/as applicable
        /// </summary>
        public List<string> SupportedButtons { get; set; } = new List<string>();

        /// <summary>
        /// Set by PresenterBase.Initialize with a reference to the view that
        /// invoked its Initialize (xxxPage.ContentPageBase.ctor).  This 
        /// provides the presenter a reference to the view if required. The
        /// presenter can cast to xxxPage and reference it directly.
        /// </summary>
        public Dictionary<string, IMvpView> Views { get; set; } =
            new Dictionary<string, IMvpView>();

        /// <summary>
        /// Invokes ViewModelSet() which in turn will invoke OnViewModelSet.
        /// </summary>
        public IMvpViewModel ViewModel {
            get { return _viewModel; }
            set {
                _viewModel = value;
                ViewModelSet();
            }
        }

        /// <summary>
        /// Initialize is invoked by a xxxxPage:ContentPageBase constructor. If
        /// a singleton we don't want initialize to be called more than once as
        /// it could cause subscriptions to be duplicated.  Presenters can
        /// interrogate this value if it wants to prevent this from happening.
        /// </summary>
        public bool IsInitialized { get; set; }

        #endregion

        /// <summary>
        /// Basic ctor which provides service provider.  Subscribes to
        /// ConnectivityChanged events which can be handled on the
        /// OnInternetConnectiveyChanged() method
        /// </summary>
        /// <param name="serviceProvider"></param>
        public PresenterBase(IServiceProvider serviceProvider)
        {
            Provider = serviceProvider;
            _internet.ConnectivityChanged += OnInternetConnectivityChanged;
        }

        /// <summary>
        /// Complete ctor that provides framework functionality for handling
        /// button clicks, data services, and event aggregation
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// <param name="eventAggregator"></param>
        /// <param name="dataService"></param>
        /// <param name="listVm"></param>
        public PresenterBase(
              IServiceProvider serviceProvider
            , IMvpEventAggregator eventAggregator
            , IDataService dataService
            , IListViewModel listVm) : this(serviceProvider)
        {
            EventAggregator = eventAggregator;
            DataService = dataService;
            ListViewModel = listVm;

            ListViewModel.Mode = DataService.Mode;
            ListViewModel.Presenter = this;

            // Property will invoke ViewModelSet() which in turn
            // will invoke OnViewModelSet.
            ViewModel = ListViewModel;
        }

        /// <summary>
        /// Invoked by the xxxxPage's baseclass [ContentPageBase] constructor.
        /// It sets the BindingContext to the Presenter.ViewModel value.  Note
        /// that it as logic to endure that it is only invoked once so that any
        /// subscriptions are not duplicated (if a singleton)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        /// <summary>
        /// Intended to be overriden.  Invoked after Presenter.Initialize()
        /// is completed.  At this point the Views dictionary will be populated
        /// with a reference of the View
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void OnInitialize(
            object sender=null, EventArgs e=null)
        {

        }

        /// <summary>
        /// Invoke a command using this presenters information
        /// to populate ButtonEventArgs
        /// </summary>
        /// <param name="commandKey"></param>
        public async void InvokeCommand(string commandKey)
        {
            var args = new ButtonEventArgs
            {
                Id = ViewModel.Id,
                Key = commandKey,
                Presenter = this,
                Sender = this,
                ViewModel = ViewModel,
                Views = Views,
            };
            await ButtonClickHandler(new Button { Text = commandKey }, args);
        }

        /// <summary>
        /// Invoked by the ViewModel base [BaseViewModel.ButtonClickHandler]
        /// with ButtonEventArgs populated with applicable information.
        ///
        /// The ButtonClickHandler will then find the command associated with
        /// the button click [within all registered IMvpCommand instances] and
        /// invoke it's Execute method.
        /// </summary>
        /// <param name="sender">instance of button that was clicked</param>
        /// <param name="e">ButtonEventArgs provided invoking process</param>
        public async Task ButtonClickHandler(
            object sender = null, EventArgs e = null)
        {
            var buttonArgs = (e as ButtonEventArgs) ?? new ButtonEventArgs();
            try
            {
                // Get the list of named commands (implementation of
                // IMvpCommand instances) registered in service provider
                var command = Provider.GetNamedCommand(buttonArgs);

                // Execute the applicable command passing in event arguments
                command.Execute(buttonArgs);

                await Task.Delay(1); // provide an await for function (1ms)
            }
            catch(Exception ex)
            {
                await MessageBox.Show($"Could not handle command" +
                    $" [{buttonArgs.Key}]",ex.Message, "OK");
            }
            return;
        }

        /// <summary>
        /// Intended to be overridden.  Invoked by the DefaultCommand
        /// if an existing command can not be found.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        public virtual async Task OnButtonClickHandler(
            object sender = null, EventArgs e = null)
        {
            var args = e as ButtonEventArgs;
            if (args != null)
                args.IsHandledByPresenter = false; // Not handled by presenter

            await Task.Delay(1); // provide an await for function (1ms)
        }

        /// <summary>
        /// Intended to be overridden, provide and opportunity for the
        /// presenter to set the buttons to be used by flexLayout.  Buttons
        /// will be displayed in the order listed in SupportedButtons list
        /// </summary>
        protected virtual void SetSupportedButtons() { }

        /// <summary>
        /// Intended to be overriden.  Invoked when there is a Internet
        /// connectivity change, e.g., OFFLINE/ONLINE
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void OnInternetConnectivityChanged(
            object sender, ConnectivityEventArgs e)
        {
        }

        /// <summary>
        /// Invoked by the ViewModel property when it is set.  This
        /// in turn invokes OnViewModelSet() so that presenters that
        /// derive from this have an opportunity to further extend
        /// logic once the view model is available
        /// </summary>
        public void ViewModelSet()
        {
            var commands = ViewModel.Presenter.Provider
               .GetServices<IMvpCommand>()
               .Where(x => true);

            if (commands != null)
            {
                SetSupportedButtons(); // Get list of supported buttons

                // Build supported button items for this presenter (using order)
                ViewModel.ButtonItems = new ObservableCollection<Button>();
                foreach (var commandTypeName in SupportedButtons)
                {
                    // SupportedButtons will have command type names
                    // find a match within our list of commands
                    var command = commands
                        .FirstOrDefault(c => c.Name == commandTypeName);

                    if (command == null) continue; // Not found

                    // Create a button using the commands button text value
                    var buttonVm = new Button
                    {
                        Margin = new Thickness(2, 10),
                        Text = command.ButtonText,
                        Style = (Style) // Apply the same style used in XAML
                            Application.Current.Resources["ButtonOutline"]
                    };

                    // Note that button Command and CommandParameter will
                    // be handled by the App.xaml x:DataType=Button declaration
                    // by default.  Which invokes ViewModel.ButtonClickHandler
                    ViewModel.ButtonItems.Add(buttonVm);
                }
            }
            OnViewModelSet();
        }

        /// <summary>
        /// Intended to be overriden. Invoked when the ViewModel is set and
        /// ViewModel.ButtonItems has been set (with supported button commands)
        /// </summary>
        public virtual void OnViewModelSet()
        {

        }
    }
}

