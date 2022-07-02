﻿
namespace Adventures.Common.Extensions
{
    public static class ServiceProviderExtension
	{
		public static IMvpCommand GetNamedCommand(this IServiceProvider serviceProvider, string name)
        {
			var commands = serviceProvider.GetServices<IMvpCommand>();
			var command = commands.FirstOrDefault(s => s.MatchButtonText == name	|| s.MatchDataType == name);
			if (command == null)
			{
				command = commands.FirstOrDefault(s => s.MatchButtonText == AppConstants.Message);
				command.Message = $"Could not find ButtonText='{name}' " +
                    $"in IMvpCommands IOC registrations - did you register it?"; // Update it with requested name
			}
			return command;
        }

        public static Dictionary<string,string> GetNamedCommands(this IServiceProvider serviceProvider)
        {
			var keyList = new Dictionary<string, string>();
            var commands = serviceProvider.GetServices<IMvpCommand>();
			foreach(IMvpCommand command in commands)
            {
				var key = command.GetType().Name;
				var value = command.MatchButtonText;
				if (!keyList.ContainsKey(key))
					keyList.Add(key, value);
            }
            return keyList;
        }

    }

}

