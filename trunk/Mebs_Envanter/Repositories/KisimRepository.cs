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
    public class KisimRepository:BaseRepository<Kisim>
    {

        public static KisimRepository INSTANCE = null;
      
        private void Clear() {

            Collection.Clear();
            Collection.Add(new Kisim(-1, ""));
        }

        public void Fetch_Kisimlar(Birlik birlik)
        {
            if (birlik == null) return ;
            if (birlik.Kisim_Repository != null)
                return ;
            DbConnection cnn = GlobalDataAccess.Get_Fresh_Connection(); 
            string sqlText = "SELECT * FROM tbl_kisim where birlik_id=@birlik_id";
            DbCommand cmd = DBCommonAccess.GetCommand(sqlText, cnn);

            bool res = DBCommonAccess.Open_DB_Connection(cnn);

            if (res)
            {
                Clear();
                DBCommonAccess.AddParameterWithValue(cmd,"@birlik_id", birlik.Id);                              
                DbDataReader dr = cmd.ExecuteReader();
                string current_kisim = null;
                int current_kisim_id = -1;
                while (dr.Read())
                {

                    current_kisim = dr["kisim_adi"].ToString();
                    current_kisim_id = (int)dr["kisim_id"];

                    Collection.Add(new Kisim(current_kisim_id, current_kisim));
                }
                dr.Close();
                cnn.Close();
            }
            
        }
    }
}
