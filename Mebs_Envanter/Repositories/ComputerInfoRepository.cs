using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;

namespace MEBS_Envanter.Repositories
{
    public class ComputerInfoRepository:MebsBaseObject
    {
        private ObservableCollection<ComputerInfo> computers = new ObservableCollection<ComputerInfo>();
        public ObservableCollection<ComputerInfo> Computers
        {
            get { return computers; }
        }
    }
}
