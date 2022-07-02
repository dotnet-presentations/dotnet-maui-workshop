#pragma warning disable CA1416

using Adventures.Commands;

namespace Adventures.Monkey.Presenters
{
    public class MonkeyPresenter : PresenterBase, IMonkeyPresenter
	{
        public MonkeyPresenter(
              IServiceProvider provider
            , IMvpEventAggregator eventAggregator
            , IMonkeyDataService dataService
            , IListViewModel listVm)
            : base(provider, eventAggregator, dataService, listVm)
        {
            Title = "Monkey Locator";
        }

        protected override void SetSupportedButtons()
        {
            SupportedButtons = new List<string>
            {
                nameof(FindClosestCommand),
                nameof(GetMonkeyListCommand),
                "GotoInventoryCommand"
            };
        }

        protected override void OnInternetConnectivityChanged(
            object sender, ConnectivityEventArgs e)
        {
            // Get list of available commands so we can determine button text
            var buttonCommands = Provider.GetNamedCommands();

            // Update view model and invoke get monkeys command (refresh data)
            ViewModel.Mode = Provider.GetService<IMonkeyDataService>().Mode;
            InvokeCommand(buttonCommands[nameof(GetMonkeyListCommand)]);

            // Update Monkey View Model with mode and publish event
            var messageArgs = new MessageEventArgs { Message = ViewModel.Mode };
            EventAggregator.Publish<MessageEventArgs>(messageArgs);
        }

        // Invoked by MauiProgram.CreateMauiApp()
        public static void InitServices(MauiAppBuilder builder)
        {
            builder.Services
                .AddSingleton<IMonkeyPresenter, MonkeyPresenter>()
                .AddTransient<IMvpCommand, GotoSelectedMonkeyCommand>()
                .AddTransient<IMvpCommand, GetMonkeyListCommand>()
                .AddTransient<IMonkeyDataService>(provider =>
                {
                    // Each request for data service will return service based
                    // on the connectivity status [OFFLINE/ONLINE]
                    var connectivity = provider.GetService<IConnectivity>();
                    return connectivity.NetworkAccess != NetworkAccess.Internet
                      ? new MonkeyOfflineService()
                      : new MonkeyOnlineService();
                });
        }
    }
}
