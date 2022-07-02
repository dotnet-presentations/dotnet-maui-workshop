#pragma warning disable CA1416

using Adventures.Commands;

namespace MonkeyFinder.Presenters
{
    public class InventoryPresenter : PresenterBase, IInventoryPresenter
	{
        // Use constructor injection to get required class instances
        public InventoryPresenter(
            IMvpEventAggregator eventAggregator
            , IInventoryDataService dataService
            , IServiceProvider provider
            , IListViewModel listVm)
            : base(provider, eventAggregator, dataService, listVm)
        {
            Title = "Inventory";
        }

        protected override void SetSupportedButtons()
        {
            SupportedButtons = new List<string>
            {
                nameof(FindClosestCommand),
                nameof(GetInventoryListCommand)
            };
        }

        // Invoked by the InventoryPage:ContentPageBase constructor.  The
        // IOC constructor injector will pass in IMvpPresenter instance
        protected override void OnInitialize(
            object sender = null, EventArgs e = null) 
        {
            // Update view model on connectivity changes
            EventAggregator.Subscribe<MessageEventArgs>(this, (sender) =>
            {
                ViewModel.Mode = sender.Message;

                // Invoke button click to refresh inventory list for mode
                var buttonCommands = Provider.GetNamedCommands();
                InvokeCommand(buttonCommands[nameof(GetInventoryListCommand)]); 
            });
        }

        public static void InitServices(MauiAppBuilder builder)
        {
            Routing.RegisterRoute(nameof(InventoryPage), typeof(InventoryPage));

            builder.Services
                .AddSingleton<IInventoryPresenter, InventoryPresenter>()

                .AddTransient<InventoryPage>()
                .AddTransient<IMvpCommand, GotoInventoryCommand>()
                .AddTransient<IMvpCommand, GetInventoryListCommand>()
                .AddTransient<IInventoryDataService>(provider =>
                {
                    // The implementation of IInventoryDataService will be
                    // determined by connection status
                    var connectivity = provider.GetService<IConnectivity>();
                    return connectivity.NetworkAccess != NetworkAccess.Internet
                      ? new InventoryOfflineService()
                      : new InventoryOnlineService();
                });
        }
    }
}
