using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using Mebs_Envanter.DB;

namespace Mebs_Envanter.Repositories
{
    public class PrinterTypesRepository:BaseRepository<PrinterType>
    {

        private static PrinterTypesRepository instance = null;

        public static PrinterTypesRepository INSTANCE
        {
            get { return PrinterTypesRepository.instance; }
            set {
                PrinterTypesRepository.instance = value; 
            }
        }

        public PrinterTypesRepository() { }

        //private ObservableCollection<PrinterType> printerTypes = new ObservableCollection<PrinterType>();
        //public ObservableCollection<PrinterType> PrinterTypes
        //{
        //    get { return printerTypes; }
        //}

        private void ClearPrinterTypes(bool isForSearch) {

            Collection.Clear();
            if (isForSearch)
            {
                Collection.Add(new PrinterType(-1, "Hepsi"));
            }
            else {
                Collection.Add(new PrinterType(-1, ""));
            }
        }

        public void FillPrinterTypes(bool isForSearch)
        {
            SqlConnection cnn = GlobalDataAccess.Get_Fresh_SQL_Connection();
            string sqlText = "SELECT * FROM tbl_printer_types";
            SqlCommand cmd = new SqlCommand(sqlText, cnn);

            bool res = GlobalDataAccess.Open_SQL_Connection(cnn);

            if (res)
            {

                ClearPrinterTypes(isForSearch);
                SqlDataReader dr = cmd.ExecuteReader();
                string current_pr_type = null;
                int current_id = -1;
                while (dr.Read())
                {

                    current_pr_type = dr["type_str"].ToString();
                    current_id = (int)dr["id"];

                    Collection.Add(new PrinterType(current_id, current_pr_type));
                }
                dr.Close();
                cnn.Close();
            }
        }
    }
}
