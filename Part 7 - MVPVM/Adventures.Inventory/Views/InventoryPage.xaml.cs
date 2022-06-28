#pragma warning disable CA1416

using System.Diagnostics;
using Adventures.Common.Controls;
using Adventures.Inventory.Interfaces;

namespace Adventures.Inventory.Views;

public partial class InventoryPage : ContentPageBase
{
	public InventoryPage(IInventoryPresenter presenter) : base(presenter)
	{
		InitializeComponent();
	}
}

