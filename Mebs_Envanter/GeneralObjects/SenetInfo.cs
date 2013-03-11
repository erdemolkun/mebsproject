using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MEBS_Envanter.GeneralObjects;

namespace MEBS_Envanter
{
    public class SenetInfo
    {



        public SenetInfo() {

            //Alan_kisi_rutbe = StaticFields.rutbeler[3];
            //Veren_kisi_isim = "Taner Kaya";
            //Alan_kisi_isim = "Aygün Bayar";     
   

        }

        private int id=-1;

        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        private String alan_kisi_rutbe=StaticFields.rutbeler[1];

        public String Alan_kisi_rutbe
        {
            get { return alan_kisi_rutbe; }
            set { alan_kisi_rutbe = value; }
        }

        private String alan_kisi_isim;

        public String Alan_kisi_isim
        {
            get { return alan_kisi_isim; }
            set { alan_kisi_isim = value; }
        }


        private Birlik alan_kisi_birlik=new Birlik(-1,"");

        public Birlik Alan_kisi_birlik
        {
            get { return alan_kisi_birlik; }
            set { alan_kisi_birlik = value; }
        }

        private Kisim alan_kisi_kisim=new Kisim(-1,"");

        public Kisim Alan_kisi_kisim
        {
            get { return alan_kisi_kisim; }
            set { alan_kisi_kisim = value; }
        }

        private String veren_kisi_isim;

        public String Veren_kisi_isim
        {
            get { return veren_kisi_isim; }
            set { veren_kisi_isim = value; }
        }
    }
}
