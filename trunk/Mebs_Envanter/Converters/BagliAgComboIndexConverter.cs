using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using Mebs_Envanter.GeneralObjects;
using Mebs_Envanter.Repositories;

namespace Mebs_Envanter.Converters
{
    public class BagliAgComboIndexConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value != null && (value is BagliAg) && BagliAgRepository.INSTANCE != null)
            {
                BagliAg infoBagliAg = value as BagliAg;
                int index = 0;
                foreach (var item in BagliAgRepository.INSTANCE.BagliAglar)
                {
                    if (infoBagliAg.Id == item.Id) { return index; }
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
