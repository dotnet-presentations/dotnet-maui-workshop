using Adventures.Common.Entities;

namespace Adventures.Common.Interfaces
{
    public interface IListViewModel : IMvpViewModel
	{
		ObservableCollection<ListItem> ListItems { get; }

        ObservableCollection<CommandItem> CommandItems { get; set; }

        bool IsRefreshing { get; set; }

        bool IsBusy { get; }

        bool IsNotBusy { get; }

        string Title { get; set; }

        // TODO: Collection of buttons
        string ButtonText1 { get; set; }
		string ButtonText2 { get; set; }
        string ButtonText3 { get; set; }
    }
}

