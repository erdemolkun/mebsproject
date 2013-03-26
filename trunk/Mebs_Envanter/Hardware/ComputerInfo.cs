using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Data;
using MEBS_Envanter.GeneralObjects;
using System.Data.SqlClient;
using MEBS_Envanter.DB;
using Mebs_Envanter;
using Mebs_Envanter.GeneralObjects;
using Mebs_Envanter.DB;
using System.Windows;
using Mebs_Envanter.Hardware;

namespace MEBS_Envanter
{
    public class ComputerInfo : MebsBaseObject
    {
        public bool IsEdit = false;
        public ComputerInfo()
        {
            NetworkInfo = new NetworkInfo();
            MonitorInfo = new Monitor();
            Senet = new SenetInfo();
            EklenmeTarihi = new DateTime(2010, 9, 12);

            bool designTime = System.ComponentModel.DesignerProperties.GetIsInDesignMode(new DependencyObject());
            if (designTime)
            {
                Pc_adi = "PC_ADI_DESIGN_TIME";
            }
        }

        public void SetGeneralFields(DataRow rowPC)
        {
            Id = (int)rowPC["bilgisayar_id"];

            int markaid = DBValueHelpers.GetInt32(rowPC["marka_id"].ToString(), -1);
            //if (!String.IsNullOrEmpty(rowPC["marka_id"].ToString())) {
            //    markaid = (int)rowPC["marka_id"];
            //}
            //try {
            //    bool x = String.IsNullOrEmpty(rowPC["marka_id"].ToString());
            //    int deneme = -1;
            //    Int32.TryParse(rowPC["marka_id"].ToString(), out deneme);
            //    //markaid = (int)rowPC["marka_id"]; if (markaid < 0)markaid = -1;
            //}
            //catch (Exception) {
            //}

            Marka = new Marka(markaid, "");


            int bagli_ag_id = DBValueHelpers.GetInt32(rowPC["bagli_ag_id"].ToString(), -1);


            NetworkInfo.BagliAg = new BagliAg("", bagli_ag_id);


            int tempest_id = DBValueHelpers.GetInt32(rowPC["tempest_id"].ToString(), -1);

            NetworkInfo.MacAddressString = rowPC["mac"].ToString();


            Pc_adi = rowPC["pc_adi"].ToString(); ;
            PcStokNo = rowPC["pc_stok_no"].ToString();
            Model = rowPC["model"].ToString();
            SerialNumber = rowPC["seri_no"].ToString();
            DeviceNo = rowPC["parca_no"].ToString();
            Notlar = rowPC["notlar"].ToString();
            Tempest = new Tempest(tempest_id, "");
        }

        internal void Set_MonitorInfo(OEMDevice devOem)
        {
            SqlConnection cnn = GlobalDataAccess.Get_Fresh_SQL_Connection();
            String conString = "Select * From tbl_monitor where parca_id=@parca_id";
            SqlCommand cmd = new SqlCommand(conString, cnn);
            cmd.Parameters.AddWithValue("@parca_id", devOem.Id);
            SqlDataAdapter adp = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            bool res = GlobalDataAccess.Open_SQL_Connection(cnn);
            if (res)
            {
                try
                {
                    adp.Fill(dt);
                }
                catch (Exception)
                {}
                finally
                {
                    cnn.Close();
                    cnn.Dispose();
                }
                foreach (DataRow rowMonitor in dt.Rows)
                {
                    String stok_no = rowMonitor["stok_no"].ToString();
                    int mon_id = (int)rowMonitor["monitor_id"];
                    int mon_type = DBValueHelpers.GetInt32(rowMonitor["monitor_tipi"], -1);

                    if (mon_type > 0)
                    {
                        (devOem as Monitor).MonType = (MonitorTypes)mon_type;
                    }
                    (devOem as Monitor).StokNo = stok_no;
                    (devOem as Monitor).Mon_id = mon_id;
                }
            }
        }

        internal void Set_YaziciInfo(OEMDevice devOem)
        {
            SqlConnection cnn = GlobalDataAccess.Get_Fresh_SQL_Connection();
            String conString = "Select * From tbl_yazici where parca_id=@parca_id";
            SqlCommand cmd = new SqlCommand(conString, cnn);
            cmd.Parameters.AddWithValue("@parca_id", devOem.Id);
            SqlDataAdapter adp = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            bool res = GlobalDataAccess.Open_SQL_Connection(cnn);
            if (res)
            {
                try
                {
                    adp.Fill(dt);
                }
                catch (Exception)
                { }
                finally
                {
                    cnn.Close();
                    cnn.Dispose();
                }
                foreach (DataRow rowYazici in dt.Rows)
                {
                    String yazici_modeli = rowYazici["yazici_modeli"].ToString();
                    int yaz_id = (int)rowYazici["yazici_id"];
                    //int mon_type = DBValueHelpers.GetInt32(rowYazici["monitor_tipi"], -1);

                   /* if (mon_type > 0)
                    {
                        (devOem as Monitor).MonType = (MonitorTypes)mon_type;
                    }*/
                    (devOem as YaziciInfo).YaziciModeli = yazici_modeli;
                    (devOem as YaziciInfo).Yaz_id = yaz_id;
                }
            }
        }

