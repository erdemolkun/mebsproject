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
        static String providerString = "System.Data.SqlClient";
        static DbProviderFactory dbProvider;
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

            //return new SqlCommand(commandText, connection as SqlConnection);
            
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
    }
}
