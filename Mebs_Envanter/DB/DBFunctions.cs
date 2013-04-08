using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.IO;
using DatabaseConnection;
using System.Windows;
using Mebs_Envanter.Hardware;
using Mebs_Envanter;
using MEBS_Envanter.GeneralObjects;

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

        static void connecitonProvider_ConnectionInformation(string error, int hataTipi)
        {
            try
            {
                               
                StringBuilder sb = new StringBuilder();                
                sb.Append(error); //Add tabulation
                sb.Append(System.Environment.NewLine);
                sb.Append("Hata tipi : " + hataTipi); //Add tabulation

                String content = sb.ToString(); // error + "\rHata tipi : " + hataTipi;            

                LoggerMebs.WriteToFile(content);
            }
            catch (Exception)
            {
            }
        }
        static void connecitonProvider_connectionResult(SqlConnection result) { }

        static SqlCommand cmBilgisayarEkleSilDuz;
        static SqlCommand cmParcaEkleSilDuz;
        static SqlCommand cmMonitorEkleSilDuz;
        static SqlCommand cmSenetEkleDilDuz;
        static SqlCommand cmYaziciEkleSilDuz;
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
            cmParcaEkleSilDuz.Parameters.Add(new SqlParameter("@parca_adedi", SqlDbType.SmallInt));
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
            cmMonitorEkleSilDuz.Parameters.Add(new SqlParameter("@boyut_id", SqlDbType.Int));
            cmMonitorEkleSilDuz.Parameters["@temp_monitor_id"].Direction = ParameterDirection.Output;



            cmYaziciEkleSilDuz = new SqlCommand("p_yazici_ek_sil_duz");
            cmYaziciEkleSilDuz.CommandType = CommandType.StoredProcedure;
            cmYaziciEkleSilDuz.Parameters.Add(new SqlParameter("@type", SqlDbType.NVarChar, 5));
            cmYaziciEkleSilDuz.Parameters.Add(new SqlParameter("@parca_id", SqlDbType.Int));
            cmYaziciEkleSilDuz.Parameters.Add(new SqlParameter("@yazici_id", SqlDbType.Int));
            cmYaziciEkleSilDuz.Parameters.Add(new SqlParameter("@senet_id", SqlDbType.Int));
            cmYaziciEkleSilDuz.Parameters.Add(new SqlParameter("@yazici_modeli", SqlDbType.NVarChar, 50));
            cmYaziciEkleSilDuz.Parameters.Add(new SqlParameter("@ip_adresi", SqlDbType.NVarChar, 50));
            cmYaziciEkleSilDuz.Parameters.Add(new SqlParameter("@bagli_ag_id", SqlDbType.Int));
            cmYaziciEkleSilDuz.Parameters.Add(new SqlParameter("@temp_yazici_id", SqlDbType.Int));
            cmYaziciEkleSilDuz.Parameters["@temp_yazici_id"].Direction = ParameterDirection.Output;


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
            cmSenetEkleDilDuz.Parameters.Add(new SqlParameter("@alan_kisi_birlik_id", SqlDbType.Int));
            cmSenetEkleDilDuz.Parameters.Add(new SqlParameter("@alan_kisi_komutanlik_id", SqlDbType.Int));
            cmSenetEkleDilDuz.Parameters.Add(new SqlParameter("@alan_kisi_kisim_id", SqlDbType.Int));
        }

        public static bool DeleteYazici(YaziciInfo infoYazici)
        {
            try
            {
                cmParcaEkleSilDuz.Connection = GlobalDataAccess.Get_Fresh_SQL_Connection();
                bool res = GlobalDataAccess.Open_SQL_Connection(cmParcaEkleSilDuz.Connection);
                if (res)
                { cmParcaEkleSilDuz.Parameters["@type"].Value = "S"; }

                cmParcaEkleSilDuz.Parameters["@parca_id"].Value = infoYazici.Id;
                cmParcaEkleSilDuz.ExecuteNonQuery();
                return true;
            }
            catch (Exception)
            {
            }
            return false;

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
                bool shouldBeEdit = (infoMonitor.Mon_id > 0) && isEdit;
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
                    if (infoMonitor.MonSize != null && infoMonitor.MonSize.Id > 0) {

                        cmMonitorEkleSilDuz.Parameters["@boyut_id"].Value = infoMonitor.MonSize.Id;
                    }
                    else{
                        cmMonitorEkleSilDuz.Parameters["@boyut_id"].Value = null;
                    }
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

        public static bool InsertOrUpdateYazici(YaziciInfo infoYazici, bool isEdit)
        {
            try
            {
                bool shouldBeEdit = (infoYazici.Yaz_id > 0) && isEdit;
                cmYaziciEkleSilDuz.Connection = GlobalDataAccess.Get_Fresh_SQL_Connection();
                bool res = GlobalDataAccess.Open_SQL_Connection(cmYaziciEkleSilDuz.Connection);
                if (res)
                {
                    if (!shouldBeEdit)
                    {
                        cmYaziciEkleSilDuz.Parameters["@type"].Value = "E";
                    }
                    else
                    {
                        cmYaziciEkleSilDuz.Parameters["@type"].Value = "D";
                        cmYaziciEkleSilDuz.Parameters["@yazici_id"].Value = infoYazici.Yaz_id;
                    }
                    cmYaziciEkleSilDuz.Parameters["@parca_id"].Value = infoYazici.Id;
                    cmYaziciEkleSilDuz.Parameters["@ip_adresi"].Value = infoYazici.NetworkInfo.IpAddress;
                    cmYaziciEkleSilDuz.Parameters["@yazici_modeli"].Value = infoYazici.YaziciModeli;
                    if (infoYazici.NetworkInfo.BagliAg != null && infoYazici.NetworkInfo.BagliAg.Ag_id > 0)
                    {
                        cmYaziciEkleSilDuz.Parameters["@bagli_ag_id"].Value = infoYazici.NetworkInfo.BagliAg.Ag_id;
                    }
                    if (infoYazici.SenetInfo.Id > 0)
                    {

                        cmYaziciEkleSilDuz.Parameters["@senet_id"].Value = infoYazici.SenetInfo.Id;
                    }
                    cmYaziciEkleSilDuz.ExecuteNonQuery();
                    if (!shouldBeEdit)
                    {
                        int yazID = Convert.ToInt32(cmYaziciEkleSilDuz.Parameters["@temp_yazici_id"].Value);
                        infoYazici.Yaz_id = yazID;
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
                    else
                    {
                        cmParcaEkleSilDuz.Parameters["@marka_id"].Value = null;
                    }
                    if (deviceOem.Tempest != null && deviceOem.Tempest.Id > 0)
                    {
                        cmParcaEkleSilDuz.Parameters["@tempest_id"].Value = deviceOem.Tempest.Id;
                    }
                    else
                    {
                        cmParcaEkleSilDuz.Parameters["@tempest_id"].Value = null;
                    }
                    cmParcaEkleSilDuz.Parameters["@seri_no"].Value = deviceOem.SerialNumber;
                    cmParcaEkleSilDuz.Parameters["@parca_tipi"].Value = (Int16)deviceOem.DeviceType;
                    cmParcaEkleSilDuz.Parameters["@parca_no"].Value = deviceOem.Parca_no;
                    cmParcaEkleSilDuz.Parameters["@parca_adedi"].Value = deviceOem.Adet;
                    cmParcaEkleSilDuz.Parameters["@parca_tanimi"].Value = deviceOem.DeviceInfo;
                    if (bilgisayar_id > 0)
                    {
                        cmParcaEkleSilDuz.Parameters["@bilgisayar_id"].Value = bilgisayar_id;
                    }
                    else
                    {
                        cmParcaEkleSilDuz.Parameters["@bilgisayar_id"].Value = null;
                    }

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
                else if (deviceOem.DeviceType == DeviceTypes.PRINTER)
                {
                    InsertOrUpdateYazici(deviceOem as YaziciInfo, isEdit);
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
                    else
                    {
                        cmBilgisayarEkleSilDuz.Parameters["@marka_id"].Value = null;
                    }
                    cmBilgisayarEkleSilDuz.Parameters["@pc_adi"].Value = infoComputer.Pc_adi;
                    if (infoComputer.NetworkInfo.BagliAg != null &&
                        infoComputer.NetworkInfo.BagliAg.Ag_id > 0)
                    {
                        cmBilgisayarEkleSilDuz.Parameters["@bagli_ag_id"].Value = infoComputer.NetworkInfo.BagliAg.Ag_id;
                    }
                    else
                    {
                        cmBilgisayarEkleSilDuz.Parameters["@bagli_ag_id"].Value = null;
                    }
                    if (infoComputer.Tempest != null && infoComputer.Tempest.Id > 0)
                    {

                        cmBilgisayarEkleSilDuz.Parameters["@tempest_id"].Value = infoComputer.Tempest.Id;
                    }
                    else
                    {
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


        private static int InsertKisim(Kisim kisim,int birlik_id) {

            try
            {
                SqlConnection cnn = GlobalDataAccess.Get_Fresh_SQL_Connection();
                SqlCommand cmd = new SqlCommand("insert into tbl_kisim (birlik_id,kisim_adi) values (@birlik_id,@kisim_adi)"+

                " SELECT IDENT_CURRENT('dbo.tbl_kisim')", cnn);

                cmd.Parameters.AddWithValue("@birlik_id", birlik_id);
                cmd.Parameters.AddWithValue("@kisim_adi", kisim.Kisim_adi);
                bool res = GlobalDataAccess.Open_SQL_Connection(cnn);
                if (res)
                {
                    object kisimid  = cmd.ExecuteScalar();
                    return Convert.ToInt32(kisimid);
                }
            }
            catch (Exception) { 
                
            }
            return -1;
        }

        public static bool InsertOrUpdateSenet(int bilgisayar_id, SenetInfo infoSenet, bool isEdit)
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
                        cmSenetEkleDilDuz.Parameters["@senet_id"].Value = infoSenet.Id;
                    }
                    if (bilgisayar_id > 0)
                    {
                        cmSenetEkleDilDuz.Parameters["@bilgisayar_id"].Value = bilgisayar_id;
                    }
                    else
                    {
                        cmSenetEkleDilDuz.Parameters["@bilgisayar_id"].Value = null;
                    }
                    cmSenetEkleDilDuz.Parameters["@alan_kisi_rutbe"].Value = infoSenet.Alan_kisi_rutbe;
                    cmSenetEkleDilDuz.Parameters["@alan_kisi_isim"].Value = infoSenet.Alan_kisi_isim;
                    cmSenetEkleDilDuz.Parameters["@veren_kisi_isim"].Value = infoSenet.Veren_kisi_isim;

                    if (infoSenet.Alan_kisi_komutanlik.Komutanlik_id > 0)
                    {
                        cmSenetEkleDilDuz.Parameters["@alan_kisi_komutanlik_id"].Value = infoSenet.Alan_kisi_komutanlik.Komutanlik_id;
                        if (infoSenet.Alan_kisi_birlik.Birlik_id > 0)
                        {
                            cmSenetEkleDilDuz.Parameters["@alan_kisi_birlik_id"].Value = infoSenet.Alan_kisi_birlik.Birlik_id;
                            if (infoSenet.Alan_kisi_kisim.Kisim_id > 0)
                            {
                                cmSenetEkleDilDuz.Parameters["@alan_kisi_kisim_id"].Value = infoSenet.Alan_kisi_kisim.Kisim_id;
                            }
                            else if (!String.IsNullOrEmpty(infoSenet.Alan_kisi_kisim.Kisim_adi))
                            {
                                int newId = InsertKisim(infoSenet.Alan_kisi_kisim, infoSenet.Alan_kisi_birlik.Birlik_id);
                               
                                if (newId > 0)
                                {
                                    cmSenetEkleDilDuz.Parameters["@alan_kisi_kisim_id"].Value = newId;
                                    infoSenet.Alan_kisi_kisim.Kisim_id = newId;
                                }
                            }
                        }
                        else
                        {
                            cmSenetEkleDilDuz.Parameters["@alan_kisi_birlik_id"].Value = null;
                        }
                    }
                    else
                    {
                        cmSenetEkleDilDuz.Parameters["@alan_kisi_komutanlik_id"].Value = null;
                    }
                    cmSenetEkleDilDuz.ExecuteNonQuery();
                    if (!isEdit)
                    {
                        int senetID = Convert.ToInt32(cmSenetEkleDilDuz.Parameters["@temp_senet_id"].Value);
                        infoSenet.Id = senetID;
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
                    bool resultSenet = DBFunctions.InsertOrUpdateSenet(freshComputerInfo.Id, freshComputerInfo.Senet, isEdit);
                    isOk |= resultSenet;

                    return isOk;
                }
            }
            catch (Exception) { }
            return false;
        }

        public static object ExecuteToFetchSingleItem(String command, String itemName)
        {
            SqlDataReader dr = null;
            try
            {
                SqlConnection cnn = GlobalDataAccess.Get_Fresh_SQL_Connection();
                SqlCommand cmd = new SqlCommand(command, cnn);

                bool res = GlobalDataAccess.Open_SQL_Connection(cnn);

                dr = cmd.ExecuteReader();
                object obj = null;
                if (dr.HasRows && dr.Read())
                {
                    obj = dr[itemName];
                }
                dr.Close();
                cnn.Close();

                return obj;
            }
            catch (Exception)
            {
                return null;
            }
            finally
            {
                if (dr != null)
                    dr.Close();
            }
        }
    }
}
