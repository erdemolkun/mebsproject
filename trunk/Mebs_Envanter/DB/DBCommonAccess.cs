using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Common;

namespace Mebs_Envanter.DB
{
    internal static class DBCommonAccess
    {
        static String providerString = "System.Data.SqlClient";
        static DbProviderFactory dbProvider;

        public static DbProviderFactory GetProvider() {
            return dbProvider;
        }

        public static DbConnection GetConnection(String conString){
            DbConnection con = dbProvider.CreateConnection();
            con.ConnectionString = conString;
            return con;
        }

        static DBCommonAccess(){
            dbProvider = DbProviderFactories.GetFactory(providerString);
        }

        /// <summary>
        /// Adds a parameter to the command.
        /// </summary>
        /// <param name="command">
        /// The command.
        /// </param>
        /// <param name="parameterName">
        /// Name of the parameter.
        /// </param>
        /// <param name="parameterValue">
        /// The parameter value.
        /// </param>
        /// <remarks>
        /// </remarks>
        public static void AddParameterWithValue(DbCommand command, string parameterName, object parameterValue)
        {
            var parameter = command.CreateParameter();
            parameter.ParameterName = parameterName;
            parameter.Value = parameterValue;
            command.Parameters.Add(parameter);
        }
        public static DbCommand GetCommand(String commandText,DbConnection connection) {
                   
            DbCommand cmd = dbProvider.CreateCommand();
            cmd.Connection = connection;
            cmd.CommandText = commandText;

            return cmd;
        }

        public static DbDataAdapter GetAdapter(DbCommand cmd) {
            DbDataAdapter adapter= dbProvider.CreateDataAdapter();
            adapter.SelectCommand = cmd;
            return adapter;            
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
