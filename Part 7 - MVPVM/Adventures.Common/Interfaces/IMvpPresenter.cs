namespace Adventures.Common.Interfaces
{
    public interface IMvpPresenter
	{
		Dictionary<string,IMvpView> Views { get; set; }

        IServiceProvider Provider { get; set; }

        List<string> SupportedButtons { get; set; }

        IMvpViewModel ViewModel { get; set; }

		bool IsInitialized { get; set; }

		void InvokeCommand(string commandKey);

        void Initialize(object sender = null, EventArgs e = null);

		Task ButtonClickHandler(object sender = null, EventArgs e = null);
	}
}

