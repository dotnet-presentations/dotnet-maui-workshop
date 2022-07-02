namespace Adventures.Common.Controls
{
    public class ContentPageBase : ContentPage, IMvpView
	{
        public ContentPageBase() { }

		public ContentPageBase(IMvpPresenter presenter)
		{
			presenter.Initialize(this);
			presenter.IsInitialized = true;

			// Set view's view model
			BindingContext = presenter.ViewModel;
		}



    }
}

