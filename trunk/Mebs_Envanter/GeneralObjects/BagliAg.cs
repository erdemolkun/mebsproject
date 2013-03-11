using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MEBS_Envanter.GeneralObjects
{
    public class BagliAg : MebsBaseObject
    {
        public override string ToString()
        {
            return Ag_adi;
        }

        public BagliAg(String _bagliag_adi,
            int _bagliag_id)
        {

            Ag_adi = _bagliag_adi;
            Ag_id = _bagliag_id;
        }


        private String ag_adi;

        public String Ag_adi
        {
            get { return ag_adi; }
            set { ag_adi = value; OnPropertyChanged("Ag_adi"); }
        }


        private int ag_id;

        public int Ag_id
        {
            get { return ag_id; }
            set { ag_id = value; }
        }
    }
}
