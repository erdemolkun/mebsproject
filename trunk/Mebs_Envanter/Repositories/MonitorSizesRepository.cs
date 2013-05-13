using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using Mebs_Envanter.DB;
using Mebs_Envanter.GeneralObjects;
using System.Data.Common;

namespace Mebs_Envanter.Repositories
{
    public class MonitorSizesRepository : BaseRepository<MonitorSize>
    {
        public static MonitorSizesRepository INSTANCE = null;

        private void Clear(bool isForSearch)
        {
            Collection.Clear();
            if (isForSearch)
            {
                Collection.Add(new MonitorSize(MonitorSize.MON_ID_FOR_SEARCH, 0));
            }
            else {
                Collection.Add(new MonitorSize(MonitorSize.MON_ID_FOR_LIST, 0));
            }
        }

        public void Fetch_Sizes(bool isForSearch)
        {
            DbConnection cnn = GlobalDataAccess.Get_Fresh_Connection();
            string sqlText = "SELECT * FROM tbl_monitor_boyutu order by monitor_boyutu ASC";
            DbCommand cmd = DBCommonAccess.GetCommand(sqlText, cnn);
            bool res = DBCommonAccess.Open_DB_Connection(cnn);

            if (res)
            {
                Clear(isForSearch);

                DbDataReader dr = cmd.ExecuteReader();
                float current_length = 0;
                int current_id = -1;
                while (dr.Read())
                {
                    float.TryParse(dr["monitor_boyutu"].ToString(), out current_length);
                    current_id = (int)dr["boyut_id"];
                    Collection.Add(new MonitorSize(current_id, current_length));
                }
                dr.Close();
                cnn.Close();
            }
        }
    }
}
