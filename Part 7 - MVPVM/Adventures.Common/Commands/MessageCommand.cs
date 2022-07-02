#pragma warning disable CA1416

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
            await MessageBox.Show("SYSTEM MESSAGE", Message, "Cancel");
        }
    }
}

