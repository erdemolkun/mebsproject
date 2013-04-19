using System;
using System.Windows;
using System.Windows.Controls;
using System.Globalization;

namespace Mebs_Envanter.GUIHelpers
{
    public  class TextBoxCaretBehaviour
    {



        public static bool GetSetCaretRightOnFocus(DependencyObject obj)
        {
            return (bool)obj.GetValue(SetCaretRightOnFocusProperty);
        }

        public static void SetSetCaretRightOnFocus(DependencyObject obj, bool value)
        {
            obj.SetValue(SetCaretRightOnFocusProperty, value);
        }

        public static readonly DependencyProperty SetCaretRightOnFocusProperty =
            DependencyProperty.RegisterAttached(
                "SetCaretRightOnFocus",
                typeof(bool),
                typeof(TextBoxCaretBehaviour),
                new FrameworkPropertyMetadata(true, SetCaretRightOnFocusChangedCallback)
                );

        private static void SetCaretRightOnFocusChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            TextBox _this = (d as TextBox);
            _this.GotFocus += new RoutedEventHandler(_this_GotFocus);
            //ValidateTextBox(_this);
        }

        static void _this_GotFocus(object sender, RoutedEventArgs e)
        {
            (sender as TextBox).CaretIndex = (sender as TextBox).Text.Length; 
        }
        
    }
}
