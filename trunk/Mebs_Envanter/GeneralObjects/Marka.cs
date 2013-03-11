using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MEBS_Envanter
{
    public class Marka : MebsBaseObject
    {
        public override string ToString()
        {
            return MarkaName.ToUpper().ToString();// + " ID : " + MarkaID; ;
        }
        public Marka(int marka_id, String markaName) {

            this.MarkaID = marka_id;
            this.MarkaName = markaName;
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
            set { _markaID = value; }
        }

    }
}
