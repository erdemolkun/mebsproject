using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using MEBS_Envanter.DB;

namespace MEBS_Envanter.GeneralObjects
{
    public class BirlikRepository:MebsBaseObject
    {

        public static BirlikRepository INSTANCE = null;

        private ObservableCollection<Birlik> birlikler = new ObservableCollection<Birlik>();
        public ObservableCollection<Birlik> Birlikler
        {
            get { return birlikler; }
        }

        public void FillBirlikler() {
            SqlConnection cnn = GlobalDataAccess.Get_Fresh_SQL_Connection();
            string sqlText = "SELECT * FROM tbl_birlik";
            SqlCommand cmd = new SqlCommand(sqlText, cnn);

            bool res = GlobalDataAccess.Open_SQL_Connection(cnn);

            if (res)
            {
                //ClearMarkalar();
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
