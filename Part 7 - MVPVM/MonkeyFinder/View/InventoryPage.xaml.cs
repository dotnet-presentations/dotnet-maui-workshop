#pragma warning disable CA1416

using MonkeyFinder.Presenters;

namespace MonkeyFinder.View;

public partial class InventoryPage : ContentPage
{
	public InventoryPage(InventoryPresenter presenter)
	{
		presenter.Initialize(this);

		InitializeComponent();
		BindingContext = presenter.ViewModel;
	}
}

