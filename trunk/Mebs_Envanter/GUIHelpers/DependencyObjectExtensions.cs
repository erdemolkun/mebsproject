using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;

namespace Mebs_Envanter.GUIHelpers
{
    /// <summary>
    /// Extension methods to the DependencyObject class.
    /// </summary>
    public static class DependencyObjectExtensions
    {
        /// <summary>
        /// Find a child of a specific type in the Visual Tree.
        /// </summary>
        public static T FindVisualChild<T>(this DependencyObject obj) where T : DependencyObject
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(obj, i);
                if (child != null && child is T)
                {
                    return (T)child;
                }
                else
                {
                    T grandChild = child.FindVisualChild<T>();
                    if (grandChild != null)
                    {
                        return grandChild;
                    }
                }
            }

            return null;
        }
    }
}
