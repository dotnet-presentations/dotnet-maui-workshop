#pragma warning disable CA1416

using MonkeyFinder.View;

namespace MonkeyFinder;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();

		Routing.RegisterRoute(nameof(DetailsPage), typeof(DetailsPage));
		Routing.RegisterRoute(nameof(InventoryPage), typeof(InventoryPage));
	}
}