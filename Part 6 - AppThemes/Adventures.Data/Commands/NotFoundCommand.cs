#pragma warning disable CA1416

namespace MonkeyFinder.Commands
{
    public class GetMonkeysCommand : IMvpCommand
	{
        public string ButtonText { get; set; } = "Get Monkeys";

        public event EventHandler CanExecuteChanged;

        private IDataService _dataService;

		public GetMonkeysCommand(IDataService dataService)
		{
            _dataService = dataService;
		}

        public bool CanExecute(object parameter) { return true; }

        public async void Execute(object parameter)
        {
            var args = parameter as ButtonEventArgs;
            await GetMonkeysAsync(args.ViewModel as MonkeysViewModel);
        }


        async Task GetMonkeysAsync(MonkeysViewModel vm)
        {
            if (vm.IsBusy)
                return;

            try
            {
                vm.IsBusy = true;

                var serviceResult = await _dataService.GetDataAsync<ServiceResult>();

                if (vm.Monkeys.Count != 0)
                    vm.Monkeys.Clear();

                foreach (var monkey in serviceResult.GetData<List<ListItem>>())
                    vm.Monkeys.Add(monkey);

            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Unable to get monkeys: {ex.Message}");
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

