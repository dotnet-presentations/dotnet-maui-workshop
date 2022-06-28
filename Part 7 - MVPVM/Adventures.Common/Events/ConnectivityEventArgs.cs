using System;
namespace Adventures.Common.Events
{
	public class ConnectivityEventArgs : EventArgs
	{
		public string Message { get; set; }
		public bool IsActive { get; set; }

		public ConnectivityEventArgs()
		{
		}
	}
}

