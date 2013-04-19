using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using Mebs_Envanter.GeneralObjects;

namespace Mebs_Envanter.Converters
{
    public class RutbeComboIndexConverter:IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value != null && (value is String) && StaticFields.rutbeler != null)
            {
                String val = value as String;
                int index = 0;
                foreach (var item in StaticFields.rutbeler)
                {
                    if (item.Equals(val))
                    {
                        return index;
                    }
                    index++;
                }                
                return 0;// throw new NotImplementedException();
            }
            else
            {
                return 0;
            }
            
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
