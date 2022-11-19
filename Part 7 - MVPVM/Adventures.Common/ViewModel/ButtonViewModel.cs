#pragma warning disable CA1416

namespace Adventures.Common.ViewModel;

public partial class ButtonViewModel : BaseViewModel
{
    public ButtonViewModel() {

    }

    [ObservableProperty]
    string matchButtonText;

}

