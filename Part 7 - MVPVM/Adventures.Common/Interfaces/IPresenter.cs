using System;
namespace Adventures.Common.Interfaces
{
	public interface IPresenter
	{
		IMvpViewModel ViewModel { get; set; }

		void Initialize(object sender = null, EventArgs e = null);

		Task ButtonClickHandler(object sender = null, EventArgs e = null);
	}
}

