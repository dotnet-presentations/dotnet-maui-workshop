namespace Adventures.Common.Controls
{
    public class ContentPageBase : ContentPage, IMvpView
	{
		protected FlexLayout _flexLayout;

        public ContentPageBase() { }

		public ContentPageBase(IMvpPresenter presenter)
		{
			presenter.Initialize(this);
			presenter.IsInitialized = true;

			// Set view's view model
			BindingContext = presenter.ViewModel;
		}

		protected void OnInitializeComponent(FlexLayout flexLayout)
        {
            var listViewModel = BindingContext as IListViewModel;
            foreach (Button item in listViewModel.ButtonItems)
                flexLayout.Add(item);
        }

        protected void TapGestureRecognizer_Tapped(System.Object sender, System.EventArgs e)
        {
            var key = "UNDEFINED";
            var label = sender as Label;
            if (label != null)
                key = label.Text;

            var listViewModel = BindingContext as IListViewModel;
            listViewModel.Presenter.InvokeCommand(key);
        }
    }
}

