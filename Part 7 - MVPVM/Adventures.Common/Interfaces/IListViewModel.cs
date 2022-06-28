using Adventures.Common.Entities;

namespace Adventures.Common.Interfaces
{
    public interface IListViewModel : IMvpViewModel
	{
		ObservableCollection<ListItem> ListItems { get; }

        ObservableCollection<CommandItem> CommandItems { get; set; }

        bool IsRefreshing { get; set; }

		string Mode { get; set; }

		string Title { get; set; }

		bool IsBusy { get; set; }

		bool IsNotBusy { get; }

		// TODO: Collection of buttons
		string GetDataButtonText { get; set; }
		string GetDataButton2Text { get; set; }
        string GetDataButton3Text { get; set; }
    }
}

