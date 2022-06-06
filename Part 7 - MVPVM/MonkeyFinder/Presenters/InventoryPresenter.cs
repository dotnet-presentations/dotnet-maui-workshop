#pragma warning disable CA1416

using Adventures.Common.Presenters;
using Adventures.Common.Interfaces;

namespace MonkeyFinder.Presenters
{
    public class InventoryPresenter : PresenterBase
	{
        IDataService _dataService;

        // Handled view models
        IListViewModel _listVm;
        IDetailViewModel _detailVm;

        public InventoryPresenter(IDataService dataService, IServiceProvider provider, IListViewModel listVm, IDetailViewModel child)
            : base(provider)
        {
            // Retrieve data service so we can get mode (online or offline)
            _dataService = dataService;

            // Resolve view models so they can be configured
            _listVm = listVm;
            _detailVm = child;
        }

        public override void Initialize(object sender = null, EventArgs e = null)
        {
            // Configure the view models this presenter will handle

            _listVm.GetDataButtonText = AppConstants.GetListButtonText;
            _listVm.GetInventoryButtonText = AppConstants.GetInventoryButtonText;
            _listVm.Title = "Monkey Locator";
            _listVm.Mode = _dataService.Mode;
            _listVm.Presenter = this;

            ViewModel = _listVm; // Used by page

            _detailVm.IsPopulationVisible = false;
            _detailVm.Presenter = this;
        }
    }
}
