#pragma warning disable CA1416

using Adventures.Common.Interfaces;

namespace MonkeyFinder.View;

public partial class MainPage : ContentPage
{
	public MainPage(IPresenter presenter)
	{
		presenter.Initialize(this);

        InitializeComponent();
		BindingContext = presenter.ViewModel;
	}
}

