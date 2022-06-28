#pragma warning disable CA1416

using Adventures.Common.Entities;

namespace Adventures.Common.ViewModel;

public partial class ListViewModel : BaseViewModel, IListViewModel
{
    public ObservableCollection<ListItem> ListItems {  get;  } = new();

    public ObservableCollection<CommandItem> CommandItems { get; set; }

    [ObservableProperty]
    [AlsoNotifyChangeFor(nameof(IsNotBusy))]
    bool isBusy;

    public bool IsNotBusy => !IsBusy;


    [ObservableProperty]
    string title;

    [ObservableProperty]
    bool isRefreshing;

    [ObservableProperty]
    string getDataButtonText;

    [ObservableProperty]
    string getDataButton2Text;

    [ObservableProperty]
    string getDataButton3Text;

    public ListViewModel()
    {
    }
}
