using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mebs_Envanter.Base;

namespace Mebs_Envanter.GeneralObjects
{
    public class Kisim : MebsBaseDBObject
    {
        public Kisim(int kisim_id, String kisim_adi)
        {

            Kisim_adi = kisim_adi;
            Id = kisim_id;
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

    }
}
