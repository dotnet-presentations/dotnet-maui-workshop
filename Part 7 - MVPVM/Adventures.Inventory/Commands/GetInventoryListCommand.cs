#pragma warning disable CA1416

namespace Adventures.Inventory.Commands
{
    /// <summary>
    /// We'll reuse the GetListCommand providing the button text used in view
    /// </summary>
    public class GetInventoryListCommand : GetListCommand
	{
		public GetInventoryListCommand(IInventoryDataService service) : base(service) 
		{
			MatchButtonText = "Get Inventory Data";
		}

        public override void OnExecute()
        {
            var args = EventArgs as ButtonEventArgs;
            if (args.ViewModel == null) // Change of network won't have VM
                args.ViewModel = args.Presenter.ViewModel;

            base.OnExecute();
            Console.WriteLine($"Invoked GetInventoryListCommand  Mode=[{((ButtonEventArgs)this.EventArgs).Presenter.ViewModel.Mode}]");
        }
    }
}

