using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using Mebs_Envanter.Hardware;

namespace Mebs_Envanter.Repositories
{
    public class YaziciInfoRepository : MebsBaseObject
    {
        private ObservableCollection<YaziciInfo> yazicilar = new ObservableCollection<YaziciInfo>();
        public ObservableCollection<YaziciInfo> Yazicilar
        {
            get { return yazicilar; }
        }
    }
}
