namespace MonkeyFinder.View;

public partial class MainPage : ContentPage
{
	public MainPage(MonkeysViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}

	private async void CollectionView_SelectionChanged(object sender, SelectionChangedEventArgs e)
	{
		var monkey = e.CurrentSelection.FirstOrDefault() as Monkey;
		if (monkey == null)
			return;

		await Shell.Current.GoToAsync(nameof(DetailsPage), true, new Dictionary<string, object>
		{
			{"Monkey", monkey }
		});

		((CollectionView)sender).SelectedItem = null;
	}
}

