using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using Mebs_Envanter.Hardware;

namespace Mebs_Envanter.Repositories
{
    public class IndividualDeviceRepository<T> : MebsBaseObject where T : IndividualDeviceInfo
    {
        private ObservableCollection<T> devices = new ObservableCollection<T>();
        public ObservableCollection<T> Devices
        {
            get { return devices; }
        }
    }
}
