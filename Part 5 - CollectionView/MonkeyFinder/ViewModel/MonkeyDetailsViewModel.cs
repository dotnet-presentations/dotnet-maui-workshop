using MonkeyFinder.Services;

namespace MonkeyFinder.ViewModel;

[QueryProperty("Monkey", "Monkey")]
public partial class MonkeyDetailsViewModel : BaseViewModel
{
    MonkeyService monkeyService;
    public MonkeyDetailsViewModel(MonkeyService monkeyService)
    {
        this.monkeyService = monkeyService;
    }

    [ObservableProperty]
    Monkey monkey;

    [ICommand]
    async Task OpenMap()
    {
        try
        {
            await Map.OpenAsync(Monkey.Latitude, Monkey.Longitude);
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Unable to launch maps: {ex.Message}");
            await Application.Current.MainPage.DisplayAlert("Error, no Maps app!", ex.Message, "OK");
        }
    }
}
