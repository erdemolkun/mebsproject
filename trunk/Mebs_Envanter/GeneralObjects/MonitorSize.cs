using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mebs_Envanter.GeneralObjects
{
    public class MonitorSize
    {
        public static int MON_ID_FOR_SEARCH=-2;
        public static int MON_ID_FOR_LIST=-1;

        public override string ToString()
        {
            if (Id > 0)
            {
                return MonitorLength.ToString();
            }
            else if (Id == MON_ID_FOR_SEARCH)
            {
                return "Hepsi";
            }
            else {
                return "";
            }
        }

        public MonitorSize(int _id, float length) {

            MonitorLength = length;
            Id = _id;
        }

        private int id = -1;

        public int Id
        {
            get { return id; }
            set { id = value; }
        }


        private float monitorLength = 0;
        public float MonitorLength
        {
            get { return monitorLength; }
            set
            {
                if (value > 0)
                {
                    monitorLength = value;
                }
            }
        }

        // Unit will be added..        
    }
}
