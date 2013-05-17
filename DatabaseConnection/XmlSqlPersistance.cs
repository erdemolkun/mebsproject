using System;
using System.Collections.Generic;

using System.Text;
using System.Runtime.Serialization;
using System.Security.AccessControl;
using System.IO;
using System.Security.Principal;
using System.Xml.Serialization;

namespace DatabaseConnection
{
    internal class XmlSqlPersistance
    {
        public static String outputFilePath = "";
        public static void SaveToFile(XmlSqlPersistanceObject ob)
        {
            FileInfo f = new FileInfo(outputFilePath);
            SerializationTool<XmlSqlPersistanceObject> serializer = new SerializationTool<XmlSqlPersistanceObject>();
            serializer.SerializeObject(outputFilePath, ob);
        }
    }

    #region Persistent Class
    [Serializable()]
    [XmlRoot("DatabaseSettings")]
    public class XmlSqlPersistanceObject
    {
        private bool isWindowsAuthentication = false;
        public bool IsWindowsAuthentication
        {
            get { return isWindowsAuthentication; }
            set { isWindowsAuthentication = value; }
        }

        private bool useAttachPath = false;
        public bool UseAttachPath
        {
            get { return useAttachPath; }
            set { useAttachPath = value; }
        }

        private string attachPathNode = "samplePath";
        public string AttachPathNode
        {
            get { return attachPathNode; }
            set { attachPathNode = value; }
        }

        private string databaseNode = "sampleNode";
        public string DatabaseNode
        {
            get { return databaseNode; }
            set { databaseNode = value; }
        }

        private string userId = "";
        public string UserId
        {
            get { return userId; }
            set { userId = value; }
        }

        private string password = "";
        public string Password
        {
            get { return password; }
            set { password = value; }
        }

        private string dataSource = @".\SQLEXPRESS";
        public string DataSource
        {
            get { return dataSource; }
            set { dataSource = value; }
        }

        private bool userInstance = true;
        public bool UserInstance
        {
            get { return userInstance; }
            set { userInstance = value; }
        }

        private bool hasInitialCatalog = true;
        public bool HasInitialCatalog
        {
            get { return hasInitialCatalog; }
            set { hasInitialCatalog = value; }
        }

        private byte timeOut = 3;
        public byte TimeOut
        {
            get { return timeOut; }
            set { timeOut = value; }
        }

        private bool persistSecurityInfo = true;
        public bool PersistSecurityInfo
        {
            get { return persistSecurityInfo; }
            set { persistSecurityInfo = value; }
        }
    }
    #endregion
}
