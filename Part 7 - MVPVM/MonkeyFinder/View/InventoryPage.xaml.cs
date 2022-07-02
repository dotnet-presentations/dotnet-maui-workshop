﻿#pragma warning disable CA1416

using Adventures.Inventory.Interfaces;

namespace MonkeyFinder.View;

public partial class InventoryPage : ContentPageBase
{
	public InventoryPage(IInventoryPresenter presenter) : base(presenter)
	{
		InitializeComponent();
	}
}
