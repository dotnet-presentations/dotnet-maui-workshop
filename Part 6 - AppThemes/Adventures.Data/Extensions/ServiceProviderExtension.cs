using System;
using Adventures.Data.Interfaces;

namespace Adventures.Data.Extensions
{
	public static class ServiceProviderExtension
	{
		public static IMvpCommand GetNamedCommand(this IServiceProvider serviceProvider, string name)
        {
			var commands = serviceProvider.GetServices<IMvpCommand>();
			var command = commands.FirstOrDefault(s => s.ButtonText == name);
			return command;
        }
	
	}
}

