using System;
using System.Windows.Input;

namespace Adventures.Data.Interfaces
{
	public interface IMvpCommand : ICommand
	{
		public string ButtonText { get; set; }
	}
}

