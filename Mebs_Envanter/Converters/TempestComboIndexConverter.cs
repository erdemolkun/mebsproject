using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using MEBS_Envanter.GeneralObjects;
using Mebs_Envanter.GeneralObjects;

namespace MEBS_Envanter.Converters
{
    public class TempestComboIndexConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value != null && (value is Tempest) && TempestRepository.INSTANCE != null)
            {
                Tempest infoTempest = value as Tempest;
                int index = -1;
                foreach (var item in TempestRepository.INSTANCE.TempestSeviyeler)
                {
                    index++;
                    if (infoTempest.Id == item.Id) { return index; }

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
