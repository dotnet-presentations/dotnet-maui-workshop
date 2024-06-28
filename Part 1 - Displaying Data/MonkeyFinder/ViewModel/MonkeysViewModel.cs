using MonkeyFinder.Services;

namespace MonkeyFinder.ViewModel;

public partial class MonkeysViewModel : BaseViewModel
{
    private readonly MonkeyService _monkeyService;
    public ObservableCollection<Monkey> Monkeys { get; } = [];
    private IConnectivity _connectivity;
    private IGeolocation _geolocation;
    public MonkeysViewModel(MonkeyService monkeyService, IConnectivity connectivity, IGeolocation geolocation)
    {
        Title = "Monkey Finder";
        _monkeyService = monkeyService;
        _connectivity = connectivity;
        _geolocation = geolocation;
    }

    [RelayCommand]
    private async Task GoToDetailsAsync(Monkey monkey)
    {
        if(monkey is null)
            return;

        await Shell.Current.GoToAsync($"{nameof(DetailsPage)}", true,
            new Dictionary<string, object>
            {
                {"Monkey", monkey}
            });
    }

    [RelayCommand]
    private async Task GetClosestMonkeyAsync()
    {
        if(IsBusy || Monkeys.Count == 0)
            return;

        try
        {
            var location = await _geolocation.GetLastKnownLocationAsync();
            if(location is null)
            {
                location = await _geolocation.GetLocationAsync(
                new GeolocationRequest
                {
                    DesiredAccuracy = GeolocationAccuracy.Medium,
                    Timeout = TimeSpan.FromSeconds(30)
                });
            }
            
            if (location is null)
                return;

            var first = Monkeys.MinBy(m =>
                location.CalculateDistance(m.Latitude, m.Longitude, DistanceUnits.Kilometers));
            
            if(first is null)
                return;
            
            await Shell.Current.DisplayAlert("Closest Monkey",
                $"The closest monkey is {first.Name} in location {first.Location}", "OK");
        }
        catch (Exception e)
        {
            Debug.WriteLine($"Error: {e}");
            await Shell.Current.DisplayAlert("Error!",
                $"Unable to get closest monkey {e.Message}", "OK");
        }
    }

    [RelayCommand]
    private async Task GetMonkeysAsync()
    {
        if(IsBusy)
            return;

        try
        {
            if(_connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                await Shell.Current.DisplayAlert("No Internet!",
                    "Please check your internet connection", "OK");
                return;
            }
            
            IsBusy = true;
            var monkeys = await _monkeyService.GetMonkeys();

            if (Monkeys.Count != 0)
                Monkeys.Clear();
            
            foreach (var monkey in monkeys)
                Monkeys.Add(monkey);
        }
        catch (Exception e)
        {
            Debug.WriteLine($"Error: {e}");
            await Shell.Current.DisplayAlert("Error!",
                $"Unable to get monkeys {e.Message}", "OK");
        }
        finally
        {
            IsBusy = false;
        }
    }
}
