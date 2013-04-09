using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using MEBS_Envanter.DB;
using MEBS_Envanter.GeneralObjects;
using Mebs_Envanter.GeneralObjects;

namespace MEBS_Envanter.Repositories
{
    public class MonitorSizesRepository : MebsBaseObject
    {

        public static MonitorSizesRepository INSTANCE = null;

        private ObservableCollection<MonitorSize> sizes = new ObservableCollection<MonitorSize>();
        public ObservableCollection<MonitorSize> Sizes
        {
            get { return sizes; }
        }

        private void ClearSizes(bool isForSearch)
        {
            Sizes.Clear();
            if (isForSearch)
            {
                Sizes.Add(new MonitorSize(MonitorSize.MON_ID_FOR_SEARCH, 0));
            }
            else {
                Sizes.Add(new MonitorSize(MonitorSize.MON_ID_FOR_LIST, 0));
            }
        }

        public void FillSizes(bool isForSearch)
        {
            SqlConnection cnn = GlobalDataAccess.Get_Fresh_SQL_Connection();
            string sqlText = "SELECT * FROM tbl_monitor_boyutu order by monitor_boyutu ASC";
            SqlCommand cmd = new SqlCommand(sqlText, cnn);
            bool res = GlobalDataAccess.Open_SQL_Connection(cnn);

            if (res)
            {
                ClearSizes(isForSearch);

                SqlDataReader dr = cmd.ExecuteReader();
                float current_length = 0;
                int current_id = -1;
                while (dr.Read())
                {
                    float.TryParse(dr["monitor_boyutu"].ToString(), out current_length);
                    current_id = (int)dr["boyut_id"];
                    Sizes.Add(new MonitorSize(current_id, current_length));
                }
                dr.Close();
                cnn.Close();
            }
        }
    }
}
