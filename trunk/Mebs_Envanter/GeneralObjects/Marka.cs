using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mebs_Envanter.Repositories;
using Mebs_Envanter.Base;

namespace Mebs_Envanter
{
    public class Marka : MebsBaseDBObject
    {
        public override string ToString()
        {
            if (Id > 0)
            {
                return MarkaName.ToUpper().ToString();// + " ID : " + MarkaID; ;
            }
            else {
                return MarkaName;
            }
        }
        public Marka(int marka_id, String markaName) {

            this.MarkaName = markaName;
            this.Id = marka_id;
           
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
        public override int Id
        {
            get { return _markaID; }
            set { 
                _markaID = value;
                if (MarkaRepository.INSTANCE != null)
                {
                    foreach (Marka item in MarkaRepository.INSTANCE.Collection)
                    {
                        if (value == item.Id)
                        {
                            if (value < 0) break;
                            MarkaName = item.MarkaName;
                            break;
                        }
                    }
                }
                else if (value > 0) { 
                
                
                }
                OnPropertyChanged("Id");
            }
        }

    }
}
