#pragma warning disable CA1416

namespace Adventures.Commands
{
    public class ShowonMapCommand : CommandBase
	{
        private IMap _map;

        public ShowonMapCommand(IMap map) {
            ButtonText = "Show on Map";
            _map = map;
        }

        /// <summary>
        /// Invoked by PresenterBase.ButtonClickHandler
        /// </summary>
        public override async void OnExecute()
        {
            var args = EventArgs as ButtonEventArgs;
            var vm = args.ViewModel as DetailsViewModel;

            try
            {
                await _map.OpenAsync(vm.ListItem.Latitude,
                    vm.ListItem.Longitude,
                    new MapLaunchOptions {
                        Name = vm.ListItem.Name,
                        NavigationMode = NavigationMode.None
                    });
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Unable to launch maps: {ex.Message}");
                await Shell.Current
                    .DisplayAlert("Error, no Maps app!", ex.Message, "OK");
            }
        }
    }
}

