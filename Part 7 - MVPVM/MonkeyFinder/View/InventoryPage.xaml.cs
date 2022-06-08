#pragma warning disable CA1416

using MonkeyFinder.Interfaes;

namespace MonkeyFinder.View;

public partial class InventoryPage : ContentPage
{
	public InventoryPage(IInventoryPresenter presenter)
	{
		presenter.Initialize(this);

		InitializeComponent();
		BindingContext = presenter.ViewModel;
	}
}

