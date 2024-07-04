namespace MonkeyFinder.Controls;

public class StandardPage : ContentPage
{
	public StandardPage()
	{
		App.Current.Resources.TryGetValue(AppColors.LightBackground, out object lightColor);
		App.Current.Resources.TryGetValue(AppColors.DarkBackground, out object darkColor);

		this.SetAppThemeColor(ContentPage.BackgroundColorProperty, lightColor as Color, darkColor as Color);
	}
}
