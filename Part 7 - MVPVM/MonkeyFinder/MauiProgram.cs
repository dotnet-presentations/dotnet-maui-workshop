#pragma warning disable CA1416

using Adventures.Commands;
using Adventures.Common;
using Adventures.Common.Interfaces;
using Adventures.ViewModel;
using MonkeyFinder.Interfaes;
using MonkeyFinder.Presenters;
using MonkeyFinder.View;

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

		// Shared commands
		builder.Services.AddSingleton<IMvpCommand, MessageCommand>();
		builder.Services.AddSingleton<IMvpCommand, ClosestItemCommand>();
		builder.Services.AddSingleton<IMvpCommand, ShowonMapCommand>();

		// Views
		builder.Services.AddTransient<MainPage>();
		builder.Services.AddTransient<InventoryPage>();
		builder.Services.AddTransient<DetailsPage>(); // Shared 

		// ViewModels
		builder.Services.AddTransient<IListViewModel, ListViewModel>(); // Shared 
		builder.Services.AddTransient<IDetailViewModel>( provider =>    // Shared 
		{
			var state = Shell.Current.CurrentState.Location.OriginalString;
			var isNotInventory = !state.ToLower().Contains("inventory");
			return new DetailsViewModel
			{
				IsPopulationVisible = isNotInventory,
				Presenter = isNotInventory
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
