namespace MonkeyFinder.Controls;

[ContentProperty(nameof(Children))]
public class VerticallyScrollingPage : StandardPage
{
	private readonly ScrollView scrollView;
	private readonly VerticalStackLayout verticalLayout;

	public IList<IView> Children => verticalLayout.Children;

	public VerticallyScrollingPage()
	{
		verticalLayout = new VerticalStackLayout { Padding = 0 };
		scrollView = new ScrollView
		{
			Content = verticalLayout
		};

		base.Content = scrollView;
	}
}
