using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MEBS_Envanter;
using System.Data;
using Mebs_Envanter.DB;
using MEBS_Envanter.GeneralObjects;
using Mebs_Envanter.GeneralObjects;

namespace Mebs_Envanter.Hardware
{
    public class YaziciInfo : OEMDevice
    {


        public YaziciInfo() {

            DeviceType = DeviceTypes.PRINTER;
        }


        public void SetGeneralFields(DataRow rowYazici) {

            Yaz_id = (int)rowYazici["yazici_id"];

            int bagli_ag_id = DBValueHelpers.GetInt32(rowYazici["bagli_ag_id"].ToString(), -1);
            NetworkInfo.BagliAg = new BagliAg("", bagli_ag_id);

            NetworkInfo.IpAddress = rowYazici["ip_adresi"].ToString();


            int tempest_id = DBValueHelpers.GetInt32(rowYazici["tempest_id"].ToString(), -1);
            Tempest = new Tempest(tempest_id, "");

            int senet_id = DBValueHelpers.GetInt32(rowYazici["senet_id"].ToString(), -1);
            SenetInfo.Set_SenetInfos(false, -1, senet_id);

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
