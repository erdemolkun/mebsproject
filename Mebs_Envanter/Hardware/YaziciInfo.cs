using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MEBS_Envanter;

namespace Mebs_Envanter.Hardware
{
    public class YaziciInfo : OEMDevice
    {


        public YaziciInfo() {

            DeviceType = DeviceTypes.PRINTER;
        }
        
        private int yaz_id = -1;

        public int Yaz_id
        {
            get { return yaz_id; }
            set { yaz_id = value; OnPropertyChanged("Yaz_id"); }
        }


        private String yaziciModeli = "";

        public String YaziciModeli
        {
            get { return yaziciModeli; }
            set { yaziciModeli = value; OnPropertyChanged("YaziciModeli"); }
        }


        private NetworkInfo networkInfo = new NetworkInfo();

        public NetworkInfo NetworkInfo
        {
            get { return networkInfo; }
            set { networkInfo = value; OnPropertyChanged("NetworkInfo"); }
        }

        private SenetInfo senetInfo = new SenetInfo();

        public SenetInfo SenetInfo
        {
            get { return senetInfo; }
            set { senetInfo = value; OnPropertyChanged("SenetInfo"); }
        }

    }
}
