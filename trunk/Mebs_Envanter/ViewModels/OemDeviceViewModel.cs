using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MEBS_Envanter;

namespace Mebs_Envanter
{
    public class OemDeviceViewModel : MebsBaseObject
    {

        public OemDeviceViewModel(OEMDevice _devOem) {

            this.DevOem = _devOem;
        }

        //private int adet = 1;

        //public int Adet
        //{
        //    get { return adet; }
        //    set {
        //        if (value > 0)
        //        {
        //            adet = value;
        //            DevOem.Adet = value;
        //            OnPropertyChanged("Adet");
        //        }
        //    }
        //}

        public String ParcaTipiIsmi
        {
            get
            {
                return DeviceTypeNameHelper.GET_DEV_NAME(DevOem.DeviceType);
            }
        }

        private OEMDevice devOem;

        public OEMDevice DevOem
        {
            get { return devOem; }
            set { devOem = value;
                OnPropertyChanged("DevOem");
                OnPropertyChanged("ParcaTipiIsmi");
            }
        }

    }
}
