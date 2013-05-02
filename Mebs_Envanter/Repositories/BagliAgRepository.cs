using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using Mebs_Envanter.DB;
using Mebs_Envanter.GeneralObjects;

namespace Mebs_Envanter.Repositories
{
    public class BagliAgRepository:BaseRepository<BagliAg>
    {
        public static BagliAgRepository INSTANCE = new BagliAgRepository();
        private void ClearAglar(bool isForSearch)
        {
            Collection.Clear();
            if (isForSearch)
            {
                Collection.Add(new BagliAg("Hepsi", -1));
            }
            else
            {
                Collection.Add(new BagliAg("", -1));
            }
        }

        public void Fill_Aglar(bool isForSearch)
        {

            SqlConnection cnn = GlobalDataAccess.Get_Fresh_SQL_Connection();
            string sqlText = "SELECT * FROM tbl_bagli_ag";
            SqlCommand cmd = new SqlCommand(sqlText, cnn);

            bool res = GlobalDataAccess.Open_SQL_Connection(cnn);

            if (res)
            {
                ClearAglar(isForSearch);
                SqlDataReader dr = cmd.ExecuteReader();
                string current_bagliag_adi = null;
                int current_bagliag_id = -1;
                while (dr.Read())
                {

                    current_bagliag_adi = dr["bagli_ag_adi"].ToString();
                    current_bagliag_id = (int)dr["bagli_ag_id"];

                    Collection.Add(new BagliAg(current_bagliag_adi, current_bagliag_id));
                }
                dr.Close();
                cnn.Close();
            }

        }

    }

}
