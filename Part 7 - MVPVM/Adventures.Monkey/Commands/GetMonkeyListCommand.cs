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
		}

        public override void OnExecute()
        {
            var args = EventArgs as ButtonEventArgs;
            if (args.ViewModel == null) // Change of network won't have VM
                args.ViewModel = args.Presenter.ViewModel;

            base.OnExecute();
            Console.WriteLine($"Invoked GetMonkeyListCommand  Mode=[{((ButtonEventArgs)this.EventArgs).Presenter.ViewModel.Mode}]");
        }
    }
}

