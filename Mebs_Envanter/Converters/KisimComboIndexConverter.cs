using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using MEBS_Envanter.GeneralObjects;
using MEBS_Envanter.Repositories;

namespace MEBS_Envanter.Converters
{
    public class KisimComboIndexConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value != null && (value is Kisim) && KisimRepository.INSTANCE != null)
            {
                Kisim infoKisim = value as Kisim;
                int index = -1;
                foreach (var item in KisimRepository.INSTANCE.Kisimlar)
                {
                    index++;
                    if (infoKisim.Kisim_id == item.Kisim_id) { return index; }
                    
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
