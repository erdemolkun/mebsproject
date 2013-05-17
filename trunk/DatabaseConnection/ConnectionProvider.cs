using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Collections;
using System.Diagnostics;
using System.Threading;
using System.IO;
using Microsoft.Win32;

namespace DatabaseConnection
{
    public delegate void ConnectionResultHandler(SqlConnection result);
    public delegate void DataBaseErrorHandler(string error, int hataTipi);
    public class ConnectionProvider
    {

        public event ConnectionResultHandler connectionResult;
        public event DataBaseErrorHandler ConnectionInformation;

        #region Global Variables

        List<String> ArrDataSourceNames = new List<string>();
        XmlSqlPersistanceObject databaseSettings = null;
        string XMLPath = "";
        string AttachDbFilename = string.Empty;               
        bool UseXML = false;
        public bool isConn = false;
        private SqlConnection con = null;
        bool tryOtherOptions = false;
        bool IsInDebugMode = false;
        #endregion

        public ConnectionProvider(string _DbFilenameXml, string _DbFilename, string _DbFilenameDebug, bool _useXml, bool _isDebug)
        {
            this.UseXML = _useXml;
            this.XMLPath = _DbFilenameXml;
            this.IsInDebugMode = _isDebug;

            if (IsInDebugMode)
            {
                this.AttachDbFilename = _DbFilenameDebug;
            }
            else
            {
                this.AttachDbFilename = _DbFilename;
            }
            XmlSqlPersistance.outputFilePath = this.XMLPath;
            if (!File.Exists(XmlSqlPersistance.outputFilePath))
            {

            }
            else
            {
                SerializationTool<XmlSqlPersistanceObject> serializer = new SerializationTool<XmlSqlPersistanceObject>();
                databaseSettings = serializer.DeSerializeObject(XmlSqlPersistance.outputFilePath);
            }
            if (databaseSettings == null)
            {
                databaseSettings = new XmlSqlPersistanceObject();
                XmlSqlPersistance.SaveToFile(databaseSettings);
            }
            if (databaseSettings.UseAttachPath)
            {

                if (!String.IsNullOrEmpty(databaseSettings.AttachPathNode))
                {
                    if (databaseSettings.AttachPathNode.ToLower() != "dbmodel")
                    {
                        AttachDbFilename = databaseSettings.AttachPathNode;
                    }
                }
            }
            FillDataSources();
        }

        private void FillDataSources()
        {
            ArrDataSourceNames.Clear();
            try
            {
                RegistryKey key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Microsoft SQL Server\Instance Names");
                foreach (string sk in key.GetSubKeyNames())
                {
                    RegistryKey rkey = key.OpenSubKey(sk);
                    foreach (string s in rkey.GetValueNames())
                    {
                        ArrDataSourceNames.Add(".\\" + s);
                    }
                }
            }
            catch (Exception)
            {
                ArrDataSourceNames.Add(".\\SQLEXPRESS");
                ArrDataSourceNames.Add(".");
                ArrDataSourceNames.Add(".\\MSSQLSERVER");
            }

            if (!ArrDataSourceNames.Contains(databaseSettings.DataSource.ToUpper()))
            {
                ArrDataSourceNames.Add(databaseSettings.DataSource);
            }
        }

        public SqlConnection Start(bool isThreaded)
        {
            if (isThreaded)
            {
                Thread th = new Thread(delegate()
                {
                    tryConnect();
                });
                th.IsBackground = true;
                th.Start();
                return null;
            }
            else
            {
                return tryConnect();
            }
        }

        private SqlConnection tryConnect()
        {
            if (databaseSettings.UseAttachPath && !File.Exists(AttachDbFilename))
            {
                if (String.IsNullOrEmpty(AttachDbFilename))
                {
                    OnConnectionInformation("Attach Path Is Unavailable In Xml", SqlErrorType.DBPathError);
                }
                else
                {
                    OnConnectionInformation("Attach Database File Is Not Available : " + AttachDbFilename, SqlErrorType.NoFileExist);
                }
                OnConnectionResult(null);
                return null;
            }
            if (UseXML && !File.Exists(XMLPath))
            {
                if (string.IsNullOrEmpty(XMLPath))
                {
                    OnConnectionInformation("Database Config Path Is Missing", SqlErrorType.XMLPathError);
                }
                else
                {
                    OnConnectionInformation("Database Config File Not Found", SqlErrorType.NoFileExistXml);
                }
                OnConnectionResult(null);
                return null;
            }
            BaglantiDene(false);
            if (isConn)
            {
                OnConnectionResult(con);
                return con;
            }
            else
            {
                OnConnectionResult(null);
                return null;
            }
        }

