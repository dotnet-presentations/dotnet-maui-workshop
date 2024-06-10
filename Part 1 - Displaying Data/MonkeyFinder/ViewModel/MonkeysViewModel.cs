using MonkeyFinder.Services;
namespace MonkeyFinder.ViewModel;

public partial class MonkeysViewModel : BaseViewModel
{
    MonkeyService monkeyService;
    public ObservableCollection<Monkey> Monkeys { get;} = new();

    public MonkeysViewModel(MonkeyService monkeyService)
    {
        this.monkeyService = new MonkeyService();
        Title = "Monkey Finder";
    }
    [RelayCommand]
    async Task GoToDetailsAsync(Monkey monkey)
    {
        if (monkey is null)
            return;

        await Shell.Current.GoToAsync($"{nameof(DetailsPage)}", true, 
            new Dictionary<string, object>
            {
                {"Monkey", monkey}
            });
    }

    [RelayCommand]
    async Task GetMonkeysAsync()
    {
        try
        {
            IsBusy = true;
            var monkeys = await monkeyService.GetMonkeys();

            if (Monkeys.Count != 0)
                Monkeys.Clear();

            foreach(var monkey in monkeys)
                Monkeys.Add(monkey);
            
        } 
        catch (Exception ex)
        {
            Debug.WriteLine(ex);
            await Shell.Current.DisplayAlert("Error!",
                $"Unable to get monkeys: {ex.Message}", "OK");
        } 
        finally
        {
            IsBusy = false;
        }
    }
}
