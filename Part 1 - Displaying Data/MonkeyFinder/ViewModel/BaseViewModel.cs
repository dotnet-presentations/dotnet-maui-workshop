namespace MonkeyFinder.ViewModel;

public partial class BaseViewModel : ObservableObject
{
    public BaseViewModel()
    {
        
    }
    
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsNotBusy))]
    private bool isBusy;
    
    [ObservableProperty]
    private string title;

    public bool IsNotBusy => !IsBusy;
}
