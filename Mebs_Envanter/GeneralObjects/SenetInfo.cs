using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MEBS_Envanter.GeneralObjects;
using Mebs_Envanter.GeneralObjects;
using System.Data.SqlClient;
using System.Data;
using MEBS_Envanter.DB;
using Mebs_Envanter.DB;

namespace MEBS_Envanter
{
    public class SenetInfo:MebsBaseObject
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

        private String alan_kisi_isim="";

        public String Alan_kisi_isim
        {
            get { return alan_kisi_isim; }
            set { 
                if (value == null) alan_kisi_isim = "";
                else alan_kisi_isim = value;

                OnPropertyChanged("Alan_kisi_isim");
            }
        }


        private Komutanlik alan_kisi_komutanlik = new Komutanlik(-1, "");
        public Komutanlik Alan_kisi_komutanlik
        {
            get { return alan_kisi_komutanlik; }
            set { alan_kisi_komutanlik = value; OnPropertyChanged("Alan_kisi_komutanlik"); }
        }

        private Birlik alan_kisi_birlik=new Birlik(-1,"");

        public Birlik Alan_kisi_birlik
        {
            get { return alan_kisi_birlik; }
            set { alan_kisi_birlik = value; OnPropertyChanged("Alan_kisi_birlik"); }
        }

        private Kisim alan_kisi_kisim=new Kisim(-1,"");

        public Kisim Alan_kisi_kisim
        {
            get { return alan_kisi_kisim; }
            set {

                alan_kisi_kisim = value; OnPropertyChanged("Alan_kisi_kisim");
            }
        }

        private String veren_kisi_isim="";

        public String Veren_kisi_isim
        {
            get { return veren_kisi_isim; }
            set {
                if (value == null) veren_kisi_isim = "";
                else  veren_kisi_isim = value;
                OnPropertyChanged("Veren_kisi_isim");
            }
        }

        internal void Set_SenetInfos(bool isInComputer,int bilgisayar_id,int senet_id)
        {
            SqlConnection cnn = GlobalDataAccess.Get_Fresh_SQL_Connection();

            SqlCommand cmd = null;

            if (isInComputer)
            {
                String conString = "Select * From tbl_senet where bilgisayar_id=@bilgisayar_id";
                cmd = new SqlCommand(conString, cnn);
                cmd.Parameters.AddWithValue("@bilgisayar_id", bilgisayar_id);
            }
            else {

                String conString = "Select * From tbl_senet where senet_id=@senet_id";
                cmd = new SqlCommand(conString, cnn);
                cmd.Parameters.AddWithValue("@senet_id", senet_id);
            }
            
            
            SqlDataAdapter adp = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();

            bool res = GlobalDataAccess.Open_SQL_Connection(cnn);
            try
            {
                adp.Fill(dt);
            }
            catch (Exception)
            {
            }
            finally
            {
                cnn.Close();
                cnn.Dispose();
            }
            foreach (DataRow rowParca in dt.Rows)
            {
                String alan_kisi_rutbe = rowParca["alan_kisi_rutbe"].ToString();
                String alan_kisi_isim = rowParca["alan_kisi_isim"].ToString();
                String veren_kisi_isim = rowParca["veren_kisi_isim"].ToString();

                Alan_kisi_isim = alan_kisi_isim;
                Alan_kisi_rutbe = alan_kisi_rutbe;
                Veren_kisi_isim = veren_kisi_isim;


                int alanKisiKomutanlikId = DBValueHelpers.GetInt32(rowParca["alan_kisi_komutanlik_id"], -1);
                int alanKisiBirlikId = DBValueHelpers.GetInt32(rowParca["alan_kisi_birlik_id"], -1);
                int alanKisiKisimId = DBValueHelpers.GetInt32(rowParca["alan_kisi_kisim_id"], -1);

                Alan_kisi_komutanlik = new Komutanlik(alanKisiKomutanlikId, "");
                Alan_kisi_birlik = new Birlik(alanKisiBirlikId, "");
                Alan_kisi_kisim = new Kisim(alanKisiKisimId, "");
                Id = Convert.ToInt32(rowParca["senet_id"]);
            }
        }

    }
}
