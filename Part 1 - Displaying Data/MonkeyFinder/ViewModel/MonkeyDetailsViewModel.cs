namespace MonkeyFinder.ViewModel;

[QueryProperty("Monkey", "Monkey")]
public partial class MonkeyDetailsViewModel : BaseViewModel
{
    public MonkeyDetailsViewModel()
    {
        
    }
    
    [ObservableProperty]
    private Monkey _monkey;
}
