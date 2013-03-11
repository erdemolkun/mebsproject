using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xceed.Wpf.Controls;
using System.Text.RegularExpressions;

namespace MEBS_Envanter.GUIHelpers
{
    public class MacMaskedTextBox : MaskedTextBox
    {

        protected override void OnPreviewTextInput(System.Windows.Input.TextCompositionEventArgs e)
        {
            String x = e.Text;
            Regex pattern = new Regex(@"^(\d|[a-f]|[A-F])+$");
            e.Handled = pattern.IsMatch(x) == false;
            base.OnPreviewTextInput(e);
        }

      
    }
}