        internal void Set_SenetInfos()
        {
            SqlConnection cnn = GlobalDataAccess.Get_Fresh_SQL_Connection();
            String conString = "Select * From tbl_senet where bilgisayar_id=@bilgisayar_id";
            SqlCommand cmd = new SqlCommand(conString, cnn);
            cmd.Parameters.AddWithValue("@bilgisayar_id", Id);
            SqlDataAdapter adp = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();

            bool res = GlobalDataAccess.Open_SQL_Connection(cnn);
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
            foreach (DataRow rowParca in dt.Rows)
            {
                String alan_kisi_rutbe = rowParca["alan_kisi_rutbe"].ToString();
                String alan_kisi_isim = rowParca["alan_kisi_isim"].ToString();
                String veren_kisi_isim = rowParca["veren_kisi_isim"].ToString();

                Senet.Alan_kisi_isim = alan_kisi_isim;
                Senet.Alan_kisi_rutbe = alan_kisi_rutbe;
                Senet.Veren_kisi_isim = veren_kisi_isim;


                int alanKisiKomutanlikId = DBValueHelpers.GetInt32(rowParca["alan_kisi_komutanlik_id"], -1);
                int alanKisiBirlikId = DBValueHelpers.GetInt32(rowParca["alan_kisi_birilk_id"], -1);
                int alanKisiKisimId = DBValueHelpers.GetInt32(rowParca["alan_kisi_kisim_id"], -1);

                Senet.Alan_kisi_komutanlik = new Komutanlik(alanKisiKomutanlikId, "");
                Senet.Alan_kisi_birlik = new Birlik(alanKisiBirlikId, "");
                Senet.Alan_kisi_kisim = new Kisim(alanKisiKisimId, "");
                Senet.Id = Convert.ToInt32(rowParca["senet_id"]);
            }
        }

        internal void Set_HardwareInfos(SqlConnection sqlCon)
        {
            SqlConnection cnn = sqlCon;//GlobalDataAccess.Get_Fresh_SQL_Connection();

            String conString = "Select * From tbl_parca where bilgisayar_id=@bilgisayar_id";
            SqlCommand cmd = new SqlCommand(conString, cnn);
            cmd.Parameters.AddWithValue("@bilgisayar_id", Id);
            SqlDataAdapter adp = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();

            bool res = GlobalDataAccess.Open_SQL_Connection(cnn);
            if (res)
            {
                try
                {
                    adp.Fill(dt);
                    #region Fill Hardware Properties

                    foreach (DataRow rowParca in dt.Rows)
                    {
                        int parcaTipi = DBValueHelpers.GetInt32(rowParca["parca_tipi"], (int)DeviceTypes.NONE);
                        DeviceTypes tip = (DeviceTypes)parcaTipi;
                        int parca_id = (int)rowParca["parca_id"];
                        String seri_no = rowParca["seri_no"].ToString();
                        String parca_tanimi = rowParca["parca_tanimi"].ToString();
                        String parca_no = rowParca["parca_no"].ToString();

                        int markaid = DBValueHelpers.GetInt32(rowParca["marka_id"], -1);
                        int tempestid = DBValueHelpers.GetInt32(rowParca["tempest_id"], -1);


                        OEMDevice devOem = null;
                        if (tip == DeviceTypes.MONITOR) {

                            devOem = new Monitor();
                        }
                        else if (tip == DeviceTypes.PRINTER)
                        {
                            devOem = new YaziciInfo();
                        }
                        else {
                            devOem = new OEMDevice(tip);
                        }
                        
                        //Ortak alanlar
                        devOem.Id = parca_id;
                        devOem.SerialNumber = seri_no;
                        devOem.Parca_no = parca_no;
                        devOem.Marka = new Marka(markaid, "");
                        devOem.Tempest = new Tempest(tempestid, "");
                        devOem.DeviceInfo = parca_tanimi;

                        if (tip == DeviceTypes.MONITOR)
                        {
                            Monitor mon = devOem as Monitor;
                            //mon.Id = parca_id;
                            //mon.SerialNumber = seri_no;
                            //mon.Parca_no = parca_no;
                            //mon.Marka = new Marka(markaid, "");
                            //mon.Tempest = new Tempest(tempestid, "");
                            Set_MonitorInfo(mon);
                            MonitorInfo = mon;
                        }
                        else if (tip == DeviceTypes.PRINTER)
                        {
                            YaziciInfo infYazi = devOem as YaziciInfo;
                            Set_YaziciInfo(infYazi);
                            YaziciInfo = infYazi;
                            
                        }
                        else 
                        {
                            //OEMDevice devOem = new OEMDevice(tip);
                            //devOem.Id = parca_id;
                            //devOem.Marka = new Marka(markaid, "");
                            //devOem.SerialNumber = seri_no;
                            //devOem.Parca_no = parca_no;
                            //devOem.DeviceInfo = parca_tanimi;
                            OemDevicesVModel.AssignOemDevice(devOem);
                        }
                    }

                    #endregion
                }
                catch (Exception)
                {

                }
                finally
                {
                    //cnn.Close();
                    //cnn.Dispose();
                }
            }

        }

