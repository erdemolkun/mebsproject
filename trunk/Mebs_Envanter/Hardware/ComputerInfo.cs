using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Data;
using Mebs_Envanter.GeneralObjects;
using System.Data.SqlClient;
using Mebs_Envanter.DB;
using Mebs_Envanter;
using System.Windows;
using Mebs_Envanter.Hardware;

namespace Mebs_Envanter
{
    public class ComputerInfo : MebsBaseObject
    {

        public override string ToString()
        {
            String str = "";
            str = Pc_adi;
            if (Marka != null && Marka.MarkaID > 0)
            {
                str += " , Marka : " + Marka.MarkaName;
            }
            return str;
        }


        CommandMap commands = null;
        public CommandMap Commands
        {
            get { return commands; }
            set { commands = value; }
        }

        public void Fetch()
        {
            if (!PropertiesFetched)
            {
                //tochange
                Set_ComputerOemDevices(null);
                Senet.Set_SenetInfos(Senet.Id);
                PropertiesFetched = true;
            }
        }

        public bool PropertiesFetched = false;

        public bool IsEdit = false;
        public ComputerInfo()
        {
            Commands = new CommandMap();
            NetworkInfo = new NetworkInfo();
            MonitorInfo = new Monitor();
            Senet = new SenetInfo();
            //EklenmeTarihi = new DateTime(2010, 9, 12);

            bool designTime = System.ComponentModel.DesignerProperties.GetIsInDesignMode(new DependencyObject());
            if (designTime)
            {
                Pc_adi = "PC_ADI_DESIGN_TIME";
            }
        }

        public ComputerInfo(Action<object> methodToExecuteOnDelete)
            : this()
        {

            Commands.AddCommand("Delete", methodToExecuteOnDelete);
        }


        public void SetGeneralFields(DataRow rowPC)
        {
            Id = (int)rowPC["bilgisayar_id"];
            
            int markaid = DBValueHelpers.GetInt32(rowPC["marka_id"].ToString(), -1);
            Marka = new Marka(markaid, "");

            int bagli_ag_id = DBValueHelpers.GetInt32(rowPC["bagli_ag_id"].ToString(), -1);


            NetworkInfo.BagliAg = new BagliAg("", bagli_ag_id);


            int tempest_id = DBValueHelpers.GetInt32(rowPC["tempest_id"].ToString(), -1);

            NetworkInfo.MacAddressString = rowPC["mac"].ToString();
            
            //tochange add senet_id
            Senet.Id = DBValueHelpers.GetInt32(rowPC["senet_id"], -1);

            Pc_adi = rowPC["pc_adi"].ToString();
            PcStokNo = rowPC["pc_stok_no"].ToString();
            Model = rowPC["model"].ToString();
            SerialNumber = rowPC["seri_no"].ToString();
            DeviceNo = rowPC["parca_no"].ToString();
            Notlar = rowPC["notlar"].ToString();
            Tempest = new Tempest(tempest_id, "");
            
            try
            {
                EklenmeTarihi = (DateTime)rowPC["kayit_ekleme_tarihi"];
            }
            catch (Exception)
            {
            }
        }

        internal void Set_MonitorInfo(OEMDevice devOem)
        {
            if (devOem==null || !(devOem is OEMDevice)) return;

            Monitor devMonitor = devOem as Monitor;

            SqlConnection cnn = GlobalDataAccess.Get_Fresh_SQL_Connection();
            String conString = "Select * From tbl_monitor where parca_id=@parca_id";
            SqlCommand cmd = new SqlCommand(conString, cnn);
            cmd.Parameters.AddWithValue("@parca_id", devMonitor.Id);
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
                foreach (DataRow rowMonitor in dt.Rows)
                {
                    String stok_no = rowMonitor["stok_no"].ToString();

                    int boyut_id = DBValueHelpers.GetInt32(rowMonitor["boyut_id"], -1);

                    int mon_id = (int)rowMonitor["monitor_id"];
                    int mon_type = DBValueHelpers.GetInt32(rowMonitor["monitor_tipi"], -1);

                    if (mon_type > 0)
                    {
                        devMonitor.MonType = (MonitorTypes)mon_type;
                    }
                    devMonitor.StokNo = stok_no;
                    devMonitor.Mon_id = mon_id;
                    devMonitor.MonSize = new MonitorSize(boyut_id, 0);
                }
            }
        }

        public void Set_ComputerOemDevices(SqlConnection sqlCon)
        {
            SqlConnection cnn = GlobalDataAccess.Get_Fresh_SQL_Connection();
            Set_HardwareInfos(cnn);
        }


        private void Set_HardwareInfos(SqlConnection sqlCon)
        {
            List<OEMDevice> devs = OEMDevice.GetOemDevicesDB(sqlCon, true, Id, -1);
            foreach (OEMDevice item in devs)
            {

                if (item is Monitor)
                {
                    Monitor mon = item as Monitor;
                    Set_MonitorInfo(mon);
                    MonitorInfo = mon;
                }

                else
                {
                    OemDevicesVModel.AssignOemDevice(item);
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

        private string pc_adi = "";
        /// <summary>
        /// Bilgisayar Adı
        /// </summary>
        public string Pc_adi
        {
            get { return pc_adi; }
            set
            {
                pc_adi = value;
                OnPropertyChanged("Pc_adi");
            }
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


        private DateTime? eklenmeTarihi = null;
        public DateTime? EklenmeTarihi
        {
            get { return eklenmeTarihi; }
            set { eklenmeTarihi = value; OnPropertyChanged("EklenmeTarihi"); }
        }

        #endregion
    }
}
