using Microsoft.Extensions.Logging;
using MonkeyFinder.Services;
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

#if DEBUG
		builder.Logging.AddDebug();
#endif

		builder.Services.AddSingleton<HttpClient>();
		builder.Services.AddSingleton<MonkeyService>();
		builder.Services.AddSingleton<MonkeysViewModel>();
		builder.Services.AddTransient<MonkeyDetailsViewModel>();
		builder.Services.AddTransient<DetailsPage>();
		builder.Services.AddSingleton<MainPage>();

		return builder.Build();
	}
}
