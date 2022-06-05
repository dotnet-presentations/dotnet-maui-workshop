using Adventures.Data.Results;

namespace Adventures.Common.Interfaces
{
    public interface IDataService
	{
		public string Mode { get; set; }

		Task<T> GetDataAsync<T>(object sender = null, EventArgs e = null) where T : ServiceResult;
	}
}

