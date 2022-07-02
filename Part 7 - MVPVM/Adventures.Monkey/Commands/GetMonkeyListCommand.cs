#pragma warning disable CA1416

namespace Adventures.Monkey.Commands
{
    /// <summary>
    /// We'll reuse the GetListCommand providing the button text used in view
    /// </summary>
    public class GetMonkeyListCommand : GetListCommand
	{
		public GetMonkeyListCommand(IMonkeyDataService service) : base(service) 
		{
			MatchButtonText = "Get Monkey Data";
            SupportedBy = "MonkeyPresenter";
		}

        public override void OnExecuted()
        {
            Console.WriteLine($"Invoked GetMonkeyListCommand  Mode=[{((ButtonEventArgs)this.EventArgs).Presenter.ViewModel.Mode}]");
        }
    }
}

