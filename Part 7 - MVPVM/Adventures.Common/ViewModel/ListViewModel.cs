#pragma warning disable CA1416

namespace Adventures.Common.ViewModel;

public partial class ListViewModel : BaseViewModel, IListViewModel
{
    public ObservableCollection<ListItem> ListItems {  get;  } = new();

    public ObservableCollection<CommandItem> CommandItems { get; set; }

    public bool IsNotBusy => !IsBusy;

    [ObservableProperty]
    [AlsoNotifyChangeFor(nameof(IsNotBusy))]
    bool isBusy;

    [ObservableProperty]
    string title;

    [ObservableProperty]
    bool isRefreshing;

    [ObservableProperty]
    string buttonText1;

    [ObservableProperty]
    string buttonText2;

    [ObservableProperty]
    string buttonText3;

    [ObservableProperty]
    string matchButtonText = "Hello";

    public ListViewModel() { }
}
