#pragma warning disable CA1416

namespace Adventures.Monkey.Presenters
{
    public class MonkeyPresenter : PresenterBase, IMonkeyPresenter
	{
        IDataService _dataService;

        // Handled view models
        IListViewModel _listVm;

        public MonkeyPresenter(IMonkeyDataService dataService, IServiceProvider provider, IListViewModel listVm)
            : base(provider)
        {
            _dataService = dataService;
            _listVm = listVm;
        }

        public override void Initialize(object sender = null, EventArgs e = null)
        {
            base.Initialize(sender, e);

            Routing.RegisterRoute(nameof(DetailsPage), typeof(DetailsPage));

            // Configure the view model this presenter will handle
            _listVm.GetDataButtonText = AppConstants.GetListButtonText;
            _listVm.Title = "Monkey Locator";
            _listVm.Mode = _dataService.Mode;
            _listVm.Presenter = this;

            ViewModel = _listVm;
        }

        public static void InitServices(MauiAppBuilder builder)
        {
            builder.Services.AddSingleton<IMonkeyPresenter, MonkeyPresenter>();
            builder.Services.AddTransient<IMvpCommand, GotoSelectedMonkeyCommand>();
            builder.Services.AddTransient<IMvpCommand, GetMonkeyListCommand>();
            builder.Services.AddTransient<IMonkeyDataService>(provider =>
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
