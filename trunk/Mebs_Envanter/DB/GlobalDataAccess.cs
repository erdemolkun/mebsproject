using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Data.Common;

namespace Mebs_Envanter.DB
{
    public class GlobalDataAccess
    {
        private static string connectionString;
        private static string ConnectionString
        {
            get { return (string)connectionString; }
            set { connectionString = value; }
        }



        private static DbConnection _sqlcon;
        
        public static DbConnection Get_Fresh_Connection()
        {
            if (connectionString == null) return null;
            DbConnection sqlCon = new SqlConnection(connectionString);            
            return sqlCon;
        }
        public static void Set_Current_Db_Connection(DbConnection con)
        {
            _sqlcon = con;
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
