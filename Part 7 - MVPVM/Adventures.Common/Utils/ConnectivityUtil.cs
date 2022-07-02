namespace Adventures.Common.Utils
{
    public class ConnectivityUtil
    {
        public event EventHandler<ConnectivityEventArgs> ConnectivityChanged;

        public ConnectivityUtil() => Connectivity.ConnectivityChanged += Connectivity_ConnectivityChanged;

        ~ConnectivityUtil() => Connectivity.ConnectivityChanged -= Connectivity_ConnectivityChanged;

        void Connectivity_ConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
        {
            var args = new ConnectivityEventArgs { IsActive = true, Message="" };

            if (e.NetworkAccess != NetworkAccess.Internet)
            {
                args.Message="Internet access has been lost.";
                args.IsActive = false;
                ConnectivityChanged?.Invoke(this, args);
                return;
            }

            if (e.NetworkAccess == NetworkAccess.ConstrainedInternet)
                args.Message = "Internet access is available but is limited.  ";

            // Build comma delimited list of all active connections
            var connections = string.Join(",", e.ConnectionProfiles);

            if (string.IsNullOrEmpty(connections))
                args.Message = "No connections available";
            else
                args.Message += $"Active connections: {connections}";

            ConnectivityChanged?.Invoke(this, args);
        }
    }
}

