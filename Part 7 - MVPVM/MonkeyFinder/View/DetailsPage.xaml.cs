#pragma warning disable CA1416

namespace MonkeyFinder.View;

public partial class DetailsPage : ContentPageBase
{
	public DetailsPage(IDetailViewModel viewModel) 
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}
