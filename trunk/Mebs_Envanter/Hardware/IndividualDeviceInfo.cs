using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mebs_Envanter.Hardware
{
    public class IndividualDeviceInfo : OEMDevice
    {
        public bool isEdit = false;
        public bool PropertiesFetched = false;
        public void Fetch()
        {
            if (!PropertiesFetched)
            {
                PropertiesFetched = true;
            }
        }


        private int id_Dev = -1;

        public int Id_Dev
        {
            get { return id_Dev; }
            set { id_Dev = value; OnPropertyChanged("Id_Dev"); }
        }


        private SenetInfo senetInfo = new SenetInfo();

        public SenetInfo SenetInfo
        {
            get { return senetInfo; }
            set { senetInfo = value; OnPropertyChanged("SenetInfo"); }
        }

    }
}
