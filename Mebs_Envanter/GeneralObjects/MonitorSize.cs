﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mebs_Envanter.Base;

namespace Mebs_Envanter.GeneralObjects
{
    public class MonitorSize  : MebsBaseDBObject  ,  IComparable<MonitorSize>
    {
        public static int MON_ID_FOR_SEARCH=-2;
        public static int MON_ID_FOR_LIST=-1;

        public override string ToString()
        {
            if (Id > 0)
            {
                if (MonitorLength > 0)
                {
                    return MonitorLength.ToString();
                }
                else { return ""; }
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

        private double monitorLength = 0;
        public double MonitorLength
        {
            get { return monitorLength; }
            set
            {
                if (value > 0)
                {
                    
                    monitorLength = Math.Round(value,1);
                }
            }
        }

        // Unit will be added..        

        public int CompareTo(MonitorSize other)
        {
            if (this.MonitorLength > other.MonitorLength) return 1;
            else return -1;
        }
    }
}
