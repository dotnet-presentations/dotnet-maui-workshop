using System;
using Adventures.Common.Constants;
using Adventures.Common.Interfaces;

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
	
	}
}