        internal OEMDevice Get_OemDevice(DeviceTypes devType)
        {
            foreach (var item in OemDevicesVModel.OemDevices)
            {
                if (item.DevOem.DeviceType == devType)
                {
                    return item.DevOem as OEMDevice;
                }
            }
            foreach (var item in OemDevicesVModel.OemDevicesExtra)
            {
                if (item.DevOem.DeviceType == devType)
                {
                    return item.DevOem as OEMDevice;
                }
            }
            return null;
        }

        #region All Properties

        public IEnumerable<OEMDevice> GetOemDevices()
        {

            foreach (var item in OemDevicesVModel.OemDevices)
            {
                yield return item.DevOem;
            }
            foreach (var item in OemDevicesVModel.OemDevicesExtra)
            {
                yield return item.DevOem;
            }
        }


        private OemDevicesViewModel oemDevicesVModel = new OemDevicesViewModel();
        public OemDevicesViewModel OemDevicesVModel
        {
            get { return oemDevicesVModel; }
            set
            {
                oemDevicesVModel = value;
                OnPropertyChanged("OemDevicesVModel");
            }
        }

        private NetworkInfo networkInfo;
        public NetworkInfo NetworkInfo
        {
            get { return networkInfo; }
            set
            {
                networkInfo = value;
                OnPropertyChanged("NetworkInfo");
            }
        }

        private SenetInfo senet = new SenetInfo();
        public SenetInfo Senet
        {
            get { return senet; }
            set { senet = value; OnPropertyChanged("Senet"); }
        }

        private Monitor monitorInfo = new Monitor();
        public Monitor MonitorInfo
        {
            get { return monitorInfo; }
            set { monitorInfo = value; OnPropertyChanged("MonitorInfo"); }
        }

        private YaziciInfo yaziciInfo = new YaziciInfo();

        public YaziciInfo YaziciInfo
        {
            get { return yaziciInfo; }
            set { yaziciInfo = value; OnPropertyChanged("YaziciInfo"); }
        }


        private string pc_adi;
        /// <summary>
        /// Bilgisayar Adı
        /// </summary>
        public string Pc_adi
        {
            get { return pc_adi; }
            set { pc_adi = value; OnPropertyChanged("Pc_adi"); }
        }


        private String notlar;
        /// <summary>
        /// Bilgisayar hakkında genel notlar
        /// </summary>
        public String Notlar
        {
            get { return notlar; }
            set
            {
                notlar = value;
                OnPropertyChanged("Notlar");
            }
        }

        private String pcStokNo;
        /// <summary>
        /// PC Stok Numarası
        /// </summary>
        public String PcStokNo
        {
            get { return pcStokNo; }
            set { pcStokNo = value; OnPropertyChanged("PcStokNo"); }
        }

        private Marka marka = new Marka(-1, "");
        /// <summary>
        /// Bilgisayarın markası
        /// </summary>
        public Marka Marka
        {
            get { return marka; }
            set { marka = value; OnPropertyChanged("Marka"); }
        }


        private Tempest tempest = new Tempest();
        /// <summary>
        /// Tempest Seviyesi
        /// </summary>
        public Tempest Tempest
        {
            get { return tempest; }
            set { tempest = value; OnPropertyChanged("Tempest"); }
        }


        private String model;
        /// <summary>
        /// Bilgisayarın Modeli
        /// </summary>
        public String Model
        {
            get { return model; }
            set { model = value; OnPropertyChanged("Model"); }
        }

        private int id = -1;

        public int Id
        {
            get { return id; }
            set { id = value; OnPropertyChanged("Id"); }
        }


        private String deviceNo;
        /// <summary>
        /// Parça Numarası
        /// </summary>
        public String DeviceNo
        {
            get { return deviceNo; }
            set { deviceNo = value; OnPropertyChanged("DeviceNo"); }
        }

        private String serialNumber;
        /// <summary>
        /// Seri Numarası
        /// </summary>
        public String SerialNumber
        {
            get { return serialNumber; }
            set { serialNumber = value; OnPropertyChanged("SerialNumber"); }
        }


        private DateTime eklenmeTarihi;
        public DateTime EklenmeTarihi
        {
            get { return eklenmeTarihi; }
            set { eklenmeTarihi = value; OnPropertyChanged("EklenmeTarihi"); }
        }

        #endregion
    }
}
