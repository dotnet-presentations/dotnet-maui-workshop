#pragma warning disable CA1416

using Adventures.Common.Interfaces;

namespace MonkeyFinder.Commands
{
	/// <summary>
    /// We'll reuse the GetListCommand providing the button text used in view
    /// </summary>
    public class GetMonkeyListCommand : GetListCommand
	{
		public GetMonkeyListCommand(IDataService service) : base(service) 
		{
			MatchButtonText = AppConstants.GetListButtonText;
		}
	}
}

