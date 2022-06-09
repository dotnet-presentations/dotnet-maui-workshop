#pragma warning disable CA1416

using Adventures.Monkey.Services.Online;

namespace MonkeyFinder.Presenters
{
    public class InventoryPresenter : PresenterBase, IInventoryPresenter
	{
        IDataService _dataService;

        // Handled view models
        IListViewModel _listVm;

        public InventoryPresenter(IInventoryDataService dataService, IServiceProvider provider, IListViewModel listVm)
            : base(provider)
        {
            _dataService = dataService;
            _listVm = listVm;
        }

        public override void Initialize(object sender = null, EventArgs e = null) 
        {
            base.Initialize(sender, e);

            _listVm.GetDataButtonText = AppConstants.GetListButtonText;
            _listVm.GetInventoryButtonText = AppConstants.GetInventoryButtonText;
            _listVm.Title = "Inventory";
            _listVm.Mode = _dataService.Mode;
            _listVm.Presenter = this;

            ViewModel = _listVm; // Used by page
        }

        public static void InitServices(MauiAppBuilder builder)
        {
            Routing.RegisterRoute(nameof(InventoryPage), typeof(InventoryPage));

            builder.Services.AddSingleton<IInventoryPresenter, InventoryPresenter>();
            builder.Services.AddTransient<IMvpCommand, GotoInventoryCommand>();
            builder.Services.AddTransient<IMvpCommand, GetInventoryListCommand>();
            builder.Services.AddTransient<IInventoryDataService>(provider =>
            {
                IConnectivity connectivity = provider.GetServices<IConnectivity>().FirstOrDefault();

                return connectivity.NetworkAccess != NetworkAccess.Internet
                  ? new InventoryOfflineService()
                  : new InventoryOnlineService();
            });
        }

    }
}
