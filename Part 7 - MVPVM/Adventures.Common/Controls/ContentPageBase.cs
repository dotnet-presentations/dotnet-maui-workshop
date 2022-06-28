namespace Adventures.Common.Controls
{
    public class ContentPageBase : ContentPage, IMvpView
	{
        public ContentPageBase() { }

		public ContentPageBase(IMvpPresenter presenter)
		{
			if (presenter == null) return;
			presenter.Initialize(this);
			presenter.IsInitialized = true;
			BindingContext = presenter.ViewModel;
		}
	}
}

