using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using MEBS_Envanter.DB;
using MEBS_Envanter.GeneralObjects;

namespace MEBS_Envanter.Repositories
{
    public class KisimRepository:MebsBaseObject
    {

        public static KisimRepository INSTANCE = null;

        private ObservableCollection<Kisim> kisimlar = new ObservableCollection<Kisim>();
        public ObservableCollection<Kisim> Kisimlar
        {
            get { return kisimlar; }
        }

        private void ClearKisimlar() {

            Kisimlar.Clear();
            Kisimlar.Add(new Kisim(-1, ""));
        }

        public void FillKisimlar(Birlik birlik)
        {
            if (birlik == null) return ;
            if (birlik.Kisim_Repository != null)
                return ;
            SqlConnection cnn = GlobalDataAccess.Get_Fresh_SQL_Connection();
            string sqlText = "SELECT * FROM tbl_kisim where birlik_id=@birlik_id";
            SqlCommand cmd = new SqlCommand(sqlText, cnn);

            bool res = GlobalDataAccess.Open_SQL_Connection(cnn);

            if (res)
            {
                ClearKisimlar();
                cmd.Parameters.AddWithValue("@birlik_id", birlik.Birlik_id);
                
                SqlDataReader dr = cmd.ExecuteReader();
                string current_kisim = null;
                int current_kisim_id = -1;
                while (dr.Read())
                {

                    current_kisim = dr["kisim_adi"].ToString();
                    current_kisim_id = (int)dr["kisim_id"];

                    Kisimlar.Add(new Kisim(current_kisim_id, current_kisim));
                }
                dr.Close();
                cnn.Close();
            }
            
        }
    }
}
