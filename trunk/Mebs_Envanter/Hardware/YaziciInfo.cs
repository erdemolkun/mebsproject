using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mebs_Envanter;
using System.Data;
using Mebs_Envanter.DB;
using Mebs_Envanter.GeneralObjects;

namespace Mebs_Envanter.Hardware
{
    public class YaziciInfo : IndividualDeviceInfo
    {
        public YaziciInfo()
        {
            DeviceType = DeviceTypes.PRINTER;
        }

        public void SetGeneralFieldsYazici(DataRow rowYazici)
        {
            Id_Dev = (int)rowYazici["yazici_id"];

            int bagli_ag_id = DBValueHelpers.GetInt32(rowYazici["bagli_ag_id"].ToString(), -1);
            NetworkInfo.BagliAg = new BagliAg("", bagli_ag_id);
            NetworkInfo.IpAddress = rowYazici["ip_adresi"].ToString();
            int tip_id = DBValueHelpers.GetInt32(rowYazici["tip_id"], -1);
            YaziciTipi = new PrinterType(tip_id, "");
        }

        private PrinterType yaziciTipi;

        public PrinterType YaziciTipi
        {
            get { return yaziciTipi; }
            set { yaziciTipi = value; OnPropertyChanged("YaziciTipi"); }
        }


        private NetworkInfo networkInfo = new NetworkInfo();
        public NetworkInfo NetworkInfo
        {
            get { return networkInfo; }
            set { networkInfo = value; OnPropertyChanged("NetworkInfo"); }
        }
    }
}
