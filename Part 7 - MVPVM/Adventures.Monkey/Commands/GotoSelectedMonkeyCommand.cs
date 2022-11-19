#pragma warning disable CA1416

namespace Adventures.Monkey.Commands
{
    public class GotoSelectedMonkeyCommand : CommandBase
	{
        public GotoSelectedMonkeyCommand() {
            // We're associating with a DataType, not Key
            DataType = nameof(ListItem);
        }

        /// <summary>
        /// Invoked by PresenterBase.ButtonClickHandler
        /// </summary>
        public override async void OnExecute()
        {
            var args = EventArgs as ButtonEventArgs;
            var listItem = args.GetSender<ListItem>();

            if (listItem == null)
                return;

            var pageInfo = new Dictionary<string, object> {
                { nameof(ListItem), listItem }
            };
            await Shell.Current.GoToAsync(nameof(DetailsPage), true, pageInfo);
        }
    }
}

