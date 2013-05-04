using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xceed.Wpf.Controls;
using System.Text.RegularExpressions;

namespace Mebs_Envanter.GUIHelpers
{
    public class IPMaskedTextBox : MaskedTextBox
    {
        protected override void OnPreviewTextInput(System.Windows.Input.TextCompositionEventArgs e)
        {            
            String x = e.Text;
            Regex pattern = new Regex(@"^(\d|[0-9]|[.])+$");
            e.Handled = pattern.IsMatch(x) == false;
            base.OnPreviewTextInput(e);
        }      
    }
}
