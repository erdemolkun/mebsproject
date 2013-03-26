using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
namespace MEBS_Envanter.Converters
{
    public class NotBoolToVisibilityConverter : System.Windows.Data.IValueConverter
    {
        public virtual object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return (value != null && value is bool) ? ((bool)value) ? Visibility.Collapsed : Visibility.Visible : Visibility.Visible;
        }
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return (value != null && value is Visibility) ? ((Visibility)value == Visibility.Visible) ? false : true : true;
        }
    }
}
