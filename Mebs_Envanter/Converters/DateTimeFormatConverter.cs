﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using Mebs_Envanter.GeneralObjects;
using Mebs_Envanter.Repositories;

namespace Mebs_Envanter.Converters
{
    public class DateTimeFormatConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            DateTime ? x =value as  DateTime ?;
            if (x.HasValue)
            {
                DateTime val = x.Value;
                
                String str = String.Format("{0:d MMMMMMMM yyyy,dddddd}", val);
                return str;
            }
            else return "";
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
