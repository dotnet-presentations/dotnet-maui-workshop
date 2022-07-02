namespace Adventures.Data.Results
{
    public class ServiceResult
	{
		public object Data { get; set; }

		public T GetData<T>(){
			return (T)Data;
		}


		public ServiceResult()
		{
		}
	}
}

