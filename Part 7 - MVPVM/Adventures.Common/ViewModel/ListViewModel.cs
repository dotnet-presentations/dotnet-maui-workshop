#pragma warning disable CA1416

namespace Adventures.Common.ViewModel;

public partial class ListViewModel : BaseViewModel, IListViewModel
{
    public ObservableCollection<ListItem> ListItems {  get;  } = new();

    [ObservableProperty]
    [AlsoNotifyChangeFor(nameof(IsNotBusy))]
    bool isBusy;

    public bool IsNotBusy => !IsBusy;

    [ObservableProperty]
    string title;

    [ObservableProperty]
    bool isRefreshing;

    [ObservableProperty]
    string mode;

    [ObservableProperty]
    string getDataButtonText;

    [ObservableProperty]
    string getInventoryButtonText;

    public ListViewModel()
    {
    }
}
