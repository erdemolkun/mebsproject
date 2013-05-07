using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mebs_Envanter;
using System.Collections.ObjectModel;
using Mebs_Envanter.DB;
using Mebs_Envanter.GeneralObjects;
using System.Data.Common;

namespace Mebs_Envanter.Repositories
{
    internal class TempestRepository : BaseRepository<Tempest>
    {
        public static TempestRepository INSTANCE = null;

        private void ClearSeviyeler(bool isForSearch)
        {
            Collection.Clear();
            if (isForSearch)
                Collection.Add(new Tempest(-1, "Hepsi"));
            else Collection.Add(new Tempest(-1, ""));
        }
        public void Fetch_Seviyeler(bool isForSearch)
        {
            DbConnection cnn = GlobalDataAccess.Get_Fresh_Connection();
            string commandText = "SELECT * FROM tbl_tempest";
            DbCommand cmd = DBCommonAccess.GetCommand(commandText, cnn); //new SqlCommand(sqlText, cnn);

            bool res = DBCommonAccess.Open_DB_Connection(cnn);

            if (res)
            {
                ClearSeviyeler(isForSearch);
                DbDataReader dr = cmd.ExecuteReader();
                string current_tempest = null;
                int current_tempest_id = -1;
                while (dr.Read())
                {

                    current_tempest = dr["tempest_seviyesi"].ToString();
                    current_tempest_id = (int)dr["tempest_id"];
                    Collection.Add(new Tempest(current_tempest_id, current_tempest));
                }
                dr.Close();
                cnn.Close();
            }
        }
    }
}
