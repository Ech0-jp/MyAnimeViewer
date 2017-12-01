using System;
using System.Globalization;
using System.Windows.Data;

namespace MyAnimeViewer.Utility.Converters
{
    public class NumberDisplayConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int num = System.Convert.ToInt32(value);
            return num == 0 ? "-" : num.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string original = System.Convert.ToString(value);
            return original.Replace("-", "0");
        }
    }
}
