using Adventures.Data.Events;
using Adventures.Data.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace MonkeyFinder.ViewModel;

public partial class BaseViewModel : ObservableObject, IMvpViewModel
{
    [ObservableProperty]
    [AlsoNotifyChangeFor(nameof(IsNotBusy))]
    bool isBusy;

    [ObservableProperty]
    string title;

    public bool IsNotBusy => !IsBusy;

    public IPresenter Presenter { get; set; }

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
