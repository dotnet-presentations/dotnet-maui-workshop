namespace MonkeyFinder.View;

public partial class MainPage : StandardPage
{
	public MainPage(MonkeysViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}

