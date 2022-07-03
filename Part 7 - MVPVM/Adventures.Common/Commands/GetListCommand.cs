#pragma warning disable CA1416

namespace Adventures.Common.Commands
{
    public class GetListCommand : Adventures.Common.Commands.CommandBase
	{
        private IDataService _dataService;

		public GetListCommand(IDataService dataService)
		{
            ButtonText = "Get List";
            _dataService = dataService;
		}

        /// <summary>
        /// Invoked by PresenterBase.ButtonClickHandler
        /// </summary>
        public override async void OnExecute()
        {
            var args = EventArgs as ButtonEventArgs;

            // Change of network won't have VM
            if (args!=null && args.ViewModel == null) 
                args.ViewModel = args.Presenter.ViewModel;

            var vm = args.ViewModel as ListViewModel;

            if (vm.IsBusy)
                return;

            try
            {
                vm.IsBusy = true;

                var serviceResult = await _dataService
                    .GetDataAsync<ServiceResult>();

                if (vm.ListItems.Count != 0)
                    vm.ListItems.Clear();

                var listItems = serviceResult.GetData<List<ListItem>>();
                foreach (var item in listItems)
                    vm.ListItems.Add(item);

            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Unable to {ButtonText}: {ex.Message}");
                await Shell.Current.DisplayAlert("Error!", ex.Message, "OK");
            }
            finally
            {
                vm.IsBusy = false;
                vm.IsRefreshing = false;
            }

            OnExecuted();
        }

        public virtual void OnExecuted() { }
    }
}

