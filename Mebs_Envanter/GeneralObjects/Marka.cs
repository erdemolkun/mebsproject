using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MEBS_Envanter.Repositories;

namespace MEBS_Envanter
{
    public class Marka : MebsBaseObject
    {
        public override string ToString()
        {
            if (MarkaID > 0)
            {
                return MarkaName.ToUpper().ToString();// + " ID : " + MarkaID; ;
            }
            else {
                return MarkaName;
            }
        }
        public Marka(int marka_id, String markaName) {

            this.MarkaName = markaName;
            this.MarkaID = marka_id;
           
        }

        private String _markaName;

        public String MarkaName
        {
            get { return _markaName; }
            set { _markaName = value; OnPropertyChanged("MarkaName"); }
        }


        private int _markaID = -1;
        /// <summary>
        /// Markanın db'deki id'si
        /// </summary>
        public int MarkaID
        {
            get { return _markaID; }
            set { 
                _markaID = value;
                if (MarkaRepository.INSTANCE != null)
                {
                    foreach (Marka item in MarkaRepository.INSTANCE.Markalar)
                    {
                        if (value == item.MarkaID)
                        {
                            if (value < 0) break;
                            MarkaName = item.MarkaName;
                            break;
                        }
                    }
                }
                else if (value > 0) { 
                
                
                }
                OnPropertyChanged("MarkaID"); }
        }

    }
}
