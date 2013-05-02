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
    public class KomutanlikRepository : BaseRepository<Komutanlik>
    {

        public static KomutanlikRepository INSTANCE = null;

        private void Clear(bool isForSearch)
        {
            Collection.Clear();
            if (isForSearch)
            {
                Collection.Add(new Komutanlik(-1, "Hepsi"));
            }
            else { Collection.Add(new Komutanlik(-1, "")); }
        }

        public void FillKomutanliklar(bool isForSearch)
        {
            SqlConnection cnn = GlobalDataAccess.Get_Fresh_SQL_Connection();
            string sqlText = "SELECT * FROM tbl_komutanlik";
            SqlCommand cmd = new SqlCommand(sqlText, cnn);

            bool res = GlobalDataAccess.Open_SQL_Connection(cnn);

            if (res)
            {
                Clear(isForSearch);
                SqlDataReader dr = cmd.ExecuteReader();
                string current_komutanlik = null;
                int current_komutanlik_id = -1;
                while (dr.Read())
                {

                    current_komutanlik = dr["komutanlik_adi"].ToString();
                    current_komutanlik_id = (int)dr["komutanlik_id"];

                    Collection.Add(new Komutanlik(current_komutanlik_id, current_komutanlik));
                }
                dr.Close();
                cnn.Close();
            }
        }
    }
}
