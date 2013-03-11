using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;

namespace MEBS_Envanter.DB
{
    public class GlobalDataAccess
    {
        private static string connectionString;
        private static string ConnectionString
        {
            get { return (string)connectionString; }
            set { connectionString = value; }
        }


       
        private static SqlConnection _sqlcon;
        //public static SqlConnection Get_Current_SQL_Connection() {

        //    return _sqlcon;                 
        //}
        public static SqlConnection Get_Fresh_SQL_Connection()
        {

            SqlConnection sqlCon = new SqlConnection(connectionString);
            
            return sqlCon;
        }
        public static void Set_Current_SQL_Connection(SqlConnection sqlcon) {
            _sqlcon = sqlcon;
            connectionString = sqlcon.ConnectionString;
        }

        public static bool Open_SQL_Connection(SqlConnection sqlcon) {

            if (sqlcon == null) { return false; }
            if (sqlcon.State == ConnectionState.Open) { return true; }
            try { sqlcon.Open(); return true; }
            catch (Exception) { return false; }        
        }

    }
}
