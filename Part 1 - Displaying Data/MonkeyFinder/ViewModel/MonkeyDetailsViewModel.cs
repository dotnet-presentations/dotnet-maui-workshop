namespace MonkeyFinder.ViewModel;

[QueryProperty("Monkey", "Monkey")]
public partial class MonkeyDetailsViewModel : BaseViewModel
{
    private IMap _map;
    public MonkeyDetailsViewModel(IMap map)
    {
        _map = map;
    }
    
    [ObservableProperty]
    private Monkey _monkey;
    
    [RelayCommand]
    private async Task GoBackAsync()
    {
        await Shell.Current.GoToAsync("..");
    }
    
    [RelayCommand]
    private async Task OpenMapAsync()
    {
        try
        {
            await _map.OpenAsync(Monkey.Latitude, Monkey.Longitude,
            new MapLaunchOptions
            {
                Name = Monkey.Name,
                NavigationMode = NavigationMode.None
            });
            
        }
        catch (Exception e)
        {
            Debug.WriteLine($"Error: {e}");
            await Shell.Current.DisplayAlert("Error!",
                $"Unable to open map {e.Message}", "OK");
        }
    }
}
