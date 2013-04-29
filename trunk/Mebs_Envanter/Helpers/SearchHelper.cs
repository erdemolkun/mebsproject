using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using Mebs_Envanter.Base;


namespace Mebs_Envanter.Helpers
{
    internal static class SearchHelper
    {
        public static bool IsActiveElement(FrameworkElement element)
        {

            return element.Visibility == Visibility.Visible && element.IsEnabled;
        }

        public static void AddToListFromCombo(ComboBox combo, SortedList<String, object> list, string keyName)
        {
            if (IsActiveElement(combo) && combo.SelectedItem != null)
            {
                MebsBaseDBObject mbs = combo.SelectedItem as MebsBaseDBObject;
                if (mbs != null)
                {
                    if (mbs.Id > 0)
                    {
                        list.Add(keyName, mbs.Id);
                    }
                }
            }
        }

    }
}
