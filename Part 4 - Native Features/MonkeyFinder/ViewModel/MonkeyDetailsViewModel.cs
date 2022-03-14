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
}
