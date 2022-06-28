#pragma warning disable CA1416

using System.Diagnostics;
using Adventures.Commands;
using Adventures.Common.Extensions;
using Adventures.Common.Utils;

namespace Adventures.Monkey.Presenters
{
    public class MonkeyPresenter : PresenterBase, IMonkeyPresenter
	{
        IMvpEventAggregator _eventAggregator;
        IDataService _dataService;
        IListViewModel _listVm;
        IServiceProvider _provider;

        ConnectivityUtil _connectivityUtil = new ConnectivityUtil();

        public MonkeyPresenter(
            IMvpEventAggregator eventAggregator,
            IMonkeyDataService dataService,
            IServiceProvider provider,
            IListViewModel listVm) : base(provider)
        {
            _eventAggregator = eventAggregator;
            _dataService = dataService;
            _provider = provider;
            _listVm = listVm;
        }

        public override void Initialize(object sender = null, EventArgs e = null)
        {
            base.Initialize(sender, e);

            Routing.RegisterRoute(nameof(DetailsPage), typeof(DetailsPage));

            // Get list of available commands so we can determine button text
            var nameFor = _provider.GetNamedCommands();

            // Use magic string when type not available (proj not referenced)
            _listVm.GetDataButtonText = nameFor[nameof(GetMonkeyListCommand)];
            _listVm.GetDataButton2Text = nameFor["GotoInventoryCommand"];
            _listVm.GetDataButton3Text = nameFor[nameof(FindClosestCommand)];

            _listVm.Title = "Monkey Locator";
            _listVm.Mode = _dataService.Mode;

            _listVm.Presenter = this; // give view model presenter reference
            ViewModel = _listVm;      // give presenter view model reference

            _connectivityUtil.ConnectivityChanged += (s, e) =>
            {
                // Update view model and invoke the GetMonkeys command (refresh data)
                _listVm.Mode = _provider.GetService<IMonkeyDataService>().Mode;
                InvokeCommand(_listVm.GetDataButtonText);

                // Update Monkey View Model with mode and publish event
                var messageArgs = new MessageEventArgs { Message = _listVm.Mode };
                _eventAggregator.Send<MessageEventArgs>(messageArgs, "Mode");
            };
        }

        // Invoked by MauiProgram.CreateMauiApp()
        public static void InitServices(MauiAppBuilder builder)
        {
            builder.Services.AddSingleton<IMonkeyPresenter, MonkeyPresenter>();

            builder.Services.AddTransient<IMvpCommand, GotoSelectedMonkeyCommand>();
            builder.Services.AddTransient<IMvpCommand, GetMonkeyListCommand>();
            builder.Services.AddTransient<IMonkeyDataService>(provider =>
            {
                IConnectivity connectivity = provider.GetService<IConnectivity>();

                return connectivity.NetworkAccess != NetworkAccess.Internet
                  ? new MonkeyOfflineService()
                  : new MonkeyOnlineService();
            });
        }
    }
}
