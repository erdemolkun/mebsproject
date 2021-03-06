﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using Mebs_Envanter.DB;
using Mebs_Envanter.GeneralObjects;
using System.Data.Common;

namespace Mebs_Envanter.Repositories
{
    public class BirlikRepository : BaseRepository<Birlik>
    {
        public static BirlikRepository INSTANCE = null;

        private void ClearBirlikler(bool isForSearch) { 
            Collection.Clear();
            if (isForSearch)
            {
                Collection.Add(new Birlik(-1, "Hepsi"));
            }
            else{
                Collection.Add(new Birlik(-1, ""));
            }
        }

        public void Fetch_Birlikler(Komutanlik komutanlik, bool isForSearch)
        {
            
            if (komutanlik == null)
            {
                ClearBirlikler(isForSearch);
                return;
            }
            if (komutanlik.Birlik_Repository != null)
                return;

            DbConnection cnn = GlobalDataAccess.Get_Fresh_Connection();
            string sqlText = "SELECT * FROM tbl_birlik where komutanlik_id=@komutanlik_id";            
            DbCommand cmd = DBCommonAccess.GetCommand(sqlText, cnn);
            bool res = DBCommonAccess.Open_DB_Connection(cnn);

            if (res)
            {
                ClearBirlikler(isForSearch);
                DBCommonAccess.AddParameterWithValue(cmd, "@komutanlik_id", komutanlik.Id);                
                DbDataReader dr = cmd.ExecuteReader();
                string current_birlik = null;
                int current_birlik_id = -1;
                while (dr.Read())
                {

                    current_birlik = dr["birlik_adi"].ToString();
                    current_birlik_id = (int)dr["birlik_id"];

                    Collection.Add(new Birlik(current_birlik_id, current_birlik));
                }
                dr.Close();
                cnn.Close();
            }
        }
    }
}
