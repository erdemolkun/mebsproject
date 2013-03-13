using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using MEBS_Envanter.GeneralObjects;

namespace MEBS_Envanter.Converters
{
    public class BirlikComboIndexConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value != null && (value is Birlik) && BirlikRepository.INSTANCE != null)
            {
                Birlik infoBirlik = value as Birlik;
                int index = -1;
                foreach (var item in BirlikRepository.INSTANCE.Birlikler)
                {
                    index++;
                    if (infoBirlik.Birlik_id == item.Birlik_id) {

                        infoBirlik.Birlik_ismi = item.Birlik_ismi;
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
