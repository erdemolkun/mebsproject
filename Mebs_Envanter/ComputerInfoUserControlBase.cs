using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Input;

namespace Mebs_Envanter
{
    public class ComputerInfoUserControlBase:UserControl
    {

        public virtual void SetFocus(int tabIndex) { }
        public virtual void KeyEventResponder(KeyEventArgs e) { }
        public virtual void SetDataContext(object context) { }
        public virtual void Init() { }
        public virtual void Assign_ComputerInfo_By_GUI(ComputerInfo current_Computer, ComputerInfo toUpdateComputer, bool isEdit) { }

    }
}
