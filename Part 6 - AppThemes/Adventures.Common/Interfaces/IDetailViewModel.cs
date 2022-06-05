using Adventures.Common.Model;

namespace Adventures.Common.Interfaces
{
    public interface IDetailViewModel : IMvpViewModel
	{
		ListItem ListItem { get; set; }

		bool IsPopulationVisible { get; set; }
	}
}

