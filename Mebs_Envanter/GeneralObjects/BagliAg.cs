using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mebs_Envanter.Repositories;
using Mebs_Envanter.Base;

namespace Mebs_Envanter.GeneralObjects
{
    public class BagliAg : MebsBaseDBObject
    {
        public override string ToString()
        {
            return Ag_adi.ToUpper();
        }

        public BagliAg(String _bagliag_adi,
            int _bagliag_id)
        {

            Ag_adi = _bagliag_adi;
            Id = _bagliag_id;
        }


        private String ag_adi;

        public String Ag_adi
        {
            get { return ag_adi; }
            set { ag_adi = value; OnPropertyChanged("Ag_adi"); }
        }


        private int ag_id;

        public override int Id
        {
            get { return ag_id; }
            set
            {
                if (BagliAgRepository.INSTANCE != null)
                {
                    foreach (BagliAg item in BagliAgRepository.INSTANCE.Collection)
                    {
                        if (value == item.Id)
                        {
                            if(value<0)break;
                            Ag_adi = item.Ag_adi;
                            break;
                        }
                    }
                }
                ag_id = value;
            }
        }
    }
}
