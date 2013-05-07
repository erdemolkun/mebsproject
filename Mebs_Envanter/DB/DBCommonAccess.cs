using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Data.Common;

namespace Mebs_Envanter.DB
{
    internal static class DBCommonAccess
    {
        public static DbCommand GetCommand(String commandText,IDbConnection connection) {

            return new SqlCommand(commandText, connection as SqlConnection);        
        }
    }
}
