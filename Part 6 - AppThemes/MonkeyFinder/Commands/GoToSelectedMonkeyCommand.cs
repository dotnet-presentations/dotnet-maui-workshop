﻿#pragma warning disable CA1416

using Adventures.Common.Commands;
using Adventures.Common.Events;
using Adventures.Common.Model;
using MonkeyFinder.View;

namespace MonkeyFinder.Commands
{
    public class GotoToSelectedMonkeyCommand : CommandBase
	{

        public GotoToSelectedMonkeyCommand() {
            MatchDataType = nameof(ListItem);
        }

        public override async void Execute(object parameter)
        {
            var args = parameter as ButtonEventArgs;
            var listItem = args.GetSender<ListItem>();

            if (listItem == null)
                return;

            var pageInfo = new Dictionary<string, object> { { nameof(ListItem), listItem } };
            await Shell.Current.GoToAsync(nameof(DetailsPage), true, pageInfo);
        }
    }
}

