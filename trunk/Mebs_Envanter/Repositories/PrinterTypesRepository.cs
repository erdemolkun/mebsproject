using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using Mebs_Envanter.DB;
using System.Data.Common;

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


        private void Clear(bool isForSearch) {

            Collection.Clear();
            if (isForSearch)
            {
                Collection.Add(new PrinterType(-1, "Hepsi"));
            }
            else {
                Collection.Add(new PrinterType(-1, ""));
            }
        }
        
        public override void Fill(bool isForSearch)
        {
            DbConnection cnn = GlobalDataAccess.Get_Fresh_Connection();
            string sqlText = "SELECT * FROM tbl_printer_types";
            DbCommand cmd = DBCommonAccess.GetCommand(sqlText, cnn);

            bool res = DBCommonAccess.Open_DB_Connection(cnn);

            if (res)
            {

                Clear(isForSearch);
                DbDataReader dr = cmd.ExecuteReader();
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
