#pragma warning disable CA1416

namespace MonkeyFinder.Commands
{
    public class GotoInventoryCommand : CommandBase
	{
        public GotoInventoryCommand() {
            MatchDataType = AppConstants.GoToInventoryButtonText;
        }

        public override async void OnExecute()
        {
            var args = EventArgs as ButtonEventArgs;

            await Shell.Current.GoToAsync(nameof(InventoryPage), true);
        }
    }
}

