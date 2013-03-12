using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mebs_Envanter.GeneralObjects;

namespace MEBS_Envanter
{
    public class OEMDevice : MebsBaseObject
    {
        public bool shouldUpdate = false;
        public OEMDevice() { }
        public OEMDevice(DeviceTypes devType) {

            DeviceType = devType;
        }

        private int id=-1;

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


        private Tempest tempest=new Tempest();
        public Tempest Tempest
        {
            get { return tempest; }
            set { tempest = value; OnPropertyChanged("Tempest"); }
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
