namespace MonkeyFinder.ViewModel;

[QueryProperty(nameof(Monkey), "Monkey")]
public partial class MonkeyDetailsViewModel : BaseViewModel
{
    public MonkeyDetailsViewModel()
    {
    }

    [ObservableProperty]
    public partial Monkey Monkey { get; set; }
}
