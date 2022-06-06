#pragma warning disable CA1416

using System.Diagnostics;
using Adventures.Common.Events;
using Adventures.Common.Interfaces;
using Adventures.Common.Model;
using Adventures.Data.Results;
using Adventures.ViewModel;

namespace MonkeyFinder.Commands
{
    public class GetListCommand : Adventures.Common.Commands.CommandBase
	{
        private IDataService _dataService;

		public GetListCommand(IDataService dataService)
		{
            MatchButtonText = "Get List";
            _dataService = dataService;
		}

        public override async void Execute(object parameter)
        {
            var args = parameter as ButtonEventArgs;
            var vm = args.ViewModel as ListViewModel;

            if (vm.IsBusy)
                return;

            try
            {
                vm.IsBusy = true;

                var serviceResult = await _dataService.GetDataAsync<ServiceResult>();

                if (vm.ListItems.Count != 0)
                    vm.ListItems.Clear();

                foreach (var item in serviceResult.GetData<List<ListItem>>())
                    vm.ListItems.Add(item);

            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Unable to {MatchButtonText}: {ex.Message}");
                await Shell.Current.DisplayAlert("Error!", ex.Message, "OK");
            }
            finally
            {
                vm.IsBusy = false;
                vm.IsRefreshing = false;
            }
        }
    }
}

