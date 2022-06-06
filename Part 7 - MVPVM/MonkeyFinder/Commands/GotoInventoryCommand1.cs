#pragma warning disable CA1416

using Adventures.Common.Commands;
using Adventures.Common.Events;
using MonkeyFinder.View;

namespace MonkeyFinder.Commands
{
    public class GotoInventoryCommand : CommandBase
	{

        public GotoInventoryCommand() {
            MatchDataType = AppConstants.GoToInventoryButtonText;
        }

        public override async void Execute(object parameter)
        {
            var args = parameter as ButtonEventArgs;

            await Shell.Current.GoToAsync(nameof(InventoryPage), true);
        }
    }
}

