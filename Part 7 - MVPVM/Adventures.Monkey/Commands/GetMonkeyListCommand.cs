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
			MatchButtonText = "Get Data";
		}

        public override void OnExecuted()
        {
            var args = this.EventArgs as ButtonEventArgs;
            Console.WriteLine($"Invoked GetMonkeyListCommand  " +
                $"Mode=[{args.Presenter.ViewModel.Mode}]");
        }
    }
}

