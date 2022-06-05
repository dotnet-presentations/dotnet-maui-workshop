#pragma warning disable CA1416


using System.Diagnostics;
using Adventures.Common.Commands;
using Adventures.Common.Events;
using Adventures.ViewModel;

namespace Adventures.Commands
{
    public class ClosestItemCommand : CommandBase
	{
        private IGeolocation _geolocation;

        public ClosestItemCommand(IGeolocation geolocation) {
            MatchButtonText = "Find Closest";
            _geolocation = geolocation;
        }

        public override async void Execute(object parameter)
        {
            var args = parameter as ButtonEventArgs;
            var vm = args.ViewModel as ListViewModel;

            if (vm.IsBusy || vm.ListItems.Count == 0)
                return;

            try
            {
                // Get cached location, else get real location.
                var location = await _geolocation.GetLastKnownLocationAsync();
                if (location == null)
                {
                    location = await _geolocation.GetLocationAsync(new GeolocationRequest
                    {
                        DesiredAccuracy = GeolocationAccuracy.Medium,
                        Timeout = TimeSpan.FromSeconds(30)
                    });
                }

                // Find closest item to us
                var first = vm.ListItems.OrderBy(m => location.CalculateDistance(
                    new Location(m.Latitude, m.Longitude), DistanceUnits.Miles))
                    .FirstOrDefault();

                await Shell.Current.DisplayAlert("", first.Name + " " +
                    first.Location, "OK");

            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Unable to query location: {ex.Message}");
                await Shell.Current.DisplayAlert("Error!", ex.Message, "OK");
            }
        }
    }
}

