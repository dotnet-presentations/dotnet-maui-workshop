using System;
namespace Adventures.Common.Utils
{
	public class Guard
	{
		public Guard()
		{
		}

		public static void IsNotNull(object sender, string message)
        {
			if (sender == null)
				throw new Exception(message);
        }
	}
}

