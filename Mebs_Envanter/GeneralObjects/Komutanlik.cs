using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mebs_Envanter;
using Mebs_Envanter.Repositories;

namespace Mebs_Envanter.GeneralObjects
{
    public class Komutanlik : MebsBaseObject
    {
        public BirlikRepository Birlik_Repository = null;

        public override string ToString()
        {            
            return Komutanlik_ismi;
        }
        public Komutanlik(int komutanlik_id, String komutanlik_ismi)
        {
            Komutanlik_ismi = komutanlik_ismi;
            Komutanlik_id = komutanlik_id;
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
            set
            {
                _komutanlik_id = value;
                if (KomutanlikRepository.INSTANCE != null)
                {
                    foreach (Komutanlik item in KomutanlikRepository.INSTANCE.Komutanliklar)
                    {
                        if (value == item.Komutanlik_id)
                        {
                            if (value < 0) break;
                            Komutanlik_ismi = item.Komutanlik_ismi;
                            break;
                        }
                    }
                }
                OnPropertyChanged("Komutanlik_id");
            }
        }
    }
}
