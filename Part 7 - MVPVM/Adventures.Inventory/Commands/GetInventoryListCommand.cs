#pragma warning disable CA1416

namespace Adventures.Inventory.Commands
{
    /// <summary>
    /// We'll reuse the GetListCommand providing the button text used in view
    /// </summary>
    public class GetInventoryListCommand : GetListCommand
	{
		public GetInventoryListCommand(IInventoryDataService service)
            : base(service) 
		{
			MatchButtonText = "Get Data";
		}

        public override void OnExecuted()
        {
            var args = this.EventArgs as ButtonEventArgs;
            Console.WriteLine($"Invoked GetInventoryListCommand  " +
                $"Mode=[{args.Presenter.ViewModel.Mode}]");
        }
    }
}

