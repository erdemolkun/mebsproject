using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using MEBS_Envanter.DB;

namespace MEBS_Envanter
{
    public class MarkaRepository
    {

        public static MarkaRepository INSTANCE = null;

        public MarkaRepository() { ClearMarkalar(); }

        private ObservableCollection<Marka> markalar = new ObservableCollection<Marka>();
        public ObservableCollection<Marka> Markalar
        {
            get { return markalar; }
        }

        private void ClearMarkalar() {

            Markalar.Clear();
            Markalar.Add(new Marka(-1, ""));        
        }

        public void FillMarkalar()
        {
            SqlConnection cnn = GlobalDataAccess.Get_Fresh_SQL_Connection();
            string sqlText = "SELECT * FROM tbl_marka";
            SqlCommand cmd = new SqlCommand(sqlText, cnn);

            bool res = GlobalDataAccess.Open_SQL_Connection(cnn);

            if (res)
            {

                ClearMarkalar();
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
