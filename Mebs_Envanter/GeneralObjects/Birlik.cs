using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mebs_Envanter.Repositories;
using Mebs_Envanter.DB;
using Mebs_Envanter.Base;

namespace Mebs_Envanter.GeneralObjects
{
    public class Birlik : MebsBaseDBObject
    {
        public KisimRepository Kisim_Repository = null;

        public override string ToString()
        {            
            return Birlik_ismi;
        }
        public Birlik(int birlik_id, String birlik_ismi)
        {
            Birlik_ismi = birlik_ismi;
            Id = birlik_id;
        }

        private String _birlik_ismi;

        public String Birlik_ismi
        {
            get { return _birlik_ismi; }
            set { _birlik_ismi = value; OnPropertyChanged("Birlik_ismi"); }
        }        
    }
}
