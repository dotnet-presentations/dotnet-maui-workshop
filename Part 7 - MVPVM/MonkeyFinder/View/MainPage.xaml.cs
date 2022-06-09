#pragma warning disable CA1416

namespace MonkeyFinder.View;

public partial class MainPage : ContentPageBase
{
	public MainPage(IMonkeyPresenter presenter) : base(presenter)
	{
        InitializeComponent();
	}
}

