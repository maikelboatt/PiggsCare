using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace PiggsCare.UI.Resources
{
    public class InverseBooleanToVisibilityConverter:IValueConverter
    {
        public object? Convert( object? value, Type targetType, object? parameter, CultureInfo culture )
        {
            return (bool)value! ? Visibility.Collapsed : Visibility.Visible;
        }

        public object? ConvertBack( object? value, Type targetType, object? parameter, CultureInfo culture )
        {
            throw new NotImplementedException();
        }
    }
}
