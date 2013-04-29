using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Windows.Controls;
using Mebs_Envanter.Repositories;

namespace Mebs_Envanter.Converters
{
    public class PrinterTypeComboIndexConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value != null && (value is PrinterType) && PrinterTypesRepository.INSTANCE != null)
            {
                PrinterType type = value as PrinterType;
                int index = 0;
                foreach (var item in PrinterTypesRepository.INSTANCE.PrinterTypes)
                {
                    if (type.Id == item.Id) {
                        type.TypeName = item.TypeName;
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
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
