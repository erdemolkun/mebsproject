using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MEBS_Envanter.GeneralObjects;
using Mebs_Envanter.GeneralObjects;

namespace MEBS_Envanter
{
    public class SenetInfo
    {
        public SenetInfo() {

           
        }

        private int id=-1;
        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        private String alan_kisi_rutbe="";

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


        private Komutanlik alan_kisi_komutanlik = new Komutanlik(-1, "");
        public Komutanlik Alan_kisi_komutanlik
        {
            get { return alan_kisi_komutanlik; }
            set { alan_kisi_komutanlik = value; }
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
