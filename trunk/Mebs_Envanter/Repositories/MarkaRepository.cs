using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using Mebs_Envanter.DB;
using System.Data.Common;

namespace Mebs_Envanter.Repositories
{
    public class MarkaRepository:BaseRepository<Marka>
    {

        private static MarkaRepository instance = null;

        public static MarkaRepository INSTANCE
        {
            get { return MarkaRepository.instance; }
            set {
                MarkaRepository.instance = value; 
            }
        }

        public MarkaRepository() { }

        private void Clear(bool isForSearch) {

            Collection.Clear();
            if (isForSearch)
            {
                Collection.Add(new Marka(-1, "Hepsi"));
            }
            else {
                Collection.Add(new Marka(-1, ""));
            }
        }

        public void Fetch_Markalar(bool isForSearch)
        {
            DbConnection cnn = GlobalDataAccess.Get_Fresh_Connection();
            string sqlText = "SELECT * FROM tbl_marka";
            DbCommand cmd = DBCommonAccess.GetCommand(sqlText, cnn); //new SqlCommand(sqlText, cnn);

            bool res = GlobalDataAccess.Open_DB_Connection(cnn);

            if (res)
            {

                Clear(isForSearch);
                DbDataReader dr = cmd.ExecuteReader();
                string current_marka = null;
                int current_marka_id = -1;
                while (dr.Read())
                {
                   
                    current_marka = dr["marka_ismi"].ToString();
                    current_marka_id = (int)dr["marka_id"];

                    Collection.Add(new Marka(current_marka_id, current_marka));
                }
                dr.Close();
                cnn.Close();
            }
        }
    }
}
