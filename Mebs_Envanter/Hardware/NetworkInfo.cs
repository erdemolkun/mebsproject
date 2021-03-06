﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Mebs_Envanter.GeneralObjects;

namespace Mebs_Envanter
{
    public class NetworkInfo : MebsBaseObject
    {
        public void SetMyDBFields(DataRow rowNetwork)
        {

            String macStr = rowNetwork["mac"].ToString();
        }

        public NetworkInfo()
        {
            //ConnectedNetworkName = "Tasnif Dışı";
            //MacAddressString = "00:98:C2:45:66:77";
        }

        private BagliAg bagliAg = new BagliAg("", -1);

        public BagliAg BagliAg
        {
            get { return bagliAg; }
            set { bagliAg = value; OnPropertyChanged("BagliAg"); }
        }


        private String macAddressString;
        /// <summary>
        /// MAC Adresi
        /// </summary>
        public String MacAddressString
        {
            get { return macAddressString; }
            set
            {
                macAddressString = value;
                OnPropertyChanged("MacAddressString");

            }
        }

        private String ipAddress;
        /// <summary>
        /// IP Adresi
        /// </summary>
        public String IpAddress
        {
            get { return ipAddress; }
            set { ipAddress = value; OnPropertyChanged("IpAddress"); }
        }
    }
}
