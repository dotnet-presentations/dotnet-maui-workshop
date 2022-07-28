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
			ButtonText = "Get Data";
		}

        public override void OnExecuted()
        {
            // Note that through EventArgs we have access to Presenter,
            // ViewModel, and Views (not shown below)
            var args = this.EventArgs as ButtonEventArgs;
            Debug.WriteLine($"Invoked GetInventoryListCommand  " +
                $"Mode=[{args.Presenter.ViewModel.Mode}]");
        }
    }
}

