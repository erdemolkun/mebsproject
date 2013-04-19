using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace Mebs_Envanter.Converters
{
    public class ListBoxElementVisibilityConverter : System.Windows.Data.IValueConverter
    {
        public virtual object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null || !(value is int)) return Visibility.Collapsed;
            else if (value is int)
            {
                int a = (int)value;
                if (a == 0) return Visibility.Visible;
                //ListBox l = value as ListBox;
                //if (l.IsEnabled && l.Visibility == Visibility.Visible && (l.Items.Count == 0)) {

                //    return Visibility.Visible;
                //}
            }
            return Visibility.Collapsed;
        }
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            //return (value != null && value is Visibility) ? ((Visibility)value == Visibility.Visible) ? true : false : false;
            throw new NotImplementedException();
        }
    }
}
