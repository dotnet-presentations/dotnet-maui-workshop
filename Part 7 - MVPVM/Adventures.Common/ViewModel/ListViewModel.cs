#pragma warning disable CA1416

using System.Collections.ObjectModel;
using Adventures.Common.Interfaces;
using Adventures.Common.Model;
using CommunityToolkit.Mvvm.ComponentModel;
using MonkeyFinder.ViewModel;

namespace Adventures.ViewModel;

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
