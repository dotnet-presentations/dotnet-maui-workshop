#pragma warning disable CA1416

namespace MonkeyFinder.Commands
{
    public class GotoSelectedMonkeyCommand : CommandBase
	{
        public GotoSelectedMonkeyCommand() {
            MatchDataType = nameof(ListItem);
        }

        public override async void OnExecute()
        {
            var args = EventArgs as ButtonEventArgs;
            var listItem = args.GetSender<ListItem>();

            if (listItem == null)
                return;

            var pageInfo = new Dictionary<string, object> { { nameof(ListItem), listItem } };
            await Shell.Current.GoToAsync(nameof(DetailsPage), true, pageInfo);
        }
    }
}

