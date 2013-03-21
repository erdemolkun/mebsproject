using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.IO;
using DatabaseConnection;
using System.Windows;

namespace MEBS_Envanter.DB
{
    public class DBFunctions
    {
        static DBFunctions()
        {
            prepareStoredProcedures();
        }

        public static SqlConnection proviceConnection()
        {
            String currentDir = Directory.GetCurrentDirectory();
            String dbName = "mebsenvanter.mdf";

            String xmlPath = currentDir + "\\DBSettings.xml";
            String dbPath = currentDir + "\\" + dbName;

            ConnectionProvider connecitonProvider = new ConnectionProvider(xmlPath, dbPath, "", true, false);
            connecitonProvider.connectionResult += new ConnectionResultHandler(connecitonProvider_connectionResult);
            connecitonProvider.ConnectionInformation += new DataBaseErrorHandler(connecitonProvider_ConnectionInformation);
            return connecitonProvider.start(false);
        }

        static void connecitonProvider_ConnectionInformation(string error, int hataTipi) { }
        static void connecitonProvider_connectionResult(SqlConnection result) { }

        static SqlCommand cmBilgisayarEkleSilDuz;
        static SqlCommand cmParcaEkleSilDuz;
        static SqlCommand cmMonitorEkleSilDuz;
        static SqlCommand cmSenetEkleDilDuz;
        private static void prepareStoredProcedures()
        { 
            cmBilgisayarEkleSilDuz = new SqlCommand("p_bilgisayar_ek_sil_duz");
            cmBilgisayarEkleSilDuz.CommandType = CommandType.StoredProcedure;

            cmBilgisayarEkleSilDuz.Parameters.Add(new SqlParameter("@bilgisayar_id", SqlDbType.Int));
            cmBilgisayarEkleSilDuz.Parameters.Add(new SqlParameter("@type", SqlDbType.NVarChar, 5));
            cmBilgisayarEkleSilDuz.Parameters.Add(new SqlParameter("@marka_id", SqlDbType.Int));
            cmBilgisayarEkleSilDuz.Parameters.Add(new SqlParameter("@pc_adi", SqlDbType.NVarChar, 100));
            cmBilgisayarEkleSilDuz.Parameters.Add(new SqlParameter("@bagli_ag_id", SqlDbType.Int));
            cmBilgisayarEkleSilDuz.Parameters.Add(new SqlParameter("@tempest_id", SqlDbType.Int));            
            cmBilgisayarEkleSilDuz.Parameters.Add(new SqlParameter("@mac", SqlDbType.NVarChar, 30));
            cmBilgisayarEkleSilDuz.Parameters.Add(new SqlParameter("@model", SqlDbType.NVarChar, 50));
            cmBilgisayarEkleSilDuz.Parameters.Add(new SqlParameter("@pc_stok_no", SqlDbType.NVarChar, 50));
            cmBilgisayarEkleSilDuz.Parameters.Add(new SqlParameter("@seri_no", SqlDbType.NVarChar, 50));
            cmBilgisayarEkleSilDuz.Parameters.Add(new SqlParameter("@parca_no", SqlDbType.NVarChar, 50));
            cmBilgisayarEkleSilDuz.Parameters.Add(new SqlParameter("@kayit_ekleme_tarihi", SqlDbType.Date));
            cmBilgisayarEkleSilDuz.Parameters.Add(new SqlParameter("@notlar", SqlDbType.NVarChar, 200));
            cmBilgisayarEkleSilDuz.Parameters.Add(new SqlParameter("@temp_bilgisayar_id", SqlDbType.Int));
            cmBilgisayarEkleSilDuz.Parameters["@temp_bilgisayar_id"].Direction = ParameterDirection.Output;


            cmParcaEkleSilDuz = new SqlCommand("p_parca_ek_sil_duz");
            cmParcaEkleSilDuz.CommandType = CommandType.StoredProcedure;
            cmParcaEkleSilDuz.Parameters.Add(new SqlParameter("@type", SqlDbType.NVarChar, 5));
            cmParcaEkleSilDuz.Parameters.Add(new SqlParameter("@parca_id", SqlDbType.Int));
            cmParcaEkleSilDuz.Parameters.Add(new SqlParameter("@bilgisayar_id", SqlDbType.Int));
            cmParcaEkleSilDuz.Parameters.Add(new SqlParameter("@marka_id", SqlDbType.Int));
            cmParcaEkleSilDuz.Parameters.Add(new SqlParameter("@seri_no", SqlDbType.NVarChar, 50));
            cmParcaEkleSilDuz.Parameters.Add(new SqlParameter("@parca_tanimi", SqlDbType.NVarChar, 50));
            cmParcaEkleSilDuz.Parameters.Add(new SqlParameter("@parca_no", SqlDbType.NVarChar, 50));
            cmParcaEkleSilDuz.Parameters.Add(new SqlParameter("@tempest_id", SqlDbType.Int));
            cmParcaEkleSilDuz.Parameters.Add(new SqlParameter("@parca_tipi", SqlDbType.SmallInt));
            cmParcaEkleSilDuz.Parameters.Add(new SqlParameter("@temp_parca_id", SqlDbType.Int));
            
            cmParcaEkleSilDuz.Parameters["@temp_parca_id"].Direction = ParameterDirection.Output;


            cmMonitorEkleSilDuz = new SqlCommand("p_monitor_ek_sil_duz");
            cmMonitorEkleSilDuz.CommandType = CommandType.StoredProcedure;
            cmMonitorEkleSilDuz.Parameters.Add(new SqlParameter("@type", SqlDbType.NVarChar, 5));
            cmMonitorEkleSilDuz.Parameters.Add(new SqlParameter("@monitor_id", SqlDbType.Int));
            cmMonitorEkleSilDuz.Parameters.Add(new SqlParameter("@parca_id", SqlDbType.Int));
            cmMonitorEkleSilDuz.Parameters.Add(new SqlParameter("@monitor_tipi", SqlDbType.SmallInt));
            cmMonitorEkleSilDuz.Parameters.Add(new SqlParameter("@stok_no", SqlDbType.NVarChar, 50));
            cmMonitorEkleSilDuz.Parameters.Add(new SqlParameter("@temp_monitor_id", SqlDbType.Int));
            cmMonitorEkleSilDuz.Parameters["@temp_monitor_id"].Direction = ParameterDirection.Output;



            cmSenetEkleDilDuz = new SqlCommand("p_senet_ek_sil_duz");
            cmSenetEkleDilDuz.CommandType = CommandType.StoredProcedure;
            cmSenetEkleDilDuz.Parameters.Add(new SqlParameter("@temp_senet_id", SqlDbType.Int));
            cmSenetEkleDilDuz.Parameters["@temp_senet_id"].Direction = ParameterDirection.Output;
            cmSenetEkleDilDuz.Parameters.Add(new SqlParameter("@type", SqlDbType.NVarChar, 5));
            cmSenetEkleDilDuz.Parameters.Add(new SqlParameter("@bilgisayar_id", SqlDbType.Int));
            cmSenetEkleDilDuz.Parameters.Add(new SqlParameter("@senet_id", SqlDbType.Int));
            cmSenetEkleDilDuz.Parameters.Add(new SqlParameter("@alan_kisi_rutbe", SqlDbType.NVarChar, 50));
            cmSenetEkleDilDuz.Parameters.Add(new SqlParameter("@alan_kisi_isim", SqlDbType.NVarChar, 50));
            cmSenetEkleDilDuz.Parameters.Add(new SqlParameter("@veren_kisi_isim", SqlDbType.NVarChar, 50));
            cmSenetEkleDilDuz.Parameters.Add(new SqlParameter("@alan_kisi_birilk_id", SqlDbType.Int));
            cmSenetEkleDilDuz.Parameters.Add(new SqlParameter("@alan_kisi_komutanlik_id", SqlDbType.Int));
            cmSenetEkleDilDuz.Parameters.Add(new SqlParameter("@alan_kisi_kisim_id", SqlDbType.Int));
        }


