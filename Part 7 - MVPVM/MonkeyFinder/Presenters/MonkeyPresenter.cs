#pragma warning disable CA1416

using Adventures.Common.Interfaces;
using Adventures.Common.Presenters;
using MonkeyFinder.Commands;
using MonkeyFinder.Interfaes;
using MonkeyFinder.Services;

namespace MonkeyFinder.Presenters
{
    public class MonkeyPresenter : PresenterBase, IMonkeyPresenter
	{
        IDataService _dataService;

        // Handled view models
        IListViewModel _listVm;

        public MonkeyPresenter(IMonkeyDataService dataService, IServiceProvider provider, IListViewModel listVm)
            : base(provider)
        {
            // Retrieve data service so we can get mode (online or offline)
            _dataService = dataService;

            // Resolve view models so they can be configured
            _listVm = listVm;
        }

        public override void Initialize(object sender = null, EventArgs e = null)
        {
            // Configure the view models this presenter will handle

            _listVm.GetDataButtonText = AppConstants.GetListButtonText;
            _listVm.Title = "Monkey Locator";
            _listVm.Mode = _dataService.Mode;
            _listVm.Presenter = this;

            ViewModel = _listVm;
        }

        public static void InitServices(MauiAppBuilder builder)
        {
            builder.Services.AddSingleton<IMonkeyPresenter, MonkeyPresenter>();
            builder.Services.AddSingleton<IMvpCommand, GotoSelectedMonkeyCommand>();
            builder.Services.AddSingleton<IMvpCommand, GetMonkeyListCommand>();

            builder.Services.AddSingleton<IMonkeyDataService>(provider =>
            {
                IConnectivity connectivity = provider
                    .GetServices<IConnectivity>().FirstOrDefault();

                return connectivity.NetworkAccess != NetworkAccess.Internet
                  ? new MonkeyOfflineService()
                  : new MonkeyOnlineService();
            });
        }
    }
}
