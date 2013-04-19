using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using Mebs_Envanter.GeneralObjects;
using Mebs_Envanter.Repositories;

namespace Mebs_Envanter.Converters
{
    public class MonitorSizeComboIndexConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value != null && (value is MonitorSize) && MonitorSizesRepository.INSTANCE != null)
            {
                MonitorSize infoSize = value as MonitorSize;
                int index = -1;
                foreach (var item in MonitorSizesRepository.INSTANCE.Sizes)
                {
                    index++;
                    if (infoSize.Id == item.Id) {
                        infoSize.MonitorLength = item.MonitorLength;
                        return index; 
                    }

                }
                return -1;// throw new NotImplementedException();
            }
            else
            {
                return -1;
            }            
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
