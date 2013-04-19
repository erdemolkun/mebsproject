using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Windows.Controls;
using Mebs_Envanter.Repositories;

namespace Mebs_Envanter.Converters
{
    public class MarkaComboIndexConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value != null && (value is Marka) && MarkaRepository.INSTANCE != null)
            {
                Marka marka = value as Marka;
                int index = 0;
                foreach (var item in MarkaRepository.INSTANCE.Markalar)
                {
                    if (marka.MarkaID == item.MarkaID) {
                        marka.MarkaName = item.MarkaName;
                        return index; 
                    }
                    index++;
                }
                return 0;
            }
            else
            {
                return 0;
            }
            //if (value != null && (value is ComputerInfo) && MarkaRepository.INSTANCE!=null)
            //{
            //    ComputerInfo infComputer = value as ComputerInfo;
            //    int index = 0;
            //    foreach (var item in MarkaRepository.INSTANCE.Markalar)
            //    {
            //        if (infComputer.Marka.MarkaID == item.MarkaID) { return index; }
            //        index++;
            //    }
            //    return 0;
            //}
            //else
            //{
            //    return 0;
            //}
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
