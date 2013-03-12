using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MEBS_Envanter
{
    public class Monitor:OEMDevice
    {

        public Monitor() {

            //Marka = new Marka(-1, "Dell");
            //StokNo = "55";
            //SerialNumber = "21N8SHKSL21";
            //DeviceNo = "210";
            DeviceType = DeviceTypes.MONITOR;
        }

        private int mon_id=-1;

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


        

        



    }
}
