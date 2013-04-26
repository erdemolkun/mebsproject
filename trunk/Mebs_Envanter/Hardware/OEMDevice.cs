using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mebs_Envanter.GeneralObjects;
using System.Data.SqlClient;
using System.Data;
using Mebs_Envanter.DB;
using Mebs_Envanter.Hardware;
using Mebs_Envanter;

namespace Mebs_Envanter
{
    public class OEMDevice : MebsBaseObject
    {
        public override string ToString()
        {
            return DeviceInfo;
        }

        private int adet = 1;
        public int Adet
        {
            get { return adet; }
            set
            {
                if (value > 0)
                {
                    adet = value;
                    OnPropertyChanged("Adet");
                }
            }
        }

        public static List<OEMDevice> GetOemDevices(SqlConnection sqlCon, bool isForComputer, int bilgisayar_id, int _parca_id)
        {
            if (sqlCon == null) {
                throw new NullReferenceException("SqlConnection parameter is null");
            }
            List<OEMDevice> devModels = new List<OEMDevice>();
            SqlConnection cnn = sqlCon;//GlobalDataAccess.Get_Fresh_SQL_Connection();
            SqlCommand cmd = null;
            if (isForComputer)
            {
                String conString = "Select * From tbl_parca where bilgisayar_id=@bilgisayar_id";
                cmd = new SqlCommand(conString, cnn);
                cmd.Parameters.AddWithValue("@bilgisayar_id", bilgisayar_id);
            }
            else
            {
                String conString = "Select * From tbl_parca where parca_id=@parca_id";
                cmd = new SqlCommand(conString, cnn);
                cmd.Parameters.AddWithValue("@parca_id", _parca_id);
            }
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
                        int parca_adedi = DBValueHelpers.GetInt32(rowParca["parca_adedi"], 1);

                        OEMDevice devOem = null;
                        if (tip == DeviceTypes.MONITOR)
                        {
                            devOem = new Monitor();
                        }
                        else if (tip == DeviceTypes.PRINTER)
                        {
                            devOem = new YaziciInfo();
                        }
                        else
                        {
                            devOem = new OEMDevice(tip);
                        }

                        //Ortak alanlar

                        devOem.Id = parca_id;
                        devOem.SerialNumber = seri_no;
                        devOem.Parca_no = parca_no;
                        devOem.Marka = new Marka(markaid, "");
                        devOem.Tempest = new Tempest(tempestid, "");
                        devOem.DeviceInfo = parca_tanimi;
                        devOem.Adet = parca_adedi;
                        devModels.Add(devOem);
                    }

                    #endregion
                }
                catch (Exception)
                {
                    cnn.Close();
                    cnn.Dispose();
                }
                finally
                {                   
                }
            }
            return devModels;
        }

        public bool shouldUpdate = false;
        public OEMDevice() { }
        public OEMDevice(DeviceTypes devType)
        {

            DeviceType = devType;
        }

        private int id = -1;

        public int Id
        {
            get { return id; }
            set { id = value; OnPropertyChanged("Id"); }
        }

        private Marka marka;

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

        private String model = "";

        public String Model
        {
            get { return model; }
            set { model = value; OnPropertyChanged("Model"); }
        }

        private String deviceInfo;
        /// <summary>
        /// Parça Tanımı
        /// </summary>
        public String DeviceInfo
        {
            get { return deviceInfo; }
            set { deviceInfo = value; OnPropertyChanged("DeviceInfo"); }
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


        private String parca_no;

        public String Parca_no
        {
            get { return parca_no; }
            set { parca_no = value; OnPropertyChanged("Parca_no"); }
        }

        private DeviceTypes deviceType;
        public DeviceTypes DeviceType
        {
            get { return deviceType; }
            set
            {
                deviceType = value;
            }
        }
    }
}
