
namespace Adventures.Common.Interfaces
{
    public interface IMvpCommand : ICommand
	{
		public string Name { get; set; }
		public string MatchButtonText { get; set; }
		public string MatchDataType { get; set; }
		public string Message { get; set; }

		void OnCanExecuteChanged();

	}
}

