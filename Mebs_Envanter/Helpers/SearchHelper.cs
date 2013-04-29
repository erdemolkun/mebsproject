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

        public static void AddToListFromFrameworkElement(FrameworkElement element, SortedList<String, object> list, string keyName)
        {
            try
            {
                if (!IsActiveElement(element)) return;
                if (element is ComboBox)
                {
                    ComboBox combobox = element as ComboBox;
                    if (IsActiveElement(combobox) && combobox.SelectedItem != null)
                    {
                        MebsBaseDBObject mbs = combobox.SelectedItem as MebsBaseDBObject;
                        if (mbs != null)
                        {
                            if (mbs.Id > 0)
                            {
                                list.Add(keyName, mbs.Id);
                            }
                        }
                    }
                }
                else if (element is TextBox)
                {
                    TextBox txtBox = element as TextBox;
                    String text = txtBox.Text.Trim().ToString();
                    if (!String.IsNullOrEmpty(text))
                    {
                        list.Add(keyName, text);
                    }
                }
            }
            catch (Exception)
            {

            }
        }
    }
}
