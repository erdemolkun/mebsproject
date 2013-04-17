using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MEBS_Envanter.Repositories;
using MEBS_Envanter.DB;

namespace MEBS_Envanter.GeneralObjects
{
    public class Birlik : MebsBaseObject
    {
        public KisimRepository Kisim_Repository = null;

        public override string ToString()
        {
            //return Birlik_ismi+" ID : "+Birlik_id;
            return Birlik_ismi;
        }
        public Birlik(int birlik_id, String birlik_ismi)
        {
            Birlik_ismi = birlik_ismi;
            Birlik_id = birlik_id;
        }

        private String _birlik_ismi;

        public String Birlik_ismi
        {
            get { return _birlik_ismi; }
            set { _birlik_ismi = value; OnPropertyChanged("Birlik_ismi"); }
        }

        private int _birlik_id = -1;

        public int Birlik_id
        {
            get { return _birlik_id; }
            set
            {
                _birlik_id = value;

                OnPropertyChanged("Birlik_id");
            }
        }
    }
}
