#pragma warning disable CA1416

namespace Adventures.Common.Views;

public partial class DetailsPage : ContentPageBase
{
	public DetailsPage(IDetailViewModel viewModel) 
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}
