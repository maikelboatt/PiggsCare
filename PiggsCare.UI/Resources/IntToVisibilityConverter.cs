using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace PiggsCare.UI.Resources
{
    public class IntToVisibilityConverter:IValueConverter
    {
        public object Convert( object value, Type targetType, object parameter, CultureInfo culture )
        {
            if (value is int intValue && intValue > 0)
                return Visibility.Visible;
            return Visibility.Collapsed;
        }

        public object ConvertBack( object value, Type targetType, object parameter, CultureInfo culture ) => throw new NotImplementedException();
    }
}
