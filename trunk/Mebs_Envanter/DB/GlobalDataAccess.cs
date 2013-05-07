using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Common;

namespace Mebs_Envanter.DB
{
    public class GlobalDataAccess
    {
        private static string connectionString;
        
        private static DbConnection current_con;        
        public static DbConnection Get_Fresh_Connection()
        {
            if (connectionString == null) return null;
            DbConnection con = DBCommonAccess.GetConnection(connectionString); //new SqlConnection(connectionString);            
            return con;
        }
        public static void Set_Current_Db_Connection(DbConnection con)
        {
            current_con = con;
            connectionString = con.ConnectionString;
        }

        public static bool Open_DB_Connection(DbConnection con)
        {
            if (con == null) { return false; }
            if (con.State == ConnectionState.Open) { return true; }
            try { con.Open(); return true; }
            catch (Exception) { return false; }        
        }
    }
}
