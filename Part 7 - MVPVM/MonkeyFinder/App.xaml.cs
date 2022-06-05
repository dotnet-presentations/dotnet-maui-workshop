#pragma warning disable CA1416

using Adventures.Common.Interfaces;

namespace MonkeyFinder;

public partial class App : Application
{
	public App(IPresenter presenter)
	{
		presenter.Initialize(this);

		InitializeComponent();

		MainPage = new AppShell();
	}
}
