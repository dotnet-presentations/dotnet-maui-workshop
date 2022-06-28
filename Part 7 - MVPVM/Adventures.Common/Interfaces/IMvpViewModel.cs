using System;
namespace Adventures.Common.Interfaces
{
	public interface IMvpViewModel
	{
        Guid Id { get; set; }
		string Mode { get; set; }
        IMvpPresenter Presenter { get; set; } 
	}
}

