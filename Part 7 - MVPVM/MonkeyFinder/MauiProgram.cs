#pragma warning disable CA1416

namespace MonkeyFinder;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
			});

    	builder.Services
			.AddSingleton<IConnectivity>(Connectivity.Current)
			.AddSingleton<IGeolocation>(Geolocation.Default)
			.AddSingleton<IMap>(Map.Default)
			.AddSingleton<IMvpEventAggregator, SimpleEventAggregator>()

			// Shared commands
			.AddSingleton<IMvpCommand, MessageCommand>()
			.AddSingleton<IMvpCommand, FindClosestCommand>()
			.AddSingleton<IMvpCommand, ShowonMapCommand>()

			// Views
			.AddTransient<MainPage>()
			.AddTransient<DetailsPage>() // Shared 

			// ViewModels
			.AddTransient<IListViewModel, ListViewModel>()	// Shared 
			.AddTransient<IDetailViewModel>( provider =>    // Shared 
			{
				// Configure view model with applicable presenter and
				// IsPopulationVisible value.  We don't want to show 
				// population on the inventory details page
				var state = Shell.Current.CurrentState.Location.OriginalString;
				var isInventoryPage = state.Contains(nameof(InventoryPage));

				return new DetailsViewModel
				{
                    // Set presenter and IsPopulationVisible based
                    // on the the current page  
                    IsPopulationVisible = !isInventoryPage,
					Presenter = isInventoryPage 
						? provider.GetService<IInventoryPresenter>()
						: provider.GetService<IMonkeyPresenter>()
				};
			});

		// Reused by both MonkeyPresenter and InventoryPresenter
        Routing.RegisterRoute(nameof(DetailsPage), typeof(DetailsPage));

        // Initialize services. 
        MonkeyPresenter.InitServices(builder);
        InventoryPresenter.InitServices(builder);

		var serviceBuilder =  builder.Build();
		return serviceBuilder;
	}
}