        public static bool DeletePC(ComputerInfo infoComputer)
        {
            try
            {
                cmBilgisayarEkleSilDuz.Connection = GlobalDataAccess.Get_Fresh_SQL_Connection();
                bool res = GlobalDataAccess.Open_SQL_Connection(cmBilgisayarEkleSilDuz.Connection);
                if (res)
                { cmBilgisayarEkleSilDuz.Parameters["@type"].Value = "S"; }

                cmBilgisayarEkleSilDuz.Parameters["@bilgisayar_id"].Value = infoComputer.Id;
                cmBilgisayarEkleSilDuz.ExecuteNonQuery();
                return true;
            }
            catch (Exception)
            {
            }
            return false;

        }

        public static bool InsertOrUpdateMonitor(Monitor infoMonitor, bool isEdit)
        {
            try
            {
                bool shouldBeEdit = (infoMonitor.Mon_id>0) && isEdit;
                cmMonitorEkleSilDuz.Connection = GlobalDataAccess.Get_Fresh_SQL_Connection();
                bool res = GlobalDataAccess.Open_SQL_Connection(cmMonitorEkleSilDuz.Connection);
                if (res)
                {
                    if (!shouldBeEdit)
                    {
                        cmMonitorEkleSilDuz.Parameters["@type"].Value = "E";
                    }
                    else
                    {
                        cmMonitorEkleSilDuz.Parameters["@type"].Value = "D";
                        cmMonitorEkleSilDuz.Parameters["@monitor_id"].Value = infoMonitor.Mon_id;
                    }
                    cmMonitorEkleSilDuz.Parameters["@parca_id"].Value = infoMonitor.Id;
                    cmMonitorEkleSilDuz.Parameters["@monitor_tipi"].Value = infoMonitor.MonType;
                    cmMonitorEkleSilDuz.Parameters["@stok_no"].Value = infoMonitor.StokNo;
                    cmMonitorEkleSilDuz.ExecuteNonQuery();
                    if (!shouldBeEdit)
                    {
                        int monID = Convert.ToInt32(cmMonitorEkleSilDuz.Parameters["@temp_monitor_id"].Value);
                        infoMonitor.Mon_id = monID;
                    }
                    return true;
                }
            }
            catch (Exception)
            {
            }
            return false;

        }

