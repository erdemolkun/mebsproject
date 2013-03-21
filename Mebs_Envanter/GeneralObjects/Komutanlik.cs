using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MEBS_Envanter;

namespace Mebs_Envanter.GeneralObjects
{
    public class Komutanlik : MebsBaseObject
    {
        public override string ToString()
        {
            //return Birlik_ismi+" ID : "+Birlik_id;
            return Komutanlik_ismi;
        }
        public Komutanlik(int komutanlik_id, String komutanlik_ismi)
        {

            Komutanlik_id = komutanlik_id;
            Komutanlik_ismi = komutanlik_ismi;
        }

        private String _komutanlik_ismi;

        public String Komutanlik_ismi
        {
            get { return _komutanlik_ismi; }
            set { _komutanlik_ismi = value; OnPropertyChanged("Komutanlik_ismi"); }
        }

        private int _komutanlik_id = -1;

        public int Komutanlik_id
        {
            get { return _komutanlik_id; }
            set { _komutanlik_id = value; OnPropertyChanged("Komutanlik_id"); }
        }
    }
}
