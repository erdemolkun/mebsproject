﻿using System;
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

namespace MEBS_Envanter
{
    public class ComputerInfo : MebsBaseObject
    {
        public bool IsEdit = false;

        public void SetGeneralFields(DataRow rowPC)
        {
            Id = (int)rowPC["bilgisayar_id"];

            int markaid = -1;
            try { markaid = (int)rowPC["marka_id"]; if (markaid < 0)markaid = -1; }
            catch (Exception) { }

            Marka = new Marka(markaid, "");

            int bagli_ag_id = -1;
            try { bagli_ag_id = (int)rowPC["bagli_ag_id"]; if (bagli_ag_id < 0)bagli_ag_id = -1; }
            catch (Exception) { }

            int tempest_id = -1;
            try { tempest_id = (int)rowPC["tempest_id"]; if (tempest_id < 0)tempest_id = -1; }
            catch (Exception) { }

            NetworkInfo.MacAddressString = rowPC["mac"].ToString();
            NetworkInfo.BagliAg = new BagliAg("", bagli_ag_id);

            Pc_adi = rowPC["pc_adi"].ToString(); ;
            PcStokNo = rowPC["pc_stok_no"].ToString();
            Model = rowPC["model"].ToString();
            SerialNumber = rowPC["seri_no"].ToString();
            DeviceNo = rowPC["parca_no"].ToString();
            Tempest = new Tempest(tempest_id, "");
        }

        private void Get_MonitorInfo(OEMDevice devOem)
        {

            SqlConnection cnn = GlobalDataAccess.Get_Fresh_SQL_Connection();
            String conString = "Select * From tbl_monitor where parca_id=@parca_id";
            SqlCommand cmd = new SqlCommand(conString, cnn);
            cmd.Parameters.AddWithValue("@parca_id", devOem.Id);
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
            foreach (DataRow rowMonitor in dt.Rows)
            {

                // int monitorTipi = Convert.ToInt32( rowMonitor["monitor_tipi"]);
                String stok_no = rowMonitor["stok_no"].ToString();
                int mon_id = (int)rowMonitor["monitor_id"];
                (devOem as Monitor).StokNo = stok_no;
                (devOem as Monitor).Mon_id = mon_id;
                //(devOem as Monitor).DeviceNo = 
            }
        }

        internal void Get_SenetInfos()
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

                int alanKisiBirlikId = -1;
                try { alanKisiBirlikId = Convert.ToInt32(rowParca["alan_kisi_birilk_id"]); }
                catch (Exception) { }

                int alanKisiKisimId = -1;
                try { alanKisiKisimId = Convert.ToInt32(rowParca["alan_kisi_kisim_id"]); }
                catch (Exception) { }

                Senet.Alan_kisi_birlik = new Birlik(alanKisiBirlikId, "");
                Senet.Alan_kisi_kisim = new Kisim(alanKisiKisimId, "");
                Senet.Id = Convert.ToInt32(rowParca["senet_id"]);
            }
        }

        internal void Get_HardwareInfos()
        {
            SqlConnection cnn = GlobalDataAccess.Get_Fresh_SQL_Connection();
            String conString = "Select * From tbl_parca where bilgisayar_id=@bilgisayar_id";
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
                int parcaTipi = Convert.ToInt32(rowParca["parca_tipi"]);
                DeviceTypes tip = (DeviceTypes)parcaTipi;
                int parca_id = (int)rowParca["parca_id"];
                String seri_no = rowParca["seri_no"].ToString();
                String parca_tanimi = rowParca["parca_tanimi"].ToString();
                String parca_no = rowParca["parca_no"].ToString();

                int markaid = -1;
                try { 
                    markaid = (int)rowParca["marka_id"]; 
                }
                catch (Exception) { }

                int tempestid = -1;
                try { 
                    tempestid = (int)rowParca["tempest_id"];
                }
                catch (Exception) { }

                if (tip == DeviceTypes.MONITOR)
                {
                    Monitor mon = new Monitor();
                    mon.Id = parca_id;
                    mon.SerialNumber = seri_no;
                    mon.Parca_no = parca_no;
                    mon.Marka = new Marka(markaid, "");
                    MonitorInfo = mon;
                    mon.Tempest = new Tempest(tempestid,"");
                    Get_MonitorInfo(mon);
                }
                else
                {
                    OEMDevice devOem = new OEMDevice(tip);
                    devOem.Id = parca_id;
                    devOem.Marka = new Marka(markaid,"");
                    devOem.SerialNumber = seri_no;
                    devOem.Parca_no = parca_no;
                    devOem.DeviceInfo = parca_tanimi;
                    OemDevicesVModel.SetOemDevice(devOem);
                    // Tüm parçaları objelere ata
                }
            }
        }

        public ComputerInfo()
        {            
            NetworkInfo = new NetworkInfo();
            MonitorInfo = new Monitor();
            Senet = new SenetInfo();
            EklenmeTarihi = new DateTime(2010, 9, 12);
        }

        internal OEMDevice Get_OemDevice(DeviceTypes devType) {

            foreach (var item in OemDevicesVModel.OemDevices)
            {
                if (item.DevOem.DeviceType == devType) {
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

        public IEnumerable<OEMDevice> GetOemDevices() {

            foreach (var item in OemDevicesVModel.OemDevices)
            {
                yield return item.DevOem;
            }
            foreach (var item in OemDevicesVModel.OemDevicesExtra)
            {
                yield return item.DevOem;
            }
        }


        private OemDevicesViewModel oemDevicesVModel=new OemDevicesViewModel();
        internal OemDevicesViewModel OemDevicesVModel
        {
            get { return oemDevicesVModel; }
            set { oemDevicesVModel = value; 
                OnPropertyChanged("OemDevicesVModel"); }
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

        private string pc_adi;

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
