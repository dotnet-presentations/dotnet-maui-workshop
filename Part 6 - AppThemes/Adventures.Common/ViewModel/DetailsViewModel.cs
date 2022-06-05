#pragma warning disable CA1416

using Adventures.Common.Interfaces;
using Adventures.Common.Model;
using CommunityToolkit.Mvvm.ComponentModel;
using MonkeyFinder.ViewModel;

namespace Adventures.ViewModel;

[QueryProperty(nameof(ListItem), "ListItem")]
public partial class DetailsViewModel : BaseViewModel, IDetailViewModel
{
    public DetailsViewModel() { }

    [ObservableProperty]
    ListItem listItem;

    [ObservableProperty]
    bool isPopulationVisible;

}

