#pragma warning disable CA1416

using MonkeyFinder.Interfaes;

namespace MonkeyFinder.View;

public partial class MainPage : ContentPage
{
	public MainPage(IMonkeyPresenter presenter)
	{
		presenter.Initialize(this);

        InitializeComponent();
		BindingContext = presenter.ViewModel;
	}
}

