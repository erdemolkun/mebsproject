using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using MEBS_Envanter.DB;
using Mebs_Envanter.GeneralObjects;
using MEBS_Envanter.GeneralObjects;

namespace MEBS_Envanter.Repositories
{
    public class BirlikRepository : MebsBaseObject
    {

        public static BirlikRepository INSTANCE = null;

        private ObservableCollection<Birlik> birlikler = new ObservableCollection<Birlik>();
        public ObservableCollection<Birlik> Birlikler
        {
            get { return birlikler; }
        }


        private void ClearBirlikler(bool isForSearch) { 
            Birlikler.Clear();
            if (isForSearch)
            {
                Birlikler.Add(new Birlik(-1, "Hepsi"));
            }
            else{
                Birlikler.Add(new Birlik(-1, ""));
            }
        }

        public void FillBirlikler(Komutanlik komutanlik, bool isForSearch)
        {
            
            if (komutanlik == null)
            {
                ClearBirlikler(isForSearch);
                return;
            }
            if (komutanlik.Birlik_Repository != null)
                return;

            SqlConnection cnn = GlobalDataAccess.Get_Fresh_SQL_Connection();
            string sqlText = "SELECT * FROM tbl_birlik where komutanlik_id=@komutanlik_id";
            SqlCommand cmd = new SqlCommand(sqlText, cnn);

            bool res = GlobalDataAccess.Open_SQL_Connection(cnn);

            if (res)
            {
                ClearBirlikler(isForSearch);
                cmd.Parameters.AddWithValue("@komutanlik_id", komutanlik.Komutanlik_id);
                SqlDataReader dr = cmd.ExecuteReader();
                string current_birlik = null;
                int current_birlik_id = -1;
                while (dr.Read())
                {

                    current_birlik = dr["birlik_adi"].ToString();
                    current_birlik_id = (int)dr["birlik_id"];

                    birlikler.Add(new Birlik(current_birlik_id, current_birlik));
                }
                dr.Close();
                cnn.Close();
            }
        }
    }
}
