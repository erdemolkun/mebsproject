using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using Mebs_Envanter.Hardware;

namespace Mebs_Envanter.Converters
{
    public class DeviceTypeToParcaIsimConverter : System.Windows.Data.IValueConverter
    {
        public virtual object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {

            try
            {

                DeviceTypes type = (DeviceTypes)value;
                return DeviceTypeNameHelper.GET_DEV_NAME(type);

            }
            catch (Exception) { 
            
            }
            return "";

        }
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            //return (value != null && value is Visibility) ? ((Visibility)value == Visibility.Visible) ? true : false : false;
            throw new NotImplementedException();
        }
    }
}
