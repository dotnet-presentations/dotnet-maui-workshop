using System;
using System.Collections.ObjectModel;
using Adventures.Common.Model;

namespace Adventures.Common.Interfaces
{
	public interface IListViewModel : IMvpViewModel
	{
		ObservableCollection<ListItem> ListItems { get; }

		bool IsRefreshing { get; set; }

		string Mode { get; set; }

		string Title { get; set; }

		bool IsBusy { get; set; }

		bool IsNotBusy { get; }

		string GetDataButtonText { get; set; }

		string GetInventoryButtonText { get; set; }
	}
}

