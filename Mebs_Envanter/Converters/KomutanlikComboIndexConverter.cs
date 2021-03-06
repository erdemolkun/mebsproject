﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using Mebs_Envanter.GeneralObjects;
using Mebs_Envanter.Repositories;

namespace Mebs_Envanter.Converters
{
    public class KomutanlikComboIndexConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value != null && (value is Komutanlik) && KomutanlikRepository.INSTANCE != null)
            {
                Komutanlik infoKomutanlik = value as Komutanlik;
                int index = -1;
                foreach (var item in KomutanlikRepository.INSTANCE.Collection)
                {
                    index++;
                    if (infoKomutanlik.Id == item.Id)
                    {

                        infoKomutanlik.Komutanlik_ismi = item.Komutanlik_ismi;
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
