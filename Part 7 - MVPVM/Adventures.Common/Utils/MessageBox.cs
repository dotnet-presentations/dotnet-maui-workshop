using System;
namespace Adventures.Common.Utils
{
	public class MessageBox
	{
		public static async Task Show(string title,
			string message, string button)
        {
            await Shell.Current.DisplayAlert(title, message, button);
        }
	}
}

