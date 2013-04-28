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
            int senet_id = DBValueHelpers.GetInt32(rowYazici["senet_id"].ToString(), -1);                        
            SenetInfo.Id = senet_id;
            //SenetInfo.Set_SenetInfos(false, -1, senet_id);            
        }



        private NetworkInfo networkInfo = new NetworkInfo();
        public NetworkInfo NetworkInfo
        {
            get { return networkInfo; }
            set { networkInfo = value; OnPropertyChanged("NetworkInfo"); }
        }
    }
}
