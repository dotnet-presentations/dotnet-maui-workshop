namespace MonkeyFinder;

public partial class DetailsPage : ContentPage
{
	public DetailsPage(MonkeyDetailsViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
	
	protected override void OnNavigatedTo(NavigatedToEventArgs e)
	{
		base.OnNavigatedTo(e);
	}
}