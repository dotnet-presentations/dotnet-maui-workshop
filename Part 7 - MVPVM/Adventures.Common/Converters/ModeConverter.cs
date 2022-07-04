

namespace Adventures.Common.Converters
{
    public class ModeConverter : IValueConverter
	{
		public ModeConverter()
		{
		}

        public object Convert(object value, Type targetType,
            object parameter, CultureInfo culture)
        {
            var isOnline = AppConstants.Online == value.ToString();
            return isOnline.ToString();
        }

        public object ConvertBack(object value, Type targetType,
            object parameter, CultureInfo culture)
        {
            bool isOnline = bool.Parse(value.ToString());

            return isOnline
                ? AppConstants.Online
                : AppConstants.Offline;
        }
    }
}

