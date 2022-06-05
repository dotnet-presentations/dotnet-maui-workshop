#pragma warning disable CA1416

using System.Diagnostics;
using Adventures.Common.Commands;
using Adventures.Common.Events;
using Adventures.ViewModel;

namespace Adventures.Commands
{
    public class ShowonMapCommand : CommandBase
	{
        private IMap _map;

        public ShowonMapCommand(IMap map) {
            MatchButtonText = "Show on Map";
            _map = map;
        }

        public override async void Execute(object parameter)
        {
            var args = parameter as ButtonEventArgs;
            var vm = args.ViewModel as DetailsViewModel;

            try
            {
                await _map.OpenAsync(vm.ListItem.Latitude, vm.ListItem.Longitude, new MapLaunchOptions
                {
                    Name = vm.ListItem.Name,
                    NavigationMode = NavigationMode.None
                });
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Unable to launch maps: {ex.Message}");
                await Shell.Current.DisplayAlert("Error, no Maps app!", ex.Message, "OK");
            }
        }
    }
}

