#pragma warning disable CA1416

namespace MonkeyFinder;

public partial class App : Application
{
	public App()
	{
		InitializeComponent();

		MainPage = new AppShell();
	}
}
