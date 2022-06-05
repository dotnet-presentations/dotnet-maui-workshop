using System;
namespace Adventures.Common.Interfaces
{
	public interface IMvpViewModel
	{
		IPresenter Presenter { get; set; } 
	}
}

