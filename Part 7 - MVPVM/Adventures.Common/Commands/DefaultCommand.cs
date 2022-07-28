#pragma warning disable CA1416

namespace Adventures.Common
{
    public class DefaultCommand : CommandBase
	{
		public DefaultCommand()
		{
            ButtonText = AppConstants.DefaultKey;
		}

        /// <summary>
        /// Invoked by PresenterBase.ButtonClickHandler 
        /// </summary>
        public override async void OnExecute()
        {
            var args = EventArgs as ButtonEventArgs;

            if (args.IsHandledByPresenter)
            {
                await args.Presenter.OnButtonClickHandler(this, args);

                // If this property is still true when returning from
                // the presenter then we're done.  Presenter will set
                // to false to allow the following message box to display
                if (args.IsHandledByPresenter)
                    return;
            }

            await MessageBox.Show("SYSTEM MESSAGE", Message, "Cancel");
        }
    }
}

