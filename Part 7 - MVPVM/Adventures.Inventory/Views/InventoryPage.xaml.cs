#pragma warning disable CA1416

namespace Adventures.Inventory.Views;

public partial class InventoryPage : ContentPageBase
{
	public InventoryPage(IInventoryPresenter presenter) : base(presenter)
	{
		InitializeComponent();
        OnInitializeComponent(flexLayout);
    }
}

