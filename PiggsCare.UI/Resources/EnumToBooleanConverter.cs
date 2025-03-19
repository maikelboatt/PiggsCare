using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace PiggsCare.UI.Resources
{
    public class EnumToBooleanConverter:IValueConverter
    {
        // Converts from the enum value to a bool (true if the enum matches the parameter)
        public object Convert( object value, Type targetType, object parameter, CultureInfo culture )
        {
            if (parameter == null || value == null)
                return DependencyProperty.UnsetValue;

            string parameterString = parameter.ToString();
            if (!Enum.IsDefined(value.GetType(), value))
                return DependencyProperty.UnsetValue;

            object parameterValue = Enum.Parse(value.GetType(), parameterString);
            return parameterValue.Equals(value);
        }

        // Converts back from a bool to the enum value if true, otherwise does nothing
        public object ConvertBack( object value, Type targetType, object parameter, CultureInfo culture )
        {
            if (parameter == null)
                return DependencyProperty.UnsetValue;

            string parameterString = parameter.ToString();
            try
            {
                // Only update the enum if the RadioButton is checked.
                return (bool)value ? Enum.Parse(targetType, parameterString) : Binding.DoNothing;
            }
            catch
            {
                return DependencyProperty.UnsetValue;
            }
        }
    }
}
