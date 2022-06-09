#pragma warning disable CA1416

using Adventures.Common.Commands;
using Adventures.Common.Constants;
using Adventures.Common.Events;

namespace Adventures.Common
{
    public class MessageCommand :CommandBase
	{
		public MessageCommand()
		{
            MatchButtonText = AppConstants.Message;
		}

        public override async void OnExecute()
        {
            var args = EventArgs as ButtonEventArgs;
            await Shell.Current.DisplayAlert("SYSTEM MESSAGE", Message, "Cancel");

        }
    }
}

