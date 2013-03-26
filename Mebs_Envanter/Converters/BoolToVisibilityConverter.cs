using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
namespace MEBS_Envanter.Converters
{
    public class BoolToVisibilityConverter : System.Windows.Data.IValueConverter
    {
        public virtual object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {            
            return (value != null && value is bool) ? ((bool)value) ? Visibility.Visible : Visibility.Collapsed : Visibility.Collapsed;
        }
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return (value != null && value is Visibility) ? ((Visibility)value == Visibility.Visible) ? true : false : false;
        }
    }
}
