#pragma warning disable CA1416

namespace Adventures.Monkey.Commands
{
    /// <summary>
    /// We'll reuse the GetListCommand providing the button text 
    /// </summary>
    public class GetMonkeyListCommand : GetListCommand
	{
		public GetMonkeyListCommand(IMonkeyDataService service) : base(service) 
		{
			ButtonText = "Get Data";
		}

        /// <summary>
        /// Invoked by PresenterBase.ButtonClickHandler
        /// </summary>
        public override void OnExecuted()
        {
            // Note that through EventArgs we have access to Presenter,
            // ViewModel, and Views (not shown below)
            var args = this.EventArgs as ButtonEventArgs;
            Debug.WriteLine($"Invoked GetMonkeyListCommand  " +
                $"Mode=[{args.Presenter.ViewModel.Mode}]");
        }
    }
}