        private String Get_Current_ConnectionString()
        {
            String conString = "Data Source = " + databaseSettings.DataSource;

            if (String.IsNullOrEmpty(AttachDbFilename) && databaseSettings.UseAttachPath)
            {
                conString += " ; AttachDbFilename = " + AttachDbFilename;
            }
            if (!String.IsNullOrEmpty(databaseSettings.DatabaseNode) && databaseSettings.HasInitialCatalog)
                conString += " ; Initial Catalog = " + databaseSettings.DatabaseNode;

            if (databaseSettings.IsWindowsAuthentication)
            {
                conString += " ; Integrated Security=SSPI";
            }
            if (!String.IsNullOrEmpty(databaseSettings.UserId))
                conString += "; User ID = " + databaseSettings.UserId;

            if (!String.IsNullOrEmpty(databaseSettings.Password))
                conString += " ; Password = " + databaseSettings.Password;

            if (databaseSettings.TimeOut > 0)
                conString += " ; Connect Timeout = " + databaseSettings.TimeOut;


            conString += " ; User Instance=" + databaseSettings.UserInstance;
            conString += " ; Persist Security Info=" + databaseSettings.PersistSecurityInfo;
            return conString;
        }

        private bool testDatabaseConnection()
        {
            con = new SqlConnection(Get_Current_ConnectionString());
            try
            {
                if (con.State == System.Data.ConnectionState.Closed)
                    con.Open();
                isConn = true;
                try
                {
                    if (databaseSettings != null)
                    {
                        if (databaseSettings.UseAttachPath)
                        {
                            if ((AttachDbFilename != "") && databaseSettings.AttachPathNode.ToLower() != "dbmodel".ToLower())
                            {
                                AttachDbFilename = databaseSettings.AttachPathNode;
                            }
                        }
                        else
                        {

                        }
                    }
                }
                catch (Exception)
                {
                    //xml hatası
                }
            }
            catch (Exception ex)
            {
                OnConnectionInformation(Get_Current_ConnectionString() + "\n" + ex.Message, SqlErrorType.SqlExceptionMebs);
                isConn = false;
            }
            return isConn;
        }

        private bool BaglantiDene(bool isDebugMode)
        {
            if (isDebugMode)
                return false;

            if (testDatabaseConnection()) return true;

            #region  Trying Other Options

            if (!tryOtherOptions) return false;
            for (int i = 0; i < ArrDataSourceNames.Count; i++)
            {
                databaseSettings.DataSource = ArrDataSourceNames[i];

                if (databaseSettings.UserInstance == false || databaseSettings.HasInitialCatalog == false)
                {
                    databaseSettings.HasInitialCatalog = true;
                    databaseSettings.UserInstance = true;
                    if (testDatabaseConnection()) return true;
                }
                databaseSettings.HasInitialCatalog = true;
                databaseSettings.UserInstance = true;

                if (databaseSettings.UserInstance == true)
                    databaseSettings.UserInstance = false;
                else
                    databaseSettings.UserInstance = true;

                if (testDatabaseConnection()) return true;


                if (databaseSettings.HasInitialCatalog == true)
                    databaseSettings.HasInitialCatalog = false;
                else
                    databaseSettings.HasInitialCatalog = true;

                if (testDatabaseConnection()) return true;

            }

            #endregion

            isConn = false;
            return false;
        }

        private void OnConnectionResult(SqlConnection result)
        {
            if (connectionResult != null)
            {
                connectionResult(result);
            }
        }

        private void OnConnectionInformation(string error, int hataTipi)
        {
            if (ConnectionInformation != null)
            {
                ConnectionInformation(error, hataTipi);
            }
        }
    }

    internal class SqlErrorType
    {
        public SqlErrorType()
        { }
        public static readonly int SqlExceptionMebs = 1;
        public static readonly int ServiceNotAvailable = 2;
        public static readonly int NoFileExist = 3;
        public static readonly int DBPathError = 4;
        public static readonly int NoFileExistXml = 5;
        public static readonly int XMLPathError = 6;
    }
}
