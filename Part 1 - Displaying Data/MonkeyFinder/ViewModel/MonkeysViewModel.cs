using MonkeyFinder.Services;

namespace MonkeyFinder.ViewModel;

public partial class MonkeysViewModel : BaseViewModel
{
    MonkeyService _monkeyService;
    public ObservableCollection<Monkey> Monkeys { get; } = new();
    public MonkeysViewModel(MonkeyService monkeyService)
    {
        _monkeyService = monkeyService;
        Title = "Monkey Finder";
    }

    [RelayCommand]
    private async Task GetMonkeysAsync()
    {
        if(IsBusy)
            return;

        try
        {
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
