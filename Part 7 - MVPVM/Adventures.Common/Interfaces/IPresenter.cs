using System;
namespace Adventures.Common.Interfaces
{
	public interface IPresenter
	{
		void Initialize(object sender = null, EventArgs e = null);

		Task ButtonClickHandler(object sender = null, EventArgs e = null);
	}
}

