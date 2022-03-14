using MonkeyFinder.Services;

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

		builder.Services.AddHttpClient<MonkeyService>();
		builder.Services.AddSingleton<MonkeyService>();
		builder.Services.AddTransient<MonkeysViewModel>();
		builder.Services.AddTransient<MonkeyDetailsViewModel>();

		return builder.Build();
	}
}
