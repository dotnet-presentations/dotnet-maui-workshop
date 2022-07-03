#pragma warning disable CA1416

namespace Adventures.Common.ViewModel;

public partial class BaseViewModel : ObservableObject, IMvpViewModel
{
    /// <summary>
    /// Supported view model buttons. Populated by PresenterBase
    /// ViewModelSet() when the ViewModel property is set
    /// </summary>
    public ObservableCollection<Button> ButtonItems { get; set; }

    /// <summary>
    /// Presenter that will be handling the ViewModel's business
    /// logic, e.g., ButtonClickHandler() invokes the
    /// Presenter.ButtonClickHandler(sender,buttonArgs)
    /// </summary>
    public IMvpPresenter Presenter { get; set; }

    /// <summary>
    /// Unique Id for the ViewModel.  
    /// </summary>
    public Guid Id { get; set; } = Guid.NewGuid();

    /// <summary>
    /// OFFLINE/ONLINE as applicable, set by the MonkeyPresenter
    /// and subsequent subscription in InventoryPresenter
    /// </summary>
    [ObservableProperty]
    string mode;

    public BaseViewModel() { }

    /// <summary>
    /// Invoked by the Command/CommandParameter settings configured
    /// in the App.xaml x:DataType=Button element.  All button
    /// clicks will end up here.
    /// </summary>
    /// <param name="sender"></param>
    /// <returns></returns>
    [ICommand]
    async Task ButtonClickHandler(object sender)
    {
        var button = sender as Button;
        var buttonArgs = new ButtonEventArgs
        {
            Id = Id,
            ViewModel = this,
            Presenter = Presenter,
            Views = Presenter.Views,
            Sender = sender,
            Key = button?.Text ?? sender.GetType().Name
        };
        await Presenter?.ButtonClickHandler(sender, buttonArgs);
    }
}
