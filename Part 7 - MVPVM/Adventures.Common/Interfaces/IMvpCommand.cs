
namespace Adventures.Common.Interfaces
{
    public interface IMvpCommand : ICommand
	{
		public string Name { get; set; }
		public string ButtonText { get; set; }
		public string DataType { get; set; }
		public string Message { get; set; }
	}
}

