#pragma warning disable CA1416

namespace Adventures.Commands
{
    public class FindClosestCommand : CommandBase
	{
        private IGeolocation _geolocation;

        public FindClosestCommand(IGeolocation geolocation) {
            MatchButtonText = "Find Closest";
            _geolocation = geolocation;
        }
        public override bool CanExecute(object parameter)
        {
            return base.CanExecute(parameter);
        }

        public override async void OnExecute()
        {
            var args = EventArgs as ButtonEventArgs;
            var vm = args.ViewModel as ListViewModel;

            if (vm.IsBusy || vm.ListItems.Count == 0)
            {
                var view = args.Views.FirstOrDefault().Value as Page;
                await MessageBox.Show("No data found to query", "Get data and try again", "OK");
                return;
            }

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

                await MessageBox.Show("", first.Name + " " + first.Location, "OK");

            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Unable to query location: {ex.Message}");
                await MessageBox.Show("Error!", ex.Message, "OK");
            }
        }
    }
}

