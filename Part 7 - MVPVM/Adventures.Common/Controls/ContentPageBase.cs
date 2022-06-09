using System;
using Adventures.Common.Interfaces;

namespace Adventures.Common.Controls
{
	public class ContentPageBase : ContentPage, IMvpView
	{
        public ContentPageBase() { }

		public ContentPageBase(IMvpPresenter presenter)
		{
			if (presenter == null) return;
			presenter.Initialize(this);
			BindingContext = presenter.ViewModel;
		}
	}
}

