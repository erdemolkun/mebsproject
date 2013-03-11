using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MEBS_Envanter.GeneralObjects
{
    public class Kisim : MebsBaseObject
    {
        public Kisim(int kisim_id, String kisim_adi)
        {

            Kisim_adi = kisim_adi;
            Kisim_id = kisim_id;
        }

        public override string ToString()
        {
            return Kisim_adi;
        }

        private String _kisim_adi;

        public String Kisim_adi
        {
            get { return _kisim_adi; }
            set { _kisim_adi = value; }
        }


        private int _kisim_id;

        public int Kisim_id
        {
            get { return _kisim_id; }
            set { _kisim_id = value; }
        }

    }
}
