using System;
using System.Windows.Input;

namespace Adventures.Common.Interfaces
{
	public interface IMvpCommand : ICommand
	{
		public string MatchButtonText { get; set; }
		public string MatchDataType { get; set; }
		public string Message { get; set; }
	}
}

