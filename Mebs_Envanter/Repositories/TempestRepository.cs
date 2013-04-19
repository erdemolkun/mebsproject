using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mebs_Envanter;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using Mebs_Envanter.DB;
using Mebs_Envanter.GeneralObjects;

namespace Mebs_Envanter.Repositories
{
    internal class TempestRepository : MebsBaseObject
    {
        public static TempestRepository INSTANCE = null;

        private ObservableCollection<Tempest> tempestSeviyeler = new ObservableCollection<Tempest>();
        public ObservableCollection<Tempest> TempestSeviyeler
        {
            get { return tempestSeviyeler; }
        }

        private void ClearSeviyeler(bool isForSearch)
        {
            TempestSeviyeler.Clear();
            if (isForSearch)
            TempestSeviyeler.Add(new Tempest(-1, "Hepsi"));
            else TempestSeviyeler.Add(new Tempest(-1, ""));
        }
        public void FillSeviyeler(bool isForSearch)
        {

            SqlConnection cnn = GlobalDataAccess.Get_Fresh_SQL_Connection();
            string sqlText = "SELECT * FROM tbl_tempest";
            SqlCommand cmd = new SqlCommand(sqlText, cnn);

            bool res = GlobalDataAccess.Open_SQL_Connection(cnn);

            if (res)
            {
                ClearSeviyeler(isForSearch);
                SqlDataReader dr = cmd.ExecuteReader();
                string current_tempest = null;
                int current_tempest_id = -1;
                while (dr.Read())
                {

                    current_tempest = dr["tempest_seviyesi"].ToString();
                    current_tempest_id = (int)dr["tempest_id"];
                    TempestSeviyeler.Add(new Tempest(current_tempest_id, current_tempest));
                }
                dr.Close();
                cnn.Close();
            }
        }
    }
}