        public static bool InsertOrUpdateOemDevice(OEMDevice deviceOem, int bilgisayar_id, bool isEdit)
        {
            try
            {
                bool isInDatabase = (deviceOem.Id > 0);
                bool shouldBeEdit = isInDatabase && isEdit;
                cmParcaEkleSilDuz.Connection = GlobalDataAccess.Get_Fresh_SQL_Connection();
                bool res = GlobalDataAccess.Open_SQL_Connection(cmParcaEkleSilDuz.Connection);
                if (res)
                {

                    if (shouldBeEdit)
                    {
                        cmParcaEkleSilDuz.Parameters["@type"].Value = "D";
                        cmParcaEkleSilDuz.Parameters["@parca_id"].Value = deviceOem.Id;
                    }
                    else
                    {
                        cmParcaEkleSilDuz.Parameters["@type"].Value = "E";                        
                    }
                    if (deviceOem.Marka != null && deviceOem.Marka.MarkaID > 0)
                    {
                        cmParcaEkleSilDuz.Parameters["@marka_id"].Value = deviceOem.Marka.MarkaID;
                    }
                    else{
                        cmParcaEkleSilDuz.Parameters["@marka_id"].Value = null;
                    }
                    if (deviceOem.Tempest != null && deviceOem.Tempest.Id > 0) {
                        cmParcaEkleSilDuz.Parameters["@tempest_id"].Value = deviceOem.Tempest.Id;
                    
                    }
                    else{
                        cmParcaEkleSilDuz.Parameters["@tempest_id"].Value = null;
                    }
                    cmParcaEkleSilDuz.Parameters["@seri_no"].Value = deviceOem.SerialNumber;
                    cmParcaEkleSilDuz.Parameters["@parca_tipi"].Value = (Int16)deviceOem.DeviceType;
                    cmParcaEkleSilDuz.Parameters["@parca_no"].Value = deviceOem.Parca_no;
                    cmParcaEkleSilDuz.Parameters["@parca_tanimi"].Value = deviceOem.DeviceInfo;
                    cmParcaEkleSilDuz.Parameters["@bilgisayar_id"].Value = bilgisayar_id;

                    cmParcaEkleSilDuz.ExecuteNonQuery();
                }
                if (!shouldBeEdit)
                {
                    int devID = Convert.ToInt32(cmParcaEkleSilDuz.Parameters["@temp_parca_id"].Value);
                    deviceOem.Id = devID;
                }
                if (deviceOem.DeviceType == DeviceTypes.MONITOR)
                {
                    InsertOrUpdateMonitor(deviceOem as Monitor, isEdit);
                }
                return true;
            }
            catch (Exception)
            {

            }
            return false;
        }

