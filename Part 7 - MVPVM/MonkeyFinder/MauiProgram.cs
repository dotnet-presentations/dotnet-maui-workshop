#pragma warning disable CA1416

using Adventures.Common.Events;
using Adventures.Common.ViewModel;
using Adventures.Inventory.Views;

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

    	builder.Services.AddSingleton<IConnectivity>(Connectivity.Current);
		builder.Services.AddSingleton<IGeolocation>(Geolocation.Default);
		builder.Services.AddSingleton<IMap>(Map.Default);

		builder.Services.AddSingleton<IMvpEventAggregator, SimpleEventAggregator>();

		// Shared commands
		builder.Services.AddSingleton<IMvpCommand, MessageCommand>();
		builder.Services.AddSingleton<IMvpCommand, FindClosestCommand>();
		builder.Services.AddSingleton<IMvpCommand, ShowonMapCommand>();

		// Views
		builder.Services.AddTransient<MainPage>();
		builder.Services.AddTransient<DetailsPage>(); // Shared 

		// ViewModels
		builder.Services.AddTransient<IListViewModel, ListViewModel>(); // Shared 
		builder.Services.AddTransient<IDetailViewModel>( provider =>    // Shared 
		{
			// Configure view model with applicable presenter and IsPopulationVisible
			// value.  We don't want to show it on Inventory details page
			var state = Shell.Current.CurrentState.Location.OriginalString;
			var isNotInventory = !state.ToLower().Contains(nameof(InventoryPage));
			return new DetailsViewModel
			{
				IsPopulationVisible = isNotInventory,
				Presenter = isNotInventory // assign the applicable presenter
					? provider.GetService<IMonkeyPresenter>()
					: provider.GetService<IInventoryPresenter>()
            };
        });

		// Encapsulated services
		MonkeyPresenter.InitServices(builder);
		InventoryPresenter.InitServices(builder);

		var serviceBuilder =  builder.Build();
		return serviceBuilder;

	}
}
