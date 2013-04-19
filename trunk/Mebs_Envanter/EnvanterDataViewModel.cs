using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mebs_Envanter;

namespace Mebs_Envanter
{
    internal class EnvanterDataViewModel : MebsBaseObject
    {
        private ComputerInfo currentComputerInfo;
        public ComputerInfo CurrentComputerInfo
        {
            get { return currentComputerInfo; }
            set { currentComputerInfo = value; OnPropertyChanged("CurrentComputerInfo"); }
        }
    }
}
