namespace MonkeyFinder;

public partial class DetailsPage : StandardPage
{
	public DetailsPage(MonkeyDetailsViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}