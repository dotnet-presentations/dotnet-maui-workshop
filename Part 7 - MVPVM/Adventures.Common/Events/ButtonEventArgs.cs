namespace Adventures.Common.Events
{
    public class ButtonEventArgs : EventArgs
	{
		public IMvpPresenter Presenter { get;set; }

		public Dictionary<string, IMvpView> Views { get; set; }

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

