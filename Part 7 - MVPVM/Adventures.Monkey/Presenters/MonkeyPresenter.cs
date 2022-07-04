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

        /// <summary>
        /// Set the buttons we'll be using for the MainPage
        /// </summary>
        protected override void SetSupportedButtons()
        {
            SupportedButtons = new List<string>
            {
                nameof(FindClosestCommand),
                nameof(GetMonkeyListCommand),
                "GotoInventoryCommand" // Out of scope (no reference to proj)
            };
        }

        /// <summary>
        /// When the internet connectivity changes we'll need to update our
        /// service (IMonkeyDataService below) and invoke the command associated
        /// with the get list command (GetMonkeyListCommand)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        /// <summary>
        /// Invoked directly by the DefaultCommand if a valid command cannot
        /// be found by PresenterBase.ButtonClickHandler(ButtonEventArgs).  It
        /// is expected that ButtonEventArgs.IsHandledByPresenter will be set
        /// to false if it is not handled by the presenter so that the default
        /// message can be displayed upon return to DefaultCommand.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        public override async Task OnButtonClickHandler(
            object sender = null, EventArgs e = null)
        {
            var args = e as ButtonEventArgs;
            if (args == null) return; // Not handled here

            args.IsHandledByPresenter = true; // default is handled here

            switch (args.Key)
            {
                case AppConstants.Offline:
                    args.IsHandledByPresenter = true;
                    await MessageBox.Show("MonkeyPresenter", args.Key, ":( OK");
                    break;

                case AppConstants.Online:
                    args.IsHandledByPresenter = true;
                    await MessageBox.Show("MonkeyPresenter", args.Key, ":) OK");
                    break;

                default:
                    args.IsHandledByPresenter = false; // Not handled here
                    break;
            }
            await base.OnButtonClickHandler();
        }


        /// <summary>
        /// Invoked by MauiProgram.CreateMauiApp()
        /// </summary>
        /// <param name="builder"></param>
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
