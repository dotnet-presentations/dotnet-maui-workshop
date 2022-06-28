using System;
namespace Adventures.Common.Events
{
	public class MessageEventArgs : EventArgs
	{
		public string Message { get; set; }

		public Action<object> Callback { get; set; }
		public Action<object> CallbackArgs { get; set; }

		public MessageEventArgs()
		{
		}
	}
}