        public static bool InsertOrUpdateGeneralInfo(ComputerInfo infoComputer, bool isEdit)
        {
            try
            {
                cmBilgisayarEkleSilDuz.Connection = GlobalDataAccess.Get_Fresh_SQL_Connection();
                bool res = GlobalDataAccess.Open_SQL_Connection(cmBilgisayarEkleSilDuz.Connection);
                if (res)
                {
                    if (!isEdit)
                    {
                        cmBilgisayarEkleSilDuz.Parameters["@type"].Value = "E";
                    }
                    else
                    {
                        cmBilgisayarEkleSilDuz.Parameters["@type"].Value = "D";
                        cmBilgisayarEkleSilDuz.Parameters["@bilgisayar_id"].Value = infoComputer.Id;
                    }


                    if (infoComputer.Marka.MarkaID > 0)
                    {
                        cmBilgisayarEkleSilDuz.Parameters["@marka_id"].Value = infoComputer.Marka.MarkaID;
                    }
                    else {
                        cmBilgisayarEkleSilDuz.Parameters["@marka_id"].Value = null;
                    }
                    cmBilgisayarEkleSilDuz.Parameters["@pc_adi"].Value = infoComputer.Pc_adi;
                    if (infoComputer.NetworkInfo.BagliAg != null &&
                        infoComputer.NetworkInfo.BagliAg.Ag_id > 0)
                    {
                        cmBilgisayarEkleSilDuz.Parameters["@bagli_ag_id"].Value = infoComputer.NetworkInfo.BagliAg.Ag_id;
                    }
                    else {
                        cmBilgisayarEkleSilDuz.Parameters["@bagli_ag_id"].Value = null;
                    }
                    if (infoComputer.Tempest != null && infoComputer.Tempest.Id > 0)
                    {

                        cmBilgisayarEkleSilDuz.Parameters["@tempest_id"].Value = infoComputer.Tempest.Id;
                    }
                    else {
                        cmBilgisayarEkleSilDuz.Parameters["@tempest_id"].Value = null;
                    }


                    cmBilgisayarEkleSilDuz.Parameters["@mac"].Value = infoComputer.NetworkInfo.MacAddressString;
                    cmBilgisayarEkleSilDuz.Parameters["@model"].Value = infoComputer.Model;
                    cmBilgisayarEkleSilDuz.Parameters["@pc_stok_no"].Value = infoComputer.PcStokNo;
                    cmBilgisayarEkleSilDuz.Parameters["@parca_no"].Value = infoComputer.DeviceNo;
                    cmBilgisayarEkleSilDuz.Parameters["@seri_no"].Value = infoComputer.SerialNumber;
                    cmBilgisayarEkleSilDuz.Parameters["@kayit_ekleme_tarihi"].Value = DateTime.Now;
                    cmBilgisayarEkleSilDuz.Parameters["@notlar"].Value = infoComputer.Notlar;

                    cmBilgisayarEkleSilDuz.ExecuteNonQuery();
                }
                if (!isEdit)
                {
                    int pcID = Convert.ToInt32(cmBilgisayarEkleSilDuz.Parameters["@temp_bilgisayar_id"].Value);
                    infoComputer.Id = pcID;
                }

                return true;
            }
            catch (Exception) { return false; }
        }

