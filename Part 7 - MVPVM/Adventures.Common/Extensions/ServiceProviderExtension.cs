
namespace Adventures.Common.Extensions
{
    public static class ServiceProviderExtension
	{
        public static IMvpCommand GetNamedCommand(
            this IServiceProvider serviceProvider, ButtonEventArgs e)
        {
            var key = e.Key;
            var commands = serviceProvider.GetServices<IMvpCommand>();
            IMvpCommand command = null;

            // key could be used in multiple presenters, e.g., "Get Data"
            // so we'll first check for the presenters supported buttons
            // to see if the key exist. If so we'll use it
            foreach(string buttonName in e.Presenter.SupportedButtons)
            {
                command = commands.FirstOrDefault(c => c.Name == buttonName);
                if (command.MatchButtonText == key)
                {
                    return command;
                }
            }

            // If not found in presenter supported buttons then we'll search
            // all available commands for the first match and use it
            command = commands.FirstOrDefault(s => s.MatchButtonText == key
                                                  || s.MatchDataType == key);
            if (command == null)
            {
                command = commands.FirstOrDefault(s =>
                    s.MatchButtonText == AppConstants.Message);

                command.Message = $"Could not find ButtonText='{key}' " +
                    $"in IMvpCommands IOC registrations - did you register it?";
            }

            return command;
        }

        public static Dictionary<string,string> GetNamedCommands(
			this IServiceProvider serviceProvider)
        {
            var keyList = new Dictionary<string, string>();
            try
            {
				var commands = serviceProvider.GetServices<IMvpCommand>();
				foreach (IMvpCommand command in commands)
				{
					var key = command.GetType().Name;
					var value = command.MatchButtonText;
					if (!keyList.ContainsKey(key))
						keyList.Add(key, value);
				}
				return keyList;
			} catch(Exception ex)
            {
				Debug.WriteLine($"ERROR: Could not get IMvpCommand Services" +
                    $" {ex.Message}");

				return keyList;
            }
        }

		public static IServiceCollection AddButtonSupport<TCommand>(
			this IServiceCollection serviceCollection)
        {
			return serviceCollection;
        }
    }
}

