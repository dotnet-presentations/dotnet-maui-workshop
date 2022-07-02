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
            SupportedBy = "InventoryPresenter";
		}

        public override void OnExecuted()
        {
            Console.WriteLine($"Invoked GetInventoryListCommand  Mode=[{((ButtonEventArgs)this.EventArgs).Presenter.ViewModel.Mode}]");
        }
    }
}

