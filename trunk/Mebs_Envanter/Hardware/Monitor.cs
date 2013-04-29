using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mebs_Envanter.GeneralObjects;

namespace Mebs_Envanter
{
    public class Monitor : OEMDevice
    {
        public override string ToString()
        {
            String str = "";
            bool hasMonType = false;
            if ((int)MonType > 0)
            {
                hasMonType = true;
                str += "Tipi : " + MonType.ToString();
            }
            if (Marka.Id > 0)
            {
                if (hasMonType)
                {
                    str += ",";
                }
                str += " Marka : " + Marka.MarkaName;
            }
            return str;
        }
        public Monitor()
        {
            DeviceType = DeviceTypes.MONITOR;
        }

        private int mon_id = -1;

        public int Mon_id
        {
            get { return mon_id; }
            set { mon_id = value; OnPropertyChanged("Mon_id"); }
        }

        private String stokNo;
        /// <summary>
        /// Stok Numarası
        /// </summary>
        public String StokNo
        {
            get { return stokNo; }
            set { stokNo = value; OnPropertyChanged("StokNo"); }
        }


        private MonitorTypes monType;
        public MonitorTypes MonType
        {
            get { return monType; }
            set { monType = value; OnPropertyChanged("MonType"); }
        }



        private MonitorSize monSize;
        public MonitorSize MonSize
        {
            get { return monSize; }
            set { monSize = value; OnPropertyChanged("MonSize"); }
        }

    }
}
