using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.IO;
using DatabaseConnection;
using System.Windows;
using Mebs_Envanter.Hardware;
using Mebs_Envanter;
using Mebs_Envanter.GeneralObjects;
using System.Data.Common;
using System.Data.SqlClient;

namespace Mebs_Envanter.DB
{
    public class DBFunctions
    {
        static DBFunctions()
        {
            prepareStoredProcedures();
        }
        
        public static DbConnection ProviceConnection()
        {                            
            // Bağlantı metni zaten oluşturulduysa tekrar bağlantı kontrolü yapma
            if (GlobalDataAccess.Get_Fresh_Connection() != null)
                return GlobalDataAccess.Get_Fresh_Connection();

            String currentDir = Directory.GetCurrentDirectory();
            String xmlPath = currentDir + "\\DBSettings.xml";

            ConnectionProvider connecitonProvider = new ConnectionProvider(xmlPath, "", "", true, false);

            connecitonProvider.ConnectionInformation += new DataBaseErrorHandler(connecitonProvider_ConnectionInformation);
            return connecitonProvider.Start(false);
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

        static DbCommand cmBilgisayarEkleSilDuz;
        static DbCommand cmParcaEkleSilDuz;
        static DbCommand cmMonitorEkleSilDuz;
        static DbCommand cmSenetEkleDilDuz;
        static DbCommand cmYaziciEkleSilDuz;

        /// <summary>
        /// Stored Procedure'ların parametreleri ayarlanıyor.
        /// </summary>
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
            cmBilgisayarEkleSilDuz.Parameters.Add(new SqlParameter("@kayit_ekleme_tarihi", SqlDbType.DateTime));
            cmBilgisayarEkleSilDuz.Parameters.Add(new SqlParameter("@notlar", SqlDbType.NVarChar, 200));
            cmBilgisayarEkleSilDuz.Parameters.Add(new SqlParameter("@senet_id", SqlDbType.Int));
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
            cmParcaEkleSilDuz.Parameters.Add(new SqlParameter("@model", SqlDbType.NVarChar, 50));
            cmParcaEkleSilDuz.Parameters.Add(new SqlParameter("@tempest_id", SqlDbType.Int));
            cmParcaEkleSilDuz.Parameters.Add(new SqlParameter("@parca_tipi", SqlDbType.SmallInt));
            cmParcaEkleSilDuz.Parameters.Add(new SqlParameter("@parca_adedi", SqlDbType.SmallInt));
            cmParcaEkleSilDuz.Parameters.Add(new SqlParameter("@senet_id", SqlDbType.Int));
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
            cmYaziciEkleSilDuz.Parameters.Add(new SqlParameter("@tip_id", SqlDbType.Int));
            cmYaziciEkleSilDuz.Parameters.Add(new SqlParameter("@ip_adresi", SqlDbType.NVarChar, 50));
            cmYaziciEkleSilDuz.Parameters.Add(new SqlParameter("@bagli_ag_id", SqlDbType.Int));
            cmYaziciEkleSilDuz.Parameters.Add(new SqlParameter("@temp_yazici_id", SqlDbType.Int));
            cmYaziciEkleSilDuz.Parameters["@temp_yazici_id"].Direction = ParameterDirection.Output;

            cmSenetEkleDilDuz = new SqlCommand("p_senet_ek_sil_duz");
            cmSenetEkleDilDuz.CommandType = CommandType.StoredProcedure;
            cmSenetEkleDilDuz.Parameters.Add(new SqlParameter("@temp_senet_id", SqlDbType.Int));
            cmSenetEkleDilDuz.Parameters["@temp_senet_id"].Direction = ParameterDirection.Output;
            cmSenetEkleDilDuz.Parameters.Add(new SqlParameter("@type", SqlDbType.NVarChar, 5));
            cmSenetEkleDilDuz.Parameters.Add(new SqlParameter("@senet_id", SqlDbType.Int));
            cmSenetEkleDilDuz.Parameters.Add(new SqlParameter("@alan_kisi_rutbe", SqlDbType.NVarChar, 50));
            cmSenetEkleDilDuz.Parameters.Add(new SqlParameter("@alan_kisi_isim", SqlDbType.NVarChar, 50));
            cmSenetEkleDilDuz.Parameters.Add(new SqlParameter("@veren_kisi_isim", SqlDbType.NVarChar, 50));
            cmSenetEkleDilDuz.Parameters.Add(new SqlParameter("@alan_kisi_birlik_id", SqlDbType.Int));
            cmSenetEkleDilDuz.Parameters.Add(new SqlParameter("@alan_kisi_komutanlik_id", SqlDbType.Int));
            cmSenetEkleDilDuz.Parameters.Add(new SqlParameter("@alan_kisi_kisim_id", SqlDbType.Int));
        }

