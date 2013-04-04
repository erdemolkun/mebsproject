using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using MEBS_Envanter.DB;
using Mebs_Envanter.GeneralObjects;

namespace MEBS_Envanter.Repositories
{
    public class KomutanlikRepository : MebsBaseObject
    {

        public static KomutanlikRepository INSTANCE = null;

        private ObservableCollection<Komutanlik> komutanliklar = new ObservableCollection<Komutanlik>();
        public ObservableCollection<Komutanlik> Komutanliklar
        {
            get { return komutanliklar; }
        }


        private void ClearBirlikler() {
            Komutanliklar.Clear();
            Komutanliklar.Add(new Komutanlik(-1, "Hepsi"));
        }

        public void FillKomutanliklar()
        {
            SqlConnection cnn = GlobalDataAccess.Get_Fresh_SQL_Connection();
            string sqlText = "SELECT * FROM tbl_komutanlik";
            SqlCommand cmd = new SqlCommand(sqlText, cnn);

            bool res = GlobalDataAccess.Open_SQL_Connection(cnn);

            if (res)
            {
                ClearBirlikler();
                SqlDataReader dr = cmd.ExecuteReader();
                string current_komutanlik = null;
                int current_komutanlik_id = -1;
                while (dr.Read())
                {

                    current_komutanlik = dr["komutanlik_adi"].ToString();
                    current_komutanlik_id = (int)dr["komutanlik_id"];

                    komutanliklar.Add(new Komutanlik(current_komutanlik_id, current_komutanlik));
                }
                dr.Close();
                cnn.Close();
            }
        }
    }
}
