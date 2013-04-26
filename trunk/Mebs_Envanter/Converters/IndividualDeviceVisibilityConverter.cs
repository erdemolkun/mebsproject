using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using Mebs_Envanter.Hardware;

namespace Mebs_Envanter.Converters
{
    public class IndividualDeviceVisibilityConverter : System.Windows.Data.IValueConverter
    {
        public virtual object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            int param = 0;
            if (parameter != null)
            {
                int.TryParse(parameter.ToString(), out param);
            }
            if (value == null || !(value is IndividualDevice))
            {

                return Visibility.Collapsed;
            }
            else {

                int result = (param & (value as IndividualDevice).DeviceType);
                if (result > 0) {
                    return Visibility.Visible;
                }
                return Visibility.Collapsed;
            }            
        }
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            //return (value != null && value is Visibility) ? ((Visibility)value == Visibility.Visible) ? true : false : false;
            throw new NotImplementedException();
        }
    }
}
