#pragma warning disable CA1416

using System.Diagnostics;
using Adventures.Common.Extensions;

namespace MonkeyFinder.Presenters
{
    public class InventoryPresenter : PresenterBase, IInventoryPresenter
	{
        IMvpEventAggregator _eventAggregator;
        IServiceProvider _provider;
        IDataService _dataService;
        IListViewModel _listVm;

        // Use constructor injection to get required class instances
        public InventoryPresenter(
            IMvpEventAggregator eventAggregator,
            IInventoryDataService dataService,
            IServiceProvider provider,
            IListViewModel listVm)  : base(provider)
        {
            _eventAggregator = eventAggregator;
            _dataService = dataService;
            _provider = provider;
            _listVm = listVm;
        }

        // Invoked by the InventoryPage:ContentPageBase constructor.  The
        // IOC constructor injector will pass in IMvpPresenter instance
        public override void Initialize(object sender = null, EventArgs e = null) 
        {
            // as a singleton we only need to do this once
            if (this.IsInitialized) return; 

            base.Initialize(sender, e);

            // Get list of available commands so we can determine button text
            var nameFor = _provider.GetNamedCommands();

            // Use magic string when type not available (proj not referenced)
            _listVm.ButtonText1 = nameFor["GetMonkeyListCommand"];
            _listVm.ButtonText2 = nameFor[nameof(GetInventoryListCommand)];

            _listVm.Title = "Inventory";
            _listVm.Mode = _dataService.Mode;

            _listVm.Presenter = this;
            ViewModel = _listVm; // Used by page

            // Update view model on connectivity changes
            _eventAggregator.Subscribe<MessageEventArgs>(this, "Mode", (sender) =>
            {
                _listVm.Mode = sender.Message;
                InvokeCommand(_listVm.ButtonText2); // Button click to get Inventory list
            });
        }

        public static void InitServices(MauiAppBuilder builder)
        {
            Routing.RegisterRoute(nameof(InventoryPage), typeof(InventoryPage));

            builder.Services.AddSingleton<IInventoryPresenter, InventoryPresenter>();

            builder.Services.AddTransient<InventoryPage>();
            builder.Services.AddTransient<IMvpCommand, GotoInventoryCommand>();
            builder.Services.AddTransient<IMvpCommand, GetInventoryListCommand>();

            builder.Services.AddTransient<IInventoryDataService>(provider =>
            {
                // The implementation of IInventoryDataService will be determined by connection status
                IConnectivity connectivity = provider.GetServices<IConnectivity>().FirstOrDefault();
                return connectivity.NetworkAccess != NetworkAccess.Internet
                  ? new InventoryOfflineService()
                  : new InventoryOnlineService();
            });
        }
    }
}
