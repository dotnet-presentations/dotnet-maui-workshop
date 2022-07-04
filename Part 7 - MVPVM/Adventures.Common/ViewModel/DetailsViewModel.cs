#pragma warning disable CA1416

namespace Adventures.Common.ViewModel;

[QueryProperty(nameof(ListItem), "ListItem")]
public partial class DetailsViewModel : BaseViewModel, IDetailViewModel
{
    [ObservableProperty]
    ListItem listItem;

    [ObservableProperty]
    bool isPopulationVisible;

    public DetailsViewModel() { }
}

