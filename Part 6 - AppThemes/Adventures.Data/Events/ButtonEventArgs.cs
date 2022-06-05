using System;
using Adventures.Data.Interfaces;

namespace Adventures.Data.Events
{
	public class ButtonEventArgs : EventArgs
	{
		public IPresenter Presenter { get;set; }
		public IMvpViewModel ViewModel;
		public object Sender { get; set; }
		public string Key { get; set; }

		public T GetSender<T>()
        {
			return (T)Sender;
        }
		public ButtonEventArgs()
		{
		}
	}
}

