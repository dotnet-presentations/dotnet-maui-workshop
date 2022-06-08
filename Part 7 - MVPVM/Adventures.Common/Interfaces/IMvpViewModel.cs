using System;
namespace Adventures.Common.Interfaces
{
	public interface IMvpViewModel
	{
		IMvpPresenter Presenter { get; set; } 
	}
}