        /// <summary>
        /// Bağımsız cihazı siler.
        /// </summary>
        /// <param name="infoDevice">Silinecek cihazın işaretçisi</param>
        /// <returns></returns>
        public static bool DeleteIndividualDevice(IndividualDeviceInfo infoDevice)
        {
            try
            {
                cmParcaEkleSilDuz.Connection = GlobalDataAccess.Get_Fresh_Connection();
                bool res = DBCommonAccess.Open_DB_Connection(cmParcaEkleSilDuz.Connection);
                if (res)
                { cmParcaEkleSilDuz.Parameters["@type"].Value = "S"; }

                cmParcaEkleSilDuz.Parameters["@parca_id"].Value = infoDevice.Id;
                cmParcaEkleSilDuz.ExecuteNonQuery();
                return true;
            }
            catch (Exception)
            {
            }
            return false;
        }

        /// <summary>
        /// Bilgisayarı siler.
        /// </summary>
        /// <param name="infoComputer">Silinecek bilgisayarın işaretçisi</param>
        /// <returns></returns>
        public static bool DeleteComputerInfo(ComputerInfo infoComputer)
        {
            try
            {
                cmBilgisayarEkleSilDuz.Connection = GlobalDataAccess.Get_Fresh_Connection();
                bool res = DBCommonAccess.Open_DB_Connection(cmBilgisayarEkleSilDuz.Connection);
                if (res)
                { cmBilgisayarEkleSilDuz.Parameters["@type"].Value = "S"; }

                cmBilgisayarEkleSilDuz.Parameters["@bilgisayar_id"].Value = infoComputer.Id;
                cmBilgisayarEkleSilDuz.ExecuteNonQuery();
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Database'deki Monitor bilgisini düzenler ya da ekler.
        /// </summary>
        /// <param name="infoMonitor"></param>
        /// <param name="isEdit"></param>
        /// <returns></returns>
        private static bool InsertOrUpdateMonitor(Monitor infoMonitor, bool isEdit)
        {
            try
            {
                bool shouldBeEdit = (infoMonitor.Mon_id > 0) && isEdit;
                cmMonitorEkleSilDuz.Connection = GlobalDataAccess.Get_Fresh_Connection();
                bool res = DBCommonAccess.Open_DB_Connection(cmMonitorEkleSilDuz.Connection);
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
                    if (infoMonitor.MonSize != null && infoMonitor.MonSize.Id > 0)
                    {

                        cmMonitorEkleSilDuz.Parameters["@boyut_id"].Value = infoMonitor.MonSize.Id;
                    }
                    else
                    {
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

        private static bool InsertOrUpdatePrinter(YaziciInfo infoYazici, bool isEdit)
        {
            try
            {
                bool shouldBeEdit = (infoYazici.Id_Dev > 0) && isEdit;
                cmYaziciEkleSilDuz.Connection = GlobalDataAccess.Get_Fresh_Connection();
                bool res = DBCommonAccess.Open_DB_Connection(cmYaziciEkleSilDuz.Connection);
                if (res)
                {
                    if (!shouldBeEdit)
                    {
                        cmYaziciEkleSilDuz.Parameters["@type"].Value = "E";
                    }
                    else
                    {
                        cmYaziciEkleSilDuz.Parameters["@type"].Value = "D";
                        cmYaziciEkleSilDuz.Parameters["@yazici_id"].Value = infoYazici.Id_Dev;
                    }
                    cmYaziciEkleSilDuz.Parameters["@parca_id"].Value = infoYazici.Id;
                    cmYaziciEkleSilDuz.Parameters["@ip_adresi"].Value = infoYazici.NetworkInfo.IpAddress;
                    if (infoYazici.NetworkInfo.BagliAg != null && infoYazici.NetworkInfo.BagliAg.Id > 0)
                    {
                        cmYaziciEkleSilDuz.Parameters["@bagli_ag_id"].Value = infoYazici.NetworkInfo.BagliAg.Id;
                    }

                    if (infoYazici.YaziciTipi != null && infoYazici.YaziciTipi.Id > 0)
                    {
                        cmYaziciEkleSilDuz.Parameters["@tip_id"].Value = infoYazici.YaziciTipi.Id;
                    }
                    cmYaziciEkleSilDuz.ExecuteNonQuery();
                    if (!shouldBeEdit)
                    {
                        int yazID = Convert.ToInt32(cmYaziciEkleSilDuz.Parameters["@temp_yazici_id"].Value);
                        infoYazici.Id_Dev = yazID;
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
                cmParcaEkleSilDuz.Connection = GlobalDataAccess.Get_Fresh_Connection();
                bool res = DBCommonAccess.Open_DB_Connection(cmParcaEkleSilDuz.Connection);
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

                    if (deviceOem.Senet.Id > 0)
                    {
                        cmParcaEkleSilDuz.Parameters["@senet_id"].Value = deviceOem.Senet.Id;
                    }

                    if (deviceOem.Marka != null && deviceOem.Marka.Id > 0)
                    {
                        cmParcaEkleSilDuz.Parameters["@marka_id"].Value = deviceOem.Marka.Id;
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
                    cmParcaEkleSilDuz.Parameters["@model"].Value = deviceOem.Model;
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
                    InsertOrUpdatePrinter(deviceOem as YaziciInfo, isEdit);
                }
                return true;
            }
            catch (Exception)
            {

            }
            return false;
        }

        public static bool InsertOrUpdateComputerGeneralInfo(ComputerInfo infoComputer, bool isEdit)
        {
            try
            {
                cmBilgisayarEkleSilDuz.Connection = GlobalDataAccess.Get_Fresh_Connection();
                bool res = DBCommonAccess.Open_DB_Connection(cmBilgisayarEkleSilDuz.Connection);
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


                    if (infoComputer.Marka.Id > 0)
                    {
                        cmBilgisayarEkleSilDuz.Parameters["@marka_id"].Value = infoComputer.Marka.Id;
                    }
                    else
                    {
                        cmBilgisayarEkleSilDuz.Parameters["@marka_id"].Value = null;
                    }
                    cmBilgisayarEkleSilDuz.Parameters["@pc_adi"].Value = infoComputer.Pc_adi;
                    if (infoComputer.NetworkInfo.BagliAg != null &&
                        infoComputer.NetworkInfo.BagliAg.Id > 0)
                    {
                        cmBilgisayarEkleSilDuz.Parameters["@bagli_ag_id"].Value = infoComputer.NetworkInfo.BagliAg.Id;
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

                    if (infoComputer.Senet != null && infoComputer.Senet.Id > 0)
                    {
                        cmBilgisayarEkleSilDuz.Parameters["@senet_id"].Value = infoComputer.Senet.Id;
                    }
                    else
                    {
                        cmBilgisayarEkleSilDuz.Parameters["@senet_id"].Value = null;
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
            catch (Exception) {
                return false;
            }
        }

        public static int InsertMonitorSize(double size)
        {
            try
            {
                DbConnection cnn = GlobalDataAccess.Get_Fresh_Connection();
                String cmdText = "insert into tbl_monitor_boyutu (monitor_boyutu) values (@monitor_boyutu)" +
                " SELECT IDENT_CURRENT('dbo.tbl_monitor_boyutu')";
                DbCommand cmd = DBCommonAccess.GetCommand(cmdText, cnn);
                
                DBCommonAccess.AddParameterWithValue(cmd, "@monitor_boyutu", size);
                bool res = DBCommonAccess.Open_DB_Connection(cnn);
                if (res)
                {
                    object monsizeid = cmd.ExecuteScalar();
                    return Convert.ToInt32(monsizeid);
                }
            }
            catch (Exception)
            {

            }
            return -1;
        }

        private static int InsertKisim(Kisim kisim, int birlik_id)
        {
            try
            {
                DbConnection cnn = GlobalDataAccess.Get_Fresh_Connection();
                String cmdText = "insert into tbl_kisim (birlik_id,kisim_adi) values (@birlik_id,@kisim_adi)" +
                " SELECT IDENT_CURRENT('dbo.tbl_kisim')";
                DbCommand cmd = DBCommonAccess.GetCommand(cmdText, cnn);

                DBCommonAccess.AddParameterWithValue(cmd, "@birlik_id", birlik_id);
                DBCommonAccess.AddParameterWithValue(cmd, "@kisim_adi", kisim.Kisim_adi);

                bool res = DBCommonAccess.Open_DB_Connection(cnn);
                if (res)
                {
                    object kisimid = cmd.ExecuteScalar();
                    return Convert.ToInt32(kisimid);
                }
            }
            catch (Exception)
            {

            }
            return -1;
        }

        public static bool InsertOrUpdateSenet(SenetInfo infoSenet, bool isEdit)
        {
            try
            {
                cmSenetEkleDilDuz.Connection = GlobalDataAccess.Get_Fresh_Connection();
                bool res = DBCommonAccess.Open_DB_Connection(cmSenetEkleDilDuz.Connection);
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

                    cmSenetEkleDilDuz.Parameters["@alan_kisi_rutbe"].Value = infoSenet.Alan_kisi_rutbe;
                    cmSenetEkleDilDuz.Parameters["@alan_kisi_isim"].Value = infoSenet.Alan_kisi_isim;
                    cmSenetEkleDilDuz.Parameters["@veren_kisi_isim"].Value = infoSenet.Veren_kisi_isim;

                    if (infoSenet.Alan_kisi_komutanlik.Id > 0)
                    {
                        cmSenetEkleDilDuz.Parameters["@alan_kisi_komutanlik_id"].Value = infoSenet.Alan_kisi_komutanlik.Id;
                        if (infoSenet.Alan_kisi_birlik.Id > 0)
                        {
                            cmSenetEkleDilDuz.Parameters["@alan_kisi_birlik_id"].Value = infoSenet.Alan_kisi_birlik.Id;
                            if (infoSenet.Alan_kisi_kisim.Id > 0)
                            {
                                cmSenetEkleDilDuz.Parameters["@alan_kisi_kisim_id"].Value = infoSenet.Alan_kisi_kisim.Id;
                            }
                            else if (!String.IsNullOrEmpty(infoSenet.Alan_kisi_kisim.Kisim_adi))
                            {
                                int newId = InsertKisim(infoSenet.Alan_kisi_kisim, infoSenet.Alan_kisi_birlik.Id);

                                if (newId > 0)
                                {
                                    cmSenetEkleDilDuz.Parameters["@alan_kisi_kisim_id"].Value = newId;
                                    infoSenet.Alan_kisi_kisim.Id = newId;
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
                bool isOk = true;
                bool resultSenet = DBFunctions.InsertOrUpdateSenet(freshComputerInfo.Senet, isEdit);
                isOk |= resultSenet;
                bool resultComputer = DBFunctions.InsertOrUpdateComputerGeneralInfo(freshComputerInfo, isEdit);
                if (resultComputer)
                {
                    // Monitörü ekle
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
                    return isOk;
                }
            }
            catch (Exception) { }
            return false;
        }

        /// <summary>
        /// Tek eleman döndüren basit sql sorguları için kullanılır.         
        /// </summary>
        /// <param name="command">ör : select lastname from tablo_persons</param>
        /// <param name="itemName">ör :  lastname</param>
        /// <returns>ör : "kazım"</returns>
        public static object ExecuteToFetchSingleItem(String command, String itemName)
        {
            IDataReader dr = null;
            try
            {
                DbConnection cnn = GlobalDataAccess.Get_Fresh_Connection();
                IDbCommand cmd = DBCommonAccess.GetCommand(command, cnn);
                bool res = DBCommonAccess.Open_DB_Connection(cnn);

                dr = cmd.ExecuteReader();
                object obj = null;

                if (dr.Read())
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

        public static DataTable FillTable(String commandText, IEnumerable<KeyValuePair<String, object>> parameters)
        {

            DbConnection cnn = GlobalDataAccess.Get_Fresh_Connection();

            DbCommand cmd = null;

            String conString = commandText;
            cmd = DBCommonAccess.GetCommand(conString, cnn);
            foreach (var item in parameters)
            {
                DBCommonAccess.AddParameterWithValue(cmd, item.Key, item.Value);
            }

            DbDataAdapter adp = DBCommonAccess.GetAdapter(cmd);
            DataTable dt = new DataTable();

            bool res = DBCommonAccess.Open_DB_Connection(cnn);
            try
            {
                adp.Fill(dt);
            }
            catch (Exception)
            {
            }
            finally
            {
                cnn.Close();
                cnn.Dispose();
            }
            return dt;
        }
    }
}