        public static bool InsertOrUpdateSenet(ComputerInfo infoComputer, bool isEdit)
        {
            try
            {
                cmSenetEkleDilDuz.Connection = GlobalDataAccess.Get_Fresh_SQL_Connection();
                bool res = GlobalDataAccess.Open_SQL_Connection(cmSenetEkleDilDuz.Connection);
                if (res)
                {
                    if (!isEdit)
                    {
                        cmSenetEkleDilDuz.Parameters["@type"].Value = "E";
                    }
                    else
                    {
                        cmSenetEkleDilDuz.Parameters["@type"].Value = "D";
                        cmSenetEkleDilDuz.Parameters["@senet_id"].Value = infoComputer.Senet.Id;
                    }
                    cmSenetEkleDilDuz.Parameters["@bilgisayar_id"].Value = infoComputer.Id;
                    cmSenetEkleDilDuz.Parameters["@alan_kisi_rutbe"].Value = infoComputer.Senet.Alan_kisi_rutbe;
                    cmSenetEkleDilDuz.Parameters["@alan_kisi_isim"].Value = infoComputer.Senet.Alan_kisi_isim;
                    cmSenetEkleDilDuz.Parameters["@veren_kisi_isim"].Value = infoComputer.Senet.Veren_kisi_isim;

                    if (infoComputer.Senet.Alan_kisi_komutanlik.Komutanlik_id > 0)
                    {
                        cmSenetEkleDilDuz.Parameters["@alan_kisi_komutanlik_id"].Value = infoComputer.Senet.Alan_kisi_komutanlik.Komutanlik_id;
                        if (infoComputer.Senet.Alan_kisi_birlik.Birlik_id > 0)
                        {
                            cmSenetEkleDilDuz.Parameters["@alan_kisi_birilk_id"].Value = infoComputer.Senet.Alan_kisi_birlik.Birlik_id;
                            if (infoComputer.Senet.Alan_kisi_kisim.Kisim_id > 0)
                            {
                                cmSenetEkleDilDuz.Parameters["@alan_kisi_kisim_id"].Value = infoComputer.Senet.Alan_kisi_kisim.Kisim_id;
                            }
                            else
                            {
                                cmSenetEkleDilDuz.Parameters["@alan_kisi_kisim_id"].Value = null;
                            }
                        }
                        else
                        {
                            cmSenetEkleDilDuz.Parameters["@alan_kisi_birilk_id"].Value = null;
                        }
                    }
                    else {
                        cmSenetEkleDilDuz.Parameters["@alan_kisi_komutanlik_id"].Value = null;
                    }
                    cmSenetEkleDilDuz.ExecuteNonQuery();
                    if (!isEdit)
                    {
                        int senetID = Convert.ToInt32(cmSenetEkleDilDuz.Parameters["@temp_senet_id"].Value);
                        infoComputer.Senet.Id = senetID;
                    }
                }


                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static bool InsertOrUpdateComputerInfo(ComputerInfo freshComputerInfo, bool isEdit)
        {
            try
            {
                bool resultComputer = DBFunctions.InsertOrUpdateGeneralInfo(freshComputerInfo, isEdit);
                if (resultComputer)
                {
                    // Monitörü ekle
                    bool isOk = true;
                    bool resultMonitor = DBFunctions.InsertOrUpdateOemDevice(freshComputerInfo.MonitorInfo, freshComputerInfo.Id, isEdit);
                    isOk |= resultComputer;
                    // Monitörü ekle

                    // Parçaları Ekle
                    foreach (var item in freshComputerInfo.GetOemDevices())
                    {
                        if (item.shouldUpdate || !isEdit)
                        {
                            bool resultOem = DBFunctions.InsertOrUpdateOemDevice(item, freshComputerInfo.Id, isEdit);
                        }
                    }
                    // Parçaları Ekle                    
                    bool resultSenet = DBFunctions.InsertOrUpdateSenet(freshComputerInfo, isEdit);
                    isOk |= resultSenet;

                    return isOk;
                }
            }
            catch (Exception) { }
            return false;
        }
    }
}
