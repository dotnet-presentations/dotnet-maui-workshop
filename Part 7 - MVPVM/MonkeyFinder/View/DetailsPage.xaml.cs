#pragma warning disable CA1416

using Adventures.Common.Interfaces;

namespace MonkeyFinder.View;

public partial class DetailsPage : ContentPage
{
	public DetailsPage(IDetailViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}