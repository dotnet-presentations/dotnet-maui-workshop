#pragma warning disable CA1416

namespace Adventures.Common.ViewModel;

public partial class BaseViewModel : ObservableObject, IMvpViewModel
{
    public ObservableCollection<ButtonViewModel> ButtonItems { get; set; }

    public IMvpPresenter Presenter { get; set; }

    public Guid Id { get; set; } = Guid.NewGuid();

    [ObservableProperty]
    string mode;

    public BaseViewModel() { }

    [ICommand]
    async Task ButtonClickHandler(object sender)
    {
        var buttonArgs = new ButtonEventArgs
        {
            ViewModel = this
        };
        await Presenter?.ButtonClickHandler(sender, buttonArgs);
    }
}
