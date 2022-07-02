#pragma warning disable CA1416

namespace Adventures.Inventory.Commands
{
    public class GotoInventoryCommand : CommandBase
	{
        public GotoInventoryCommand() {
            MatchButtonText = "Go to Inventory";
            MatchDataType = "Inventory";
            SupportedBy = "MonkeyPresenter";
        }

        public override async void OnExecute()
        {
            var args = EventArgs as ButtonEventArgs;

            await Shell.Current.GoToAsync(nameof(InventoryPage), true);
        }
    }
}

