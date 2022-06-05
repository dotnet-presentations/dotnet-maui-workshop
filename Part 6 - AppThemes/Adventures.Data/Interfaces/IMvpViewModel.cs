using System;
namespace Adventures.Data.Interfaces
{
	public interface IMvpViewModel
	{
		IPresenter Presenter { get; set; } 
	}
}

