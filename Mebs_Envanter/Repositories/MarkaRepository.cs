using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using MEBS_Envanter.DB;

namespace MEBS_Envanter.Repositories
{
    public class MarkaRepository
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

        private ObservableCollection<Marka> markalar = new ObservableCollection<Marka>();
        public ObservableCollection<Marka> Markalar
        {
            get { return markalar; }
        }

        private void ClearMarkalar(bool isForSearch) {

            Markalar.Clear();
            if (isForSearch)
            {
                Markalar.Add(new Marka(-1, "Hepsi"));
            }
            else {
                Markalar.Add(new Marka(-1, ""));
            }
        }

        public void FillMarkalar(bool isForSearch)
        {
            SqlConnection cnn = GlobalDataAccess.Get_Fresh_SQL_Connection();
            string sqlText = "SELECT * FROM tbl_marka";
            SqlCommand cmd = new SqlCommand(sqlText, cnn);

            bool res = GlobalDataAccess.Open_SQL_Connection(cnn);

            if (res)
            {

                ClearMarkalar(isForSearch);
                SqlDataReader dr = cmd.ExecuteReader();
                string current_marka = null;
                int current_marka_id = -1;
                while (dr.Read())
                {
                   
                    current_marka = dr["marka_ismi"].ToString();
                    current_marka_id = (int)dr["marka_id"];

                    markalar.Add(new Marka(current_marka_id, current_marka));
                }
                dr.Close();
                cnn.Close();
            }
        }
    }
}
