using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MEBS_Envanter
{
    public class Monitor : OEMDevice
    {
        public override string ToString()
        {
            String str = "StokNo : " + StokNo + "";
            if ((int)MonType > 0) {
                str+="Tipi : " + MonType.ToString();
            }
            if (Marka.MarkaID > 0) {
                str += "Marka : " + Marka.MarkaName;
            }
            return str;
            
        }
        public Monitor()
        {

            //Marka = new Marka(-1, "Dell");
            //StokNo = "55";
            //SerialNumber = "21N8SHKSL21";
            //DeviceNo = "210";
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





    }
}
