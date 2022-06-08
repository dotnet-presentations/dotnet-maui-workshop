#pragma warning disable CA1416

using Adventures.Common.Interfaces;
using Adventures.Common.Presenters;
using MonkeyFinder.Commands;
using MonkeyFinder.Interfaes;
using MonkeyFinder.Services;

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
            _listVm.GetDataButtonText = AppConstants.GetListButtonText;
            _listVm.GetInventoryButtonText = AppConstants.GetInventoryButtonText;
            _listVm.Title = "Inventory";
            _listVm.Mode = _dataService.Mode;
            _listVm.Presenter = this;

            ViewModel = _listVm; // Used by page
        }

        public static void InitServices(MauiAppBuilder builder)
        {
            builder.Services.AddSingleton<IInventoryPresenter, InventoryPresenter>();
            builder.Services.AddSingleton<IMvpCommand, GotoInventoryCommand>();
            builder.Services.AddSingleton<IMvpCommand, GetInventoryListCommand>();

            builder.Services.AddSingleton<IInventoryDataService>(provider =>
            {
                IConnectivity connectivity = provider.GetServices<IConnectivity>().FirstOrDefault();

                return connectivity.NetworkAccess != NetworkAccess.Internet
                  ? new InventoryOfflineService()
                  : new InventoryOnlineService();
            });
        }

    }
}
