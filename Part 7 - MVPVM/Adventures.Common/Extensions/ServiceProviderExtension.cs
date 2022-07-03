
namespace Adventures.Common.Extensions
{
    public static class ServiceProviderExtension
	{
        /// <summary>
        /// Get the command associated with the ButtonEventArgs.Key
        /// Note: If the key is used in multiple commands, e.g.,
        /// "Get Data" the process will first check the applicable
        /// Presenters SupportButtons, and if not found, then all
        /// IMvpCommand registrations.  If the Key is not found in
        /// any commands then the DefaultCommand will be used with
        /// Command.IsHandledByPresenter=true.  
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// <param name="e"></param>
        /// <returns></returns>
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
                if (command.ButtonText == key)
                {
                    return command;
                }
            }

            // If not found in presenter supported buttons then we'll search
            // all available commands for the first match and use it
            command = commands.FirstOrDefault(s => s.ButtonText == key
                                                  || s.DataType == key);

            if (command == null) // DefaultCommand will be used
            {
                // Provide the presenter an opportunity to handle this
                // command.  The DefaultCommand will check for this.
                e.IsHandledByPresenter = true;

                command = commands.FirstOrDefault(s =>
                    s.ButtonText == AppConstants.DefaultKey);

                command.Message = $"Could not find ButtonText='{key}' " +
                    $"in IMvpCommands IOC registrations - did you register it? " +
                    $"\r\n\r\n" +
                    $"You can override OnButtonClickHandler in presenter to " +
                    $"handle this button click OR create a command";
            }

            return command;
        }

        /// <summary>
        /// Retuns all commands in a dictionary using the format:
        /// Key=CommandType and Value=command.ButtonText, e.g.,
        /// Key=FindClosestCommand, Value="Find Closest"
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// <returns></returns>
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
					var value = command.ButtonText;
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

    }
}

