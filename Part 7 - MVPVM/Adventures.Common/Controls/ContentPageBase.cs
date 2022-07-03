namespace Adventures.Common.Controls
{
    public class ContentPageBase : ContentPage, IMvpView
	{
		protected FlexLayout _flexLayout;

        public ContentPageBase() { }

        /// <summary>
        /// Initializes the presenter and sets the BindingContext
        /// to the appliable presenter's view model
        /// </summary>
        /// <param name="presenter"></param>
		public ContentPageBase(IMvpPresenter presenter)
		{
			presenter.Initialize(this);
			presenter.IsInitialized = true;

			// Set view's view model
			BindingContext = presenter.ViewModel;
		}

        /// <summary>
        /// Populates the flex layout provided with the view models buttons
        /// </summary>
        /// <param name="flexLayout"></param>
		protected void OnInitializeComponent(FlexLayout flexLayout)
        {
            var ViewModel = BindingContext as IListViewModel;
            foreach (Button item in ViewModel.ButtonItems)
                flexLayout.Add(item);
        }

        /// <summary>
        /// Generic gesture recognizer, will invoke a command using the
        /// name within the label as the key
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void TapGestureRecognizer_Tapped(Object sender, EventArgs e)
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

